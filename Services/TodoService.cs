using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoRedis.Data;
using TodoRedis.Models;
using TodoRedis.Services.Caching;

namespace TodoRedis.Services
{
    
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;
        private readonly ICachingService _cachingService;

        public TodoService(TodoDbContext context, ICachingService cachingService)
        {
            _context = context;
            _cachingService = cachingService;
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {

            var list = await _cachingService.GetListAsync<Todo>("todos");

            if (list is null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                list = await _context.Todos.ToListAsync();
                _cachingService.SetAsync("todos", list);
            }

            return list;
        }

        public async Task<Todo> GetTodoAsync(int id)
        {
            var todo = await _cachingService.GetAsync<Todo>(id.ToString());

            if (todo is null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                todo = await _context.Todos.FindAsync(id);
                _cachingService.SetAsync(id.ToString(), todo);
            }

            return todo;
        }

        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }
    }
}