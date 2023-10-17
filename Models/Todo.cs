using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoRedis.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public bool Concluida { get; set; }
    }
}