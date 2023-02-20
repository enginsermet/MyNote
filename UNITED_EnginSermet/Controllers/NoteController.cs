using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UNITED_EnginSermet.Data;
using UNITED_EnginSermet.DTOs;
using UNITED_EnginSermet.Entities;
using UNITED_EnginSermet.Models;
using Newtonsoft.Json;

namespace UNITED_EnginSermet.Controllers
{
    public class NoteController : Controller
    {
        private readonly DataContext _context;

        public NoteController(DataContext context)
        {
            _context = context;
        }

        // GET: Note
        public async Task<IActionResult> Index(int? id, int? subNoteId)
        
        
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notes = await _context.Notes.Where(a => a.UserId == userId).ToListAsync();

            var subNotes = await _context.SubNotes.ToListAsync();

            //return View(await _context.Notes.Where(a => a.UserId == userId).ToListAsync());

            List<TreeViewNode> nodes = new List<TreeViewNode>();


            //Loop and add the Parent Nodes.
            foreach (var note in notes)
            {
                nodes.Add(new TreeViewNode { id = note.Id.ToString(), parent = "#", text = note.Title });
            }

            //Loop and add the Child Nodes.
            foreach (var subNote in subNotes)
            {
                nodes.Add(new TreeViewNode { id = subNote.NoteId.ToString() + "-" + subNote.Id.ToString(), parent = subNote.NoteId.ToString(), text = subNote.Title });
            }


            //Serialize to JSON string.
            string jsonString = JsonConvert.SerializeObject(nodes);
            ViewBag.Json = jsonString;

            if (subNoteId == null)
            {
                var selectedNote = await _context.Notes.SingleOrDefaultAsync(a => a.Id == id);
                if (selectedNote != null)
                {
                    NoteDTO noteDTO = new NoteDTO
                    {
                        Id = selectedNote.Id,
                        Title = selectedNote.Title,
                        Description = selectedNote.Description,
                        Date = selectedNote.Date
                    };
                    return View(noteDTO);
                }
                return View();
            }
            else
            {
                var selectedNote = await _context.SubNotes.SingleOrDefaultAsync(a => a.Id == subNoteId);

                if (selectedNote != null)
                {
                    NoteDTO noteDTO = new NoteDTO
                    {
                        Id = selectedNote.Id,
                        NoteId = selectedNote.NoteId,
                        Title = selectedNote.Title,
                        Description = selectedNote.Description,
                        Date = selectedNote.Date
                    };
                    return View(noteDTO);
                }

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string selectedItems)
        {
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);  
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DisplayNote(int? id)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("id", id);
            return RedirectToAction("Index", routeValues);
        }

        public async Task<IActionResult> DisplaySubNote(int? id)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("subNoteId", id);
            return RedirectToAction("Index", routeValues);
        }

        // GET: Note/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Note/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteDTO noteDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                Note note = new Note();
                note.UserId = userId;
                note.Title = noteDTO.Title;
                note.Description = noteDTO.Description;
                note.Date = DateTime.Now;
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult CreateNote(int id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(int id, NoteDTO noteDTO)
        {

            if (ModelState.IsValid)
            {
                SubNote note = new SubNote();
                note.NoteId = id;
                note.Title = noteDTO.Title;
                note.Description = noteDTO.Description;
                note.Date = DateTime.Now;
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Note/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Description,Date")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            return View(note);
        }

        // GET: Note/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notes == null)
            {
                return Problem("Entity set 'DataContext.Notes'  is null.");
            }
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("DeleteNote")]
        public async Task<IActionResult> DeleteNoteConfirmed(int id)
        {
            if (_context.Notes == null)
            {
                return Problem("Entity set 'DataContext.Notes'  is null.");
            }
            var note = await _context.SubNotes.FindAsync(id);
            if (note != null)
            {
                _context.SubNotes.Remove(note);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
          return _context.Notes.Any(e => e.Id == id);
        }
    }
}
