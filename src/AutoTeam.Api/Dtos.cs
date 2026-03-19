namespace AutoTeam.Api;

// ── Request DTOs ──

public class SetupRequest
{
    public List<StudentDto> Students { get; set; } = new();
    public List<GroupDefinition> Groups { get; set; } = new();
}

public class StudentDto
{
    public string Name { get; set; } = string.Empty;
    public string Classification { get; set; } = string.Empty;
}

public class GroupDefinition
{
    public int MaleCapacity { get; set; }
    public int FemaleCapacity { get; set; }
}

// ── Response DTOs ──

public class SetupResponse
{
    public Guid SessionId { get; set; }
}

public class GroupAssignmentResponse
{
    public List<GroupResult> Groups { get; set; } = new();
}

public class GroupResult
{
    public int GroupIndex { get; set; }
    public List<MemberDto> Members { get; set; } = new();
}

public class MemberDto
{
    public string Name { get; set; } = string.Empty;
    public string Classification { get; set; } = string.Empty;
}

public class AcceptResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
}
