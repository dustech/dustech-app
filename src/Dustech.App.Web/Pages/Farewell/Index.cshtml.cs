using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dustech.App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.FSharp.Core;

namespace Dustech.App.Web.Pages.Farewell;

[Authorize]
public class IndexModel(Users.IUser users) : LayoutModel("Farewell", showMask: true)
{
    public IEnumerable<UserViewModel> FilteredUsers { get; private set; } = [];
    public Guid UserId { get; private set; } = Guid.Empty;
    public override void OnGet()
    {
        base.OnGet();
        var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var id = Guid.Empty;
        if (!string.IsNullOrEmpty(sub))
        {
            id = Guid.Parse(sub);
            UserId = id;
        }
        var query = new Users.UserQuery(null, null, UserId = id, isAdmin: User.IsInRole("Administrator"));

        var viewUsers = users.getUsers(query)
            .Select(u => new UserViewModel(u.Id, u.Name, u.LastName, u.Quote,u.PublicQuote ,u.Gender)).ToList();
        
        FilteredUsers = GetViewUsers(viewUsers);

        //.OrderBy(u => u.LastName);
    }

    private List<UserViewModel> GetViewUsers(List<UserViewModel> viewUsers)
    {
        viewUsers = viewUsers.OrderBy(u => u.LastName).ToList();
        var userToMove = viewUsers.FirstOrDefault(u => u.Id == UserId);
        if (userToMove != null) 
        {
            viewUsers.Remove(userToMove);
            viewUsers.Insert(0,userToMove);
        }

        return viewUsers;
    }

    internal static IEnumerable<CarouselViewModel> GetCarouselViewModels
    {
        get
        {
            var carouselData = new List<CarouselViewModel>
            {
                
                new("/media/carousel/start.jpg",
                    "Area quality",
                    "Entrare in area quality non vuol dire riuscire ad uscirne.",
                    "active"
                ),
                new("/media/carousel/mangiamo-insieme.jpg",
                    "Abbiamo condiviso tanto",
                    "Pizza, hamburger, birra, vino. Siamo stati tanto insieme."
                ),
                new("/media/carousel/flame.jpg",
                    "Otto di Marzo 2020",
                    "Il deploy di COM che ci fece a pezzi."
                ),
                new("/media/carousel/ict.jpg",
                    "ICT assurdi",
                    "Un cliente a dir poco particolare."
                ),
                new("/media/carousel/vpn.jpg",
                    "F5",
                    "Uniti nel dolore."
                ),
                new("/media/carousel/ceo.jpg",
                    "Importanti successi aziendali",
                    "I soldi vanno spesi bene."
                ),
                new("/media/carousel/save-deploy.jpg",
                    "Deploy automatici",
                    "Mai avuto problemi."
                ),
                new("/media/carousel/rafting.jpg",
                    "Estate 2023 - Rafting",
                    "Ci siamo divertiti!"
                ),
                new("/media/carousel/krav.jpg",
                    "Krav Maga",
                    "Jonny ci ha menato in sicurezza."
                ),
                new("/media/carousel/futuro.jpg",
                    "La nostra azienda",
                    "Ci amiamo incondizionatamente."
                ),
            };

            return carouselData;
        }
    }

    [SuppressMessage("Security", "CA5394:Do not use insecure randomness")]
    internal static IEnumerable<string> GetHobbies(IRequestCultureFeature? modelRequestCultureFeature)
    {
        var rand = new Random();
        var numberOfHobbies = rand.Next(1, 4);
        var randomHobbies = new List<string>();
        var culture = modelRequestCultureFeature?.RequestCulture.Culture.ToString() ?? "en";
        IEnumerable<string> hobbies;
        hobbies = culture == "it" ? Hobbies.ItHobbies.ToList() : Hobbies.EnHobbies.ToList();
        
        for (int i = 0; i < numberOfHobbies; i++)
        {
            var randomHobby = rand.Next(0, 50);
            randomHobbies.Add(hobbies.ElementAt(randomHobby));
        }

        return randomHobbies.ToHashSet();
    }

    [SuppressMessage("Security", "CA5394:Do not use insecure randomness")]
    internal static string GetCity(IRequestCultureFeature? modelRequestCultureFeature)
    {
        var rand = new Random();
        var cityNumber = rand.Next(0, 50);
        var culture = modelRequestCultureFeature?.RequestCulture.Culture.ToString() ?? "en";
        IEnumerable<string> cities;
        cities = culture == "it" ? Cities.ItCities.ToList() : Cities.EnCities.ToList();
        
        return cities.ElementAt(cityNumber);
    }
    
}

public record UserViewModel(Guid Id, string Name, string LastName, FSharpOption<string> Quote, FSharpOption<string> PublicQuote, Gender.Gender Gender)
{
    public string GenderString =>  Dustech.App.Domain.Gender.equal(Gender, "male")?"men":"women";

    public string FullName => $"{Name} {LastName}";
}

internal sealed record CarouselViewModel(
    string Path,
    string Title,
    string Description,
    string Active = "",
    string Context = "warning"
    );