namespace Dustech.IDP.Entities;

public class User 
{
    public Guid Id { get; set; }

    public string Subject { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool Active { get; set; }

    public string? Email { get; set; }

    public string? SecurityCode { get; set; }

    public DateTime SecurityCodeExpirationDate { get; set; }
    
    public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    // public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
    // public ICollection<UserSecret> Secrets { get; set; } = new List<UserSecret>();

}