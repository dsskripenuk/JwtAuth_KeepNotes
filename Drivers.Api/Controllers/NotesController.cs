using Drivers.Api.Interfaces.Services.IRepositories;
using Drivers.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Drivers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public NotesController(INoteRepository noteRepository, UserManager<IdentityUser> userManager)
        {
            _noteRepository = noteRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetNotes()
        {
            var userId = _userManager.GetUserId(User);
            var notes = _noteRepository.GetNotes(userId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public IActionResult GetNoteById(int id)
        {
            var userId = _userManager.GetUserId(User);
            var note = _noteRepository.GetNoteById(id, userId);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public IActionResult AddNote([FromBody] Note note)
        {
            var userId = _userManager.GetUserId(User);
            note.UserId = userId;

            Console.WriteLine($"Received Note: {note.Title}, {note.Content}, {note.UserId}");

            _noteRepository.AddNote(note);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, [FromBody] Note note)
        {
            var userId = _userManager.GetUserId(User);
            var existingNote = _noteRepository.GetNoteById(id, userId);

            if (existingNote == null)
            {
                return NotFound();
            }

            if (existingNote.UserId != userId)
            {
                return Forbid();
            }

            existingNote.Title = note.Title;
            existingNote.Content = note.Content;

            _noteRepository.UpdateNote(existingNote);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var userId = _userManager.GetUserId(User);
            _noteRepository.DeleteNote(id, userId);
            return NoContent();
        }
    }
}
