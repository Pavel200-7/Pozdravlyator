using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Data;
using Pozdravlyator.Models;
using Pozdravlyator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Pozdravlyator.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PozdravlyatorContext _context;
        private readonly IImageWorker _imageWorker;

        public PeopleController(PozdravlyatorContext context, IImageWorker imageWorker)
        {
            _context = context;
            _imageWorker = imageWorker;
        }

        // GET: People
        public async Task<IActionResult> Index(PersonSortState sortOrder = PersonSortState.IdAsk)
        {
            IQueryable<Person> people = _context.Person;

            ViewData["IdSort"] = sortOrder == PersonSortState.IdAsk ? PersonSortState.IdDesc : PersonSortState.IdAsk;
            ViewData["NameSort"] = sortOrder == PersonSortState.NameAsk ? PersonSortState.NameDesc : PersonSortState.NameAsk;
            ViewData["BirthDaySort"] = sortOrder == PersonSortState.BirthDayAsk ? PersonSortState.BirthDayDesc : PersonSortState.BirthDayAsk;

            people= sortOrder switch
            {
                PersonSortState.IdDesc => people.OrderByDescending(s => s.Id),
                PersonSortState.NameAsk => people.OrderBy(s => s.Name),
                PersonSortState.NameDesc => people.OrderByDescending(s => s.Name),
                PersonSortState.BirthDayAsk => people.OrderBy(s => s.BirthDay),
                PersonSortState.BirthDayDesc => people.OrderByDescending(s => s.BirthDay),
                _ => people.OrderBy(s => s.Id),
            };


            return View(await people.AsNoTracking().ToListAsync());

            //return View(await _context.Person.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDay")] Person person, IFormFile? PhotoFile)
        {
            if (ModelState.IsValid)
            {
                if (PhotoFile != null)
                {
                    string newUniqueFileName = _imageWorker.CreateImageFile(ref PhotoFile);
                    person.PicturePath = newUniqueFileName;
                } else
                {
                    person.PicturePath = null;
                }
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthDay,PicturePath")] Person person, IFormFile? PhotoFile)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try {                
                    if (PhotoFile != null && person.PicturePath != null)
                    {
                        person.PicturePath = _imageWorker.UpdateImageFile(ref PhotoFile, person.PicturePath);
                    } else if (PhotoFile != null && person.PicturePath == null)
                    {
                        person.PicturePath = _imageWorker.CreateImageFile(ref PhotoFile);
                    }

                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                if (person.PicturePath != null)
                {
                    _imageWorker.DeleteImageFile(person.PicturePath);
                }
                _context.Person.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
