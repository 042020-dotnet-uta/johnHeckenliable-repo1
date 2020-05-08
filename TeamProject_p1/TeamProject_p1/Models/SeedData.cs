using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TeamProject_p1.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using System.Globalization;

namespace TeamProject_p1.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProjectDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProjectDbContext>>()))
            {

                // Look for any taskDates or tasks
                if (context.CalendarDates.Any() || context.DailyTasks.Any())
                {
                    return;
                }

                context.CalendarDates.AddRange(
                    new Calendar
                    {
                        TaskDate = DateTime.Parse("2020-5-7")
                    },

                     new Calendar
                     {
                         TaskDate = DateTime.Parse("2020-5-8")
                     },

                     new Calendar
                     {
                         TaskDate = DateTime.Parse("2020-5-9")
                     }
                    );
                /*
                context.Tasks.AddRange(
                    new DailyTask
                    {
                        DateId = 1,
                        Description = "Clean the dishes"
                    },

                    new DailyTask
                    {
                        DateId = 2,
                        Description = "Finish this project"
                    },

                    new DailyTask
                    {
                        DateId = 3,
                        Description = "Feed the dogs"
                    }
                );
                */

                context.SaveChanges();
            }
        }
    }
}
