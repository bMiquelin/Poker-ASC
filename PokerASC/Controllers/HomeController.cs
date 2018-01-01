using PokerASC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokerASC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            var salasAtivas = db.Sala.Where(j => j.Ativo).ToList();
            return View(salasAtivas);
        }
    }
}