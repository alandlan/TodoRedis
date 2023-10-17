using TodoRedis.Models;

namespace TodoRedis.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetTodosAsync();
        Task<Todo> GetTodoAsync(int id);
        Task<Todo> CreateTodoAsync(Todo todo);
    }
}