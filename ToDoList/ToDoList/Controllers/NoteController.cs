using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class NoteController : Controller
    {
        private FernleaEntities4 db = new FernleaEntities4();

        public NoteController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: Note
        public ActionResult Index() => View();

        public ActionResult PartialEdit()
        {
            Note note = new Note()
            {
                Id = 0,
                Task_Id = 0
            };
            return PartialView("PartialEdit", note);
        }

        public ActionResult PartialList(int id)
        {
            var q = db.Notes.Where(x => x.Task_Id == id);

            return PartialView(q);
        }
    }
}