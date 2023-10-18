using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoRedis.Data;
using TodoRedis.Helpers;
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
            var key = RedisName.GetObjectKey<Todo>("all");

            var list = await _cachingService.GetListAsync<Todo>(key);

            if (list is null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                list = await _context.Todos.ToListAsync();
                await _cachingService.SetAsync(key, list);
            }

            return list;
        }

        public async Task<Todo> GetTodoAsync(int id)
        {
            var key = RedisName.GetObjectKey<Todo>(id.ToString());

            var todo = await _cachingService.GetAsync<Todo>(key);

            if (todo is null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                todo = await _context.Todos.FindAsync(id);
                await _cachingService.SetAsync(key, todo);
            }

            return todo;
        }

        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task UpdateTodoAsync(Todo todo){
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var key = RedisName.GetObjectKey<Todo>(todo.Id.ToString());
            await _cachingService.SetAsync(key, todo);
        }
    }
}