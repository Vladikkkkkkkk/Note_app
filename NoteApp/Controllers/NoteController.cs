using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApp.Contracts;
using NoteApp.DataAccess;
using NoteApp.Models;

namespace NoteApp.Controllers;

[ApiController]
[Route("[controller]")]
public class NoteController : ControllerBase
{
    private readonly NoteDbContext _dbContext;
    
    public NoteController(NoteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request, CancellationToken ct)
     {
         var note = new Note(request.Title, request.Description);

         await _dbContext.Notes.AddAsync(note, ct);
         await _dbContext.SaveChangesAsync(ct);
         
         return Ok();
     }

     [HttpGet]
     public async Task<IActionResult> Get([FromQuery] GetNoteRequest request, CancellationToken ct)
     {
         var noteQuery = _dbContext.Notes
             .Where(n => string.IsNullOrWhiteSpace(request.Search) ||
                         n.Title.ToLower().Contains(request.Search.ToLower()));
         
         Expression<Func<Note, object>> selectorKey = request.SortItem?.ToLower( ) switch
         {
             "date" => note => note.CreateAt,
             "title" => note => note.Title,
             _ => note => note.Id
         };

         noteQuery = request.SortOrder == "desc" 
             ? noteQuery.OrderByDescending(selectorKey)
             : noteQuery.OrderBy(selectorKey);
         

         var noteDtos = await noteQuery
             .Select(n => new NoteDto(n.Id, n.Title, n.Description, n.CreateAt))
             .ToListAsync(cancellationToken: ct);
         
         var notes = await _dbContext.Notes.ToListAsync();


         return Ok(new GetNotesResponse(noteDtos));

     }
}
