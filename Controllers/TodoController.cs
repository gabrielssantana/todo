using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTodoBaltaIo.Data;
using MyTodoBaltaIo.Models;
using MyTodoBaltaIo.ViewModels;

namespace MyTodoBaltaIo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class TodoController: ControllerBase
    {
        [HttpGet]
        [Route("todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices]AppDbContext context)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .ToListAsync();
            return Ok(todos);
        }
        
        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices]AppDbContext context,
            [FromRoute]int id
            )
        {
            var todo = await context
                .Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id == id);
            return todo == null
                ? NotFound()
                : Ok(todo);
        }

        [HttpPost("todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model
        )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var newTodo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };
            
            try
            {
                await context
                    .Todos
                    .AddAsync(newTodo);
                await context
                    .SaveChangesAsync();
                return Created($"v1/todos/{newTodo.id}", newTodo);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpPut("todos/{id}")]
        public async Task<ActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id,
            [FromBody] CreateTodoViewModel model
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var todo = await context
                .Todos
                .FirstOrDefaultAsync(x => x.id == id);
            
            if (todo == null) 
                return NotFound();
            
            try
            {
                todo.Title = model.Title;
                context.Todos.Update(todo);
                await context.SaveChangesAsync();
                return Ok(todo);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("todos/{id}")]
        public async Task<ActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id
        )
        {
            var todo = await context
                .Todos
                .FirstOrDefaultAsync(x => x.id == id);

            if (todo == null)
                return NotFound();
            
            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}