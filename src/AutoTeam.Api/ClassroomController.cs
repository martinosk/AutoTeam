using System.Collections.Concurrent;
using AutoTeam.Domain.Model;
using AutoTeam.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace AutoTeam.Api;

[ApiController]
[Route("api/classroom")]
public class ClassroomController : ControllerBase
{
    private static readonly ConcurrentDictionary<Guid, ClassroomSession> Sessions = new();

    // POST /api/classroom/setup
    [HttpPost("setup")]
    public IActionResult Setup([FromBody] SetupRequest request)
    {
        if (request.Students == null || request.Students.Count == 0)
            return BadRequest(new ErrorResponse { Error = "At least one student is required." });

        if (request.Groups == null || request.Groups.Count == 0)
            return BadRequest(new ErrorResponse { Error = "At least one group is required." });

        // Create shared classification instances so reference equality works
        // within this classroom session.
        var male = Classification.Create(ClassificationEnum.Male);
        var female = Classification.Create(ClassificationEnum.Female);

        var strategy = new AssignToBestGroup();
        var groups = new Groups(strategy);
        var orderedGroups = new List<Group>();

        foreach (var gDef in request.Groups)
        {
            var group = new Group();
            if (gDef.MaleCapacity > 0)
                group.AddCapacity(new Capacity(male, gDef.MaleCapacity));
            if (gDef.FemaleCapacity > 0)
                group.AddCapacity(new Capacity(female, gDef.FemaleCapacity));
            groups.AddGroup(group);
            orderedGroups.Add(group);
        }

        var classroom = new Classroom(groups);

        foreach (var s in request.Students)
        {
            var classification = s.Classification?.Trim().Equals("Male", StringComparison.OrdinalIgnoreCase) == true
                ? male
                : female;

            var student = Student.WithName(s.Name).WithClassification(classification);
            classroom.AddStudent(student);
        }

        var sessionId = Guid.NewGuid();
        var session = new ClassroomSession(classroom, orderedGroups);
        Sessions[sessionId] = session;

        return Ok(new SetupResponse { SessionId = sessionId });
    }

    // POST /api/classroom/{sessionId}/assign
    [HttpPost("{sessionId:guid}/assign")]
    public IActionResult Assign(Guid sessionId)
    {
        if (!Sessions.TryGetValue(sessionId, out var session))
            return NotFound(new ErrorResponse { Error = "Session not found." });

        try
        {
            session.Classroom.AssignStudentsToGroups();
        }
        catch (ClassroomGroupCapacityException ex)
        {
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }

        return Ok(BuildGroupAssignmentResponse(session));
    }

    // POST /api/classroom/{sessionId}/accept
    [HttpPost("{sessionId:guid}/accept")]
    public IActionResult Accept(Guid sessionId)
    {
        if (!Sessions.TryGetValue(sessionId, out var session))
            return NotFound(new ErrorResponse { Error = "Session not found." });

        session.Classroom.AcceptAllGroups();
        session.RoundNumber++;

        return Ok(new AcceptResponse
        {
            Success = true,
            Message = $"Groups accepted. Now on round {session.RoundNumber}."
        });
    }

    // GET /api/classroom/{sessionId}/state
    [HttpGet("{sessionId:guid}/state")]
    public IActionResult State(Guid sessionId)
    {
        if (!Sessions.TryGetValue(sessionId, out var session))
            return NotFound(new ErrorResponse { Error = "Session not found." });

        return Ok(BuildGroupAssignmentResponse(session));
    }

    private static GroupAssignmentResponse BuildGroupAssignmentResponse(ClassroomSession session)
    {
        var result = new GroupAssignmentResponse();

        for (int i = 0; i < session.OrderedGroups.Count; i++)
        {
            var group = session.OrderedGroups[i];
            var groupResult = new GroupResult
            {
                GroupIndex = i,
                Members = group.CurrentMembers.Select(m => new MemberDto
                {
                    Name = m.Name,
                    Classification = m.Classification.Description
                }).ToList()
            };
            result.Groups.Add(groupResult);
        }

        return result;
    }
}
