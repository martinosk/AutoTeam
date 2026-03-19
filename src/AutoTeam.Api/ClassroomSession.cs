using AutoTeam.Domain.Model;

namespace AutoTeam.Api;

/// <summary>
/// Holds a Classroom and a stable-ordered list of its groups so we can
/// return consistent group indices through the API.
/// </summary>
public class ClassroomSession
{
    public Classroom Classroom { get; }
    public List<Group> OrderedGroups { get; }
    public int RoundNumber { get; set; } = 1;

    public ClassroomSession(Classroom classroom, List<Group> orderedGroups)
    {
        Classroom = classroom;
        OrderedGroups = orderedGroups;
    }
}
