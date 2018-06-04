using RentApp.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Attributes;

namespace RentApp.Controllers
{
    public class HomeController : Controller
    {
    
        public IUnitOfWork unitOfWork { get; set; }
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            /*foreach (var item in unitOfWork.AppUsers.GetAll())
            {
                Console.WriteLine(item.Name);
                try
                {
                    item.Name = "Tijana";
                    unitOfWork.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("I got u");
                }
            }*/
            

            return View();
        }
    }
}
