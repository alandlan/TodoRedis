using Microsoft.AspNetCore.Mvc;
using TodoRedis.Services;
using TodoRedis.Services.Caching;

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
            var todos = await _service.GetTodosAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoAsync(int id)
        {
            var todo = await _service.GetTodoAsync(id);
            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync([FromBody] TodoRedis.Models.Todo todo)
        {
            var createdTodo = await _service.CreateTodoAsync(todo);
            return Created($"/api/todo/{createdTodo.Id}", createdTodo);
        }
    }
}