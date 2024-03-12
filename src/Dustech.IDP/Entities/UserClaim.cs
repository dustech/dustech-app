namespace Dustech.IDP.Entities;

public class UserClaim 
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

   
    public Guid UserId { get; set; }

    public User User { get; set; }
}