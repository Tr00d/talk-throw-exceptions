namespace EmployeeOnboarding.Models;

public record Department(string Name);

public record Candidate(string Name, string Email);

public record Contract(string CandidateName, string Email);

public record Account(string Name, string Login);
