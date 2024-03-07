using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// LLama el Método GET para la Pantalla de Inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }
    }
}