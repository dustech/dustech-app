using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dustech.App.Domain;
using Microsoft.AspNetCore.Authorization;
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
    internal static IEnumerable<string> GetHobbies()
    {
        var rand = new Random();
        var numberOfHobbies = rand.Next(1, 4);
        var randomHobbies = new List<string>();
        for (int i = 0; i < numberOfHobbies; i++)
        {
            var randomHobby = rand.Next(0, 50);
            randomHobbies.Add(Hobbies.ElementAt(randomHobby));
        }

        return randomHobbies.ToHashSet();
    }

    [SuppressMessage("Security", "CA5394:Do not use insecure randomness")]
    internal static string GetCity()
    {
        var rand = new Random();
        var cityNumber = rand.Next(0, 50);
        return Cities.ElementAt(cityNumber);
    }

    internal static IEnumerable<string> Hobbies =>
        new List<string>
        {
            "Reading",
            "Writing",
            "Gardening",
            "Cooking",
            "Baking",
            "Painting",
            "Drawing",
            "Knitting",
            "Sewing",
            "Crocheting",
            "Quilting",
            "Scrapbooking",
            "Photography",
            "Videography",
            "Birdwatching",
            "Hiking",
            "Camping",
            "Fishing",
            "Hunting",
            "Kayaking",
            "Canoeing",
            "Sailing",
            "Surfing",
            "Snowboarding",
            "Skiing",
            "Ice Skating",
            "Rollerblading",
            "Skateboarding",
            "Dancing",
            "Yoga",
            "Pilates",
            "Running",
            "Jogging",
            "Cycling",
            "Swimming",
            "Bodybuilding",
            "Martial Arts",
            "Boxing",
            "Golfing",
            "Tennis",
            "Bowling",
            "Archery",
            "Chess",
            "Sudoku",
            "Playing",
            "Singing",
            "Acting",
            "Stand-up comedy",
            "Magic",
            "Collecting"
        };


    internal static IEnumerable<string> Cities =>
        new List<string>
        {
            "Tokyo, JP",
            "Delhi, IN",
            "Shanghai, CN",
            "São Paulo, BR",
            "Città del Messico, MX",
            "Il Cairo, EG",
            "Mumbai, IN",
            "Pechino, CN",
            "Dacca, BD",
            "Osaka, JP",
            "New York, US",
            "Karachi, PK",
            "Buenos Aires, AR",
            "Chongqing, CN",
            "Istanbul, TR",
            "Kolkata, IN",
            "Manila, PH",
            "Lagos, NG",
            "Rio de Janeiro, BR",
            "Tianjin, CN",
            "Kinshasa, CD",
            "Guangzhou, CN",
            "Los Angeles, US",
            "Mosca, RU",
            "Shenzhen, CN",
            "Lahore, PK",
            "Bangalore, IN",
            "Parigi, FR",
            "Londra, GB",
            "Lima, PE",
            "Chengdu, CN",
            "Johannesburg, ZA",
            "Baghdad, IQ",
            "Toronto, CA",
            "Santiago, CL",
            "Madrid, ES",
            "Yangon, MM",
            "Alessandria, EG",
            "Houston, US",
            "Mumbai, IN",
            "Hangzhou, CN",
            "Quanzhou, CN",
            "Bangkok, TH",
            "Hong Kong, HK",
            "Dhaka, BD",
            "Hyderabad, IN",
            "Wuhan, CN",
            "Pune, IN",
            "Riyadh, SA",
            "Chennai, IN"
        };
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