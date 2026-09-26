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
                }

            };
        }



    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
