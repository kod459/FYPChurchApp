using PIMS.DataAccess;
using System.Linq;
using System.Web.Mvc;

namespace PIMS.Controllers
{
    public class HomeController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to Our Church for Kilnamanagh Castleview!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}
