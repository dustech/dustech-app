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

    internal static IEnumerable<CarouselViewModel> GetCarouselViewModels
    {
        get
        {
            var carouselData = new List<CarouselViewModel>
            {
                new ("/media/carousel/start.jpg",
                    "Gli inizi",
                    "Quando approdi ad area quality.",
                    "warning",
                    "active"
                ),
                new ("/media/carousel/flame.jpg",
                "Otto di Marzo 2020",
                "l giorno in cui tutto ando' a meraviglia."
                ),
                new ("/media/carousel/ict.jpg",
                    "ICT deliziosi",
                    "Perche uccidere il fornitore diverte sempre."
                ),
                new ("/media/carousel/save-deploy.jpg",
                    "Deploy automatici",
                    "Mai avuto problemi."
                ),
                new ("/media/carousel/rafting.jpg",
                    "Estate 2023",
                    "Ci siamo anche divertiti ogni tanto."
                ),
                
            };

            return carouselData;
        }
    }
}

public record UserViewModel(string Name, string Gender);

internal sealed record CarouselViewModel(
    string Path, 
    string Title, 
    string Description, 
    string Context = "warning",
    string Active = "");