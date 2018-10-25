using System.Web.Mvc;
using System.Collections.Generic;
using Sklep_CMS.Models.ViewModels.Pages;
using Sklep_CMS.Models.Data;
using System.Linq;

namespace Sklep_CMS.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> pagesList;

            using (CmsDataBase db = new CmsDataBase())
            {
                //list initialization
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //return page to View
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        public ActionResult AddPage()
        {
            return View();
        }
    }
}