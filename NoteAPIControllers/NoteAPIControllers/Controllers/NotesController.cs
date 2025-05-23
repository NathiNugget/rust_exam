using Microsoft.AspNetCore.Mvc;
using NoteAPIControllers.Models;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;


namespace NoteAPIControllers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly ConcurrentBag<string> _notes; 


        public NotesController(ConcurrentBag<string> notes)
        {
            _notes = notes;
        }

        [HttpGet]
        public IActionResult GetNotes()
        {
            return Ok(_notes);
        }

        [HttpPost]
        public IActionResult PostNote(MessageDTO dto)
        {
            _notes.Add(dto.msg);
            return NoContent();
        }
    }
}


