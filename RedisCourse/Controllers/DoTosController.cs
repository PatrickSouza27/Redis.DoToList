using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RedisCourse.Core.Entities;
using RedisCourse.Infrastructure.Caching.Interfaces;
using RedisCourse.Infrastructure.Data;
using RedisCourse.ViewModels;

namespace RedisCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoTosController : ControllerBase
{
    private readonly ToDoListDbContext _context;
    private readonly ICachingService _cache;
    
    
    public DoTosController(ToDoListDbContext context, ICachingService cache)
    {
        _context = context;
        _cache = cache;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todoCache = await _cache.GetAsync($"todo_{id}");

        if (!string.IsNullOrWhiteSpace(todoCache))
        {
            var todo = JsonConvert.DeserializeObject<ToDo>(todoCache);
            return Ok(todo);
        }
        
        var todos = await _context.ToDos.SingleOrDefaultAsync(x=> x.Id == id);

        if (todos is null)
            return NotFound();
        
        await _cache.SetAsync($"todo_{id}", JsonConvert.SerializeObject(todos));
        
        return Ok(todos);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToDoInputViewModel model)
    {
        var todo = new ToDo(0, model.Title, model.Description);
        await _context.ToDos.AddAsync(todo);
        await _context.SaveChangesAsync();
        
        
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }
    
    
}