using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
using ToDoList.Models;
using System.Data.Entity;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        //private sql9242975Entities1 db = new sql9242975Entities1();
        private FernleaEntities4 db = new FernleaEntities4();

        public TaskController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }
        
        public ActionResult Index() => RedirectToAction("List", "Task");

        public ActionResult List()
        {
            var q = db.Tasks.Include(x => x.Notes).Include(x => x.Priority);

            return View("List", q);
        }

        //Re-learned modal code from here https://www.c-sharpcorner.com/UploadFile/092589/implementing-modal-pop-up-in-mvc-application/
        public ActionResult PartialDetails(int id)
        {
            var q = db.Tasks.Where(x => x.Id == id).Include(x => x.Priority).Include(x => x.Notes).FirstOrDefault();

            return PartialView("PartialDetails", q);
        }

        public ActionResult Create()
        {
            Task t = new Task()
            {
                Id = 0,
                Date_Due = DateTime.Now
            };

            EditCreateViewbag("Create");

            return View("EditCreate", t);
        }

        public ActionResult Edit(int id = 1)
        {
            var q = db.Tasks.Where(x => x.Id == id).Include(x => x.Priority).Include(x => x.Notes).FirstOrDefault();
            //q.NotesList = q.Notes.ToList();
            EditCreateViewbag("Edit");
            return PartialView("EditCreate", q);
        }

        public ActionResult Delete(int id)//poor practice with GET, but in a rush
        {
            ModelState.Clear(); // Just in case

            Task task = db.Tasks.Find(id);

            if(task != null)
            {
                Delete(task);
            }

            return RedirectToAction("Index", "Task");
        }

        [HttpPost]
        public ActionResult EditCreateSave(Task task)
        {
            List<Note> notes = task.Notes.ToList();//ICollection can't do everything here

            //for(int i = 0; i < task.Notes.Count; ++i)
            //{
            //    task.Notes[i].
            //}

            //deal with task
            task.Notes = new HashSet<Note>();
            if (task.Id <= 0)
            {
                Add(task);//Id will be changed. Can use that to add/update/delete Notes
            }
            else
            {
                Update(task);
            }

            //deal with notes
            //pointless to keep the ones they wanted deleted that were newly added
            notes = notes.Where(x => !(x.Id == 0 && x.Delete)).ToList();
            //sets the foreign key for the new ones. Doesn't matter if it overwrites existing ones, since it's the same value.
            notes.ForEach(x => x.Task_Id = task.Id);

            //delete where Delete is true
            DeleteNotes(notes.Where(x => x.Delete).ToList());
            //get rid of ^
            notes = notes.Where(x => !x.Delete).ToList();

            //update/add the rest
            UpdateNotes(notes.Where(x => x.Id != 0).ToList());
            AddNotes(notes.Where(x => x.Id == 0).ToList());

            return RedirectToAction("Index");
        }

        #region database
        //takes care of foreign key dependencies
        private void Delete(Task task)
        {
            DeleteNotes(task.Id);

            db.Tasks.Attach(task);
            db.Tasks.Remove(task);
            db.SaveChanges();
        }

        private void Update(Task task)
        {
            //db.Entry(task.Notes).State = EntityState.Unchanged;//will deal with these after

            db.Tasks.Attach(task);
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void Add(Task task)
        {
            db.Tasks.Add(task);
            db.SaveChanges();
        }

        private void DeleteNotes(int taskId)
        {
            List<Note> q = db.Notes.Where(x => x.Task_Id == taskId).ToList();//Have to make it a list, can't iterate through and delete at the same time

            DeleteNotes(q);
        }

        private void DeleteNotes(List<Note> notes)
        {
            foreach (Note note in notes)
            {
                DeleteNote(note);
            }
        }

        private void DeleteNote(Note note)
        {
            db.Notes.Attach(note);
            db.Notes.Remove(note);
            db.SaveChanges();
        }

        private void AddNotes(List<Note> notes)
        {
            foreach(Note n in notes)
            {
                db.Notes.Add(n);
                db.SaveChanges();
            }
        }

        private void UpdateNotes(List<Note> notes)
        {
            foreach (Note n in notes)
            {
                db.Notes.Attach(n);
                db.Entry(n).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        #endregion

        //Creates the dropdownlist options and stores them in the viewbag, as well as another option(s)
        private void EditCreateViewbag(string topText)
        {
            var tmpItems = db.Priorities;
            List<SelectListItem> items = tmpItems.Select(x => new SelectListItem { Text = x.Priority_Type, Value = x.Id.ToString() }).ToList();

            ViewBag.Priorities = items;

            ViewBag.h2 = topText;
        }



        
    }
}