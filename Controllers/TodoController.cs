using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoRedis.Services;

namespace TodoRedis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var todos = await _service.GetTodosAsync();
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            
            return Ok(new{duration = new {milliseconds = ts.Microseconds, seconds = ts.Seconds}, todos});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoAsync(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var todo = await _service.GetTodoAsync(id);
            if (todo is null)
            {
                return NotFound();
            }

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            return Ok(new{duration = new {milliseconds = ts.Microseconds, seconds = ts.Seconds}, todo});
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync([FromBody] TodoRedis.Models.Todo todo)
        {
            var createdTodo = await _service.CreateTodoAsync(todo);
            return Created($"/api/todo/{createdTodo.Id}", createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoAsync(int id, [FromBody] TodoRedis.Models.Todo todo)
        {
            var existingTodo = await _service.GetTodoAsync(id);
            if (existingTodo is null)
            {
                return NotFound();
            }

            existingTodo.Descricao = todo.Descricao;
            existingTodo.Concluida = todo.Concluida;

            await _service.UpdateTodoAsync(existingTodo);

            return NoContent();
        }
    }
}