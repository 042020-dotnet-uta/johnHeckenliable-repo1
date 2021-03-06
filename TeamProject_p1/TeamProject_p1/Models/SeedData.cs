﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TeamProject_p1.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

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
                        //CalendarId = 1,
                        TaskDate = DateTime.Parse("2020-5-1")
                    },

                     new Calendar
                     {
                         //CalendarId = 2,
                         TaskDate = DateTime.Parse("2020-5-2")
                     },

                     new Calendar
                     {
                         //CalendarId = 3,
                         TaskDate = DateTime.Parse("2020-5-3")
                     }
                    );
                
                context.SaveChanges();

                context.DailyTasks.AddRange(
                    new DailyTask
                    {
                        CalendarItem = new Calendar
                        {
                            //CalendarId = 1,
                            TaskDate = DateTime.Parse("2020-5-5")
                        },
                        Description = "Clean the dishes"
                    },

                    new DailyTask
                    {
                        CalendarItem = new Calendar
                        {
                            //CalendarId = 2,
                            TaskDate = DateTime.Parse("2020-5-6")
                        },
                        Description = "Finish this project"
                    },

                    new DailyTask
                    {
                        CalendarItem = new Calendar
                        {
                            //CalendarId = 3,
                            TaskDate = DateTime.Parse("2020-5-7")
                        },
                        Description = "Feed the dogs"
                    },

                    new DailyTask
                    {
                        CalendarItem = context.CalendarDates
                        .Where(d => d.CalendarId == 1)
                        .FirstOrDefault(),
                        Description = "Task set to calendar 1"
                    },

                    new DailyTask
                    {
                        CalendarItem = context.CalendarDates
                        .Where(d => d.CalendarId == 1)
                        .FirstOrDefault(),
                        Description = "Another task set to calendar 1"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
