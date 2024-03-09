using System.Collections.Generic;
using System.Linq;
using Dustech.App.Domain;

namespace Dustech.App.Web.Pages.Farewell;

public class IndexModel(Users.IUser users) : LayoutModel("Farewell", showMask: true)
{
    public IEnumerable<UserViewModel> FilteredUsers { get; private set; } = [];

    public override void OnGet()
    {
        base.OnGet();

        var query = new Users.UserQuery(null, "male");

        FilteredUsers = users.getUsers(query).Select(u => new UserViewModel(u.Name, u.Gender.ToString()));
    }
}

public record UserViewModel(string Name, string Gender);