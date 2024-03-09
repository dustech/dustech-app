using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dustech.App.Domain;

namespace Dustech.App.Web.Pages.Farewell;

public class IndexModel(Users.IUser users) : LayoutModel("Farewell", showMask: true)
{
    public IEnumerable<UserViewModel> FilteredUsers { get; private set; } = [];

    public override void OnGet()
    {
        base.OnGet();

        var query = new Users.UserQuery(null, null);

        FilteredUsers = users.getUsers(query).Select(u => new UserViewModel(u.Name, u.Gender.ToString()));
    }

    internal static IEnumerable<CarouselViewModel> GetCarouselViewModels
    {
        get
        {
            var carouselData = new List<CarouselViewModel>
            {
                new("/media/carousel/start.jpg",
                    "Gli inizi",
                    "Quando approdi ad area quality.",
                    "warning",
                    "active"
                ),
                new("/media/carousel/flame.jpg",
                    "Otto di Marzo 2020",
                    "l giorno in cui tutto ando' a meraviglia."
                ),
                new("/media/carousel/ict.jpg",
                    "ICT deliziosi",
                    "Perche uccidere il fornitore diverte sempre."
                ),
                new("/media/carousel/save-deploy.jpg",
                    "Deploy automatici",
                    "Mai avuto problemi."
                ),
                new("/media/carousel/rafting.jpg",
                    "Estate 2023",
                    "Ci siamo anche divertiti ogni tanto."
                ),
                new("/media/carousel/futuro.jpg",
                    "Il futuro",
                    "W il DEI."
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

public record UserViewModel(string Name, string Gender)
{
    [SuppressMessage("Security", "CA5394:Do not use insecure randomness")]
    public string GetQuote()
    {
        List<string> saluti = new List<string>
        {
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aliquid commodi consectetur deleniti dolorem exercitationem fugit libero nihil numquam, officia quia.",
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Libero molestiae odio quibusdam recusandae, tenetur vel?",
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Consequuntur corporis distinctio ea eaque facere fuga illum ipsum laborum minima minus mollitia nemo nostrum officia officiis quam quos similique sit, tempora.",
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. A ab accusamus alias aliquam animi aspernatur assumenda doloribus ducimus enim est eum excepturi exercitationem expedita illum in minus natus necessitatibus nihil officia, porro provident quasi qui repudiandae sapiente similique temporibus, ullam vero. Id in placeat quis!"
        };

        var rand = new Random();
        return saluti[rand.Next(saluti.Count)];
    }
};

internal sealed record CarouselViewModel(
    string Path,
    string Title,
    string Description,
    string Context = "warning",
    string Active = "");