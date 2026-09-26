using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Models;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers;

public class HomeController : Controller
{

    private readonly IMenuApiClient _menuApiClient;
    public HomeController(IMenuApiClient menuApiClient) { _menuApiClient = menuApiClient; }

    public IActionResult Index()
    {
        return View();
    }

      private List<StoreLocation> GetStoreLocations()
        {
            return new List<StoreLocation>
            {
                new StoreLocation
                {
                    Name = "Noosh Saxony Westwood Mall",
                    MallName = "Saxony Westwood Mall",
                    Address = "Westwood Mall, Lincoln Terrace, Westville, Durban",
                    PhoneNumber = "087 226 6674",
                    HoursSchedule = new List<StoreHoursLine>
                    {
                        new StoreHoursLine { Label = "Sun - Mon", Time = "10:00 - 20:00" },
                        new StoreHoursLine { Label = "Fri", Time = "Closed 12:15 - 13:15", IsClosure = true }
                    },
                    MapEmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3461.248254569304!2d30.961017199999997!3d-29.8282552!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x1ef7019e07301083%3A0x17534deb24a7a4ad!2sNoosh%20Saxony%20Westwood%20Mall!5e0!3m2!1sen!2sza!4v1784144426551!5m2!1sen!2sza",
                    Latitude = -29.82560417513887,
                    Longitude = 30.96048452258684,
                    UberEatsUrl = "https://www.ubereats.com/za/store/noosh-saxony-westwood/nsuGbWvGSqSQOUj1JqDKkw?ps=1",
                    MrDUrl = "https://www.mrd.com/delivery/restaurant/noosh-westwood-mall-sherwood/28174"
                },
                  new StoreLocation
                {
                    Name = "Noosh Florida Road",
                    MallName = "Musgrave Centre",
                    Address = "279 Florida Rd, Windermere, Berea",
                    PhoneNumber = "087 226 6674",
                    HoursSchedule = new List<StoreHoursLine>
                    {
                        new StoreHoursLine { Label = "Sun - Thu", Time = "11:00 - 20:00" },
                        new StoreHoursLine { Label = "Fri", Time = "13:00 - 22:00" },
                        new StoreHoursLine { Label = "Sat", Time = "11:00 - 22:00" }
                    },
                    MapEmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3461.177084092737!2d31.012795600000008!3d-29.830309900000003!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x1ef707005debfad1%3A0xbda15fc83aa5af0c!2sNoosh%20Florida%20Road!5e0!3m2!1sen!2sza!4v1784145278083!5m2!1sen!2sza",
                    Latitude = -29.825767122943052,
                    Longitude = 31.012779506706707,
                    UberEatsUrl = "https://www.ubereats.com/za/store/noosh-florida-square/U1samMrsX6G2XoWoBu_Qdg?ps=1",
                    MrDUrl = "https://www.mrd.com/delivery/restaurant/noosh-florida-square-morningside/34184"
                }
            };
        }








    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
