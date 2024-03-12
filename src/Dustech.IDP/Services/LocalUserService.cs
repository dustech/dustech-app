using Dustech.App.DAL;
using Dustech.App.Infrastructure;
using Dustech.IDP.Entities;

namespace Dustech.IDP.Services;

public class LocalUserService : ILocalUserService
{
    private User ToIdpUser(Auth.User user)
    {
        return new User
        {
            Id = user.Id,
            Subject = user.Sub.ToString(),
            UserName = user.Username,
            Active = user.Active
        };
    }
    private UserClaim ToIdpUserClaim(Auth.UserClaim claim)
    {
        return new UserClaim
        {
            Id = claim.Id,
            Type = claim.Type,
            Value = claim.Value,
            UserId = claim.UserId,
            ConcurrencyStamp = claim.ConcurrentStamp
            
        };
    }
    public Task<bool> ValidateCredentialsAsync(string userName, string password)
    {
        
        var conData = ConnectionData();
        var authInPostgres = (Auth.IIdentity)AuthInDatabase.toAuthInPostgres(conData);
        var query = new Auth.IdentityQuery(id:null, username: userName, password: password, sub: null);
        authInPostgres.getIdentities(query);
        return Task.FromResult(authInPostgres.Any());
    }

    public Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject)
    {
        var conData = ConnectionData();
        var authInPostgres = (Auth.IIdentity)AuthInDatabase.toAuthInPostgres(conData);
        var query = new Auth.IdentityQuery(id:null, username: null, password: null , sub: subject);
        var identities = authInPostgres.getIdentities(query);
        var claims = identities
            .Select(i => i.UserClaims);
        var idpClaims = claims.SelectMany(cs=> cs).Select(ToIdpUserClaim);
        
        return Task.FromResult(idpClaims);
    }

    public Task<User> GetUserByUserNameAsync(string userName)
    {
        var conData = ConnectionData();
        var authInPostgres = (Auth.IIdentity)AuthInDatabase.toAuthInPostgres(conData);
        var query = new Auth.IdentityQuery(id:null, username: userName, password: null, sub:null);
        var users= authInPostgres.getIdentities(query);
        return Task.FromResult(
            users.Select(u => ToIdpUser(u.User)).FirstOrDefault());
    }

    private static Common.ConnectionData ConnectionData()
    {
        var pwd = Environment.GetEnvironmentVariable("PSLpwd");
        var usr = Environment.GetEnvironmentVariable("PSLusr");
        var host = Environment.GetEnvironmentVariable("PSLhost");
        var database = Environment.GetEnvironmentVariable("PSLdatabase");


        var conData = new Common.ConnectionData(password: pwd, username: usr, host: host, database: database);
        return conData;
    }

    
}