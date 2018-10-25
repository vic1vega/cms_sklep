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
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //form model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (CmsDataBase db = new CmsDataBase())
            {

                string slug;
                
                //initalization PageDTO
                PageDTO dto = new PageDTO();

                //slug is Empty - assign title to slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //this same slug exsist
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError(key: "", errorMessage: "Tytuł tworzonej strony lub jej adres już istnieje w sklepie");
                    return View(model);
                }

                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 1000;

                //save DTO to DB
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            TempData["SM"] = "Dodałeś nową stronę";

            return RedirectToAction("AddPage");
        }
    }

}