using System;
using System.Collections.Generic;
using System.Linq;
using Dustech.App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dustech.App.Web.Pages.Farewell;


public class IndexModel(Users.IUser users) : LayoutModel("Farewell")
{
    public IEnumerable<UserViewModel> FilteredUsers { get; private set; } = [];
    public override void OnGet()
    {
        base.OnGet();

        var query = new Users.UserQuery(null,null);
        
        FilteredUsers = users.getUsers(query).Select(u => new UserViewModel(u.Name,u.Gender));

    }

}


public record UserViewModel(string Name, string Gender);