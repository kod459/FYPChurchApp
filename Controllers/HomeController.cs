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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetEvents()
        {
            ChurchDBContext db = new ChurchDBContext();
            var events = db.Appointments.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
