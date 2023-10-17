using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoRedis.Models;

namespace TodoRedis.Data
{
    public class TodoDbContext : DbContext
    {

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        
    }
}