using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

public class TodoItem
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }

    [ForeignKey(nameof(TodoType))]
    public long TypeId { get; set; }
    public virtual TodoType Type { get; set; }
}
