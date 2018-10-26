using System.Web.Mvc;
using System.Collections.Generic;
using Sklep_CMS.Models.ViewModels.Pages;
using Sklep_CMS.Models.Data;
using System.Linq;
using System.Web.ModelBinding;

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

        // GET: Admin/Pages/Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            PageVM model;

            using (CmsDataBase db = new CmsDataBase())
            {
                //get DTO page with current id
                PageDTO dto = db.Pages.Find(id);

                //check that page 
                if (dto == null)
                {
                    return Content("Strona nie istnieje");
                }

                //assign page to view
                model = new PageVM(dto);
            }

            return View(model);
        }

        // POST: Admin/Pages/Edit/id
        [HttpPost]
        public ActionResult Edit(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (CmsDataBase db = new CmsDataBase())
            {
                //get page id
                int id = model.Id;

                string slug = "home";

                //get pages to edit
                PageDTO dto = db.Pages.Find(id);

                //initalization PageDTO
                dto.Title = model.Title;

                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //this same page already exsist
                //do poprawy - nie rozpoznaje takiego samego adresu strony ("home")
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Strona lub adres strony już istnieje");
                }

                //assign model to DTO
                dto.Title = model.Title;
                dto.Slug = model.Slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //save model to DTO
                db.SaveChanges();
            }

            //set up TempData
            TempData["SM"] = "Zmiany zostały zapisane";

            return RedirectToAction("Edit");
        }

        // GET: Admin/Pages/Details/id
        [HttpGet]
        public ActionResult Details(int id)
        {
            //PageVm declaration
            PageVM model;

            using (CmsDataBase db = new CmsDataBase())
            {
                //get pages with id
                PageDTO dto = db.Pages.Find(id);

                if (dto == null)
                {
                    return Content("Wybrana strona nie istnieje");
                }

                //PageVM initialization
                model = new PageVM(dto);
            }

            return View(model);
        }

        // GET: Admin/Pages/Delete/id
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (CmsDataBase db = new CmsDataBase())
            {
                //get page to delete
                PageDTO dto = db.Pages.Find(id);

                //delete selected page from db
                db.Pages.Remove(dto);

                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }
    }

}