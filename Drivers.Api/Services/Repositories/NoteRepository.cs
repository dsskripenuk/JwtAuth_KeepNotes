using Drivers.Api.Data;
using Drivers.Api.Interfaces.Services.IRepositories;
using Drivers.Api.Models;

namespace Drivers.Api.Services.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Note> GetNotes(string userId)
        {
            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        public Note GetNoteById(int id, string userId)
        {
            return _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
        }

        public void AddNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void UpdateNote(Note note)
        {
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == note.Id && n.UserId == note.UserId);
            if (existingNote != null)
            {
                existingNote.Title = note.Title;
                existingNote.Content = note.Content;
                _context.SaveChanges();
            }
        }
        public void DeleteNote(int id, string userId)
        {
            var noteToRemove = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (noteToRemove != null)
            {
                _context.Notes.Remove(noteToRemove);
                _context.SaveChanges();
            }
        }
    }
}
