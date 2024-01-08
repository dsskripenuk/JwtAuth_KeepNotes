using Drivers.Api.Models;

namespace Drivers.Api.Interfaces.Services.IRepositories
{
    public interface INoteRepository
    {
        List<Note> GetNotes(string userId);
        Note GetNoteById(int id, string userId);
        void AddNote(Note note);
        void UpdateNote(Note note);
        void DeleteNote(int id, string userId);
    }
}
