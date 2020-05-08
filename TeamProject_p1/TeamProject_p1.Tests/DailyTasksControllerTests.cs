using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TeamProject_p1.Controllers;
using TeamProject_p1.Data;
using TeamProject_p1.Models;
using Xunit;

namespace TeamProject_p1.Tests
{
    public class DailyTasksControllerTests
    {
        [Fact]
        public async Task IndexShouldReturnViewWithData()
        {
            // arrange
            var mockRepo = new Mock<IRepository<DailyTask>>();
            // Moq gives us Mock class
            // which can implement interfaces at runtime
            // 1. you create the Mock
            // 2. you Setup the mock (giving it behavior)
            // 3. you give mock.Object to your test subject object
            // (4. you can use methods like Verify ot make sure that things were called on the mock object)
            mockRepo.Setup(x => x.GetAll())
                .ReturnsAsync(new List<DailyTask> { new DailyTask { DailyTaskId = 1 } });

            // mockRepo.Setup(x => x.Delete(It.IsAny<int>()))
            // mockRepo.Setup(x => x.Delete(It.IsInRange<int>(-99999, 0, Moq.Range.Inclusive)))
            // mockRepo.Setup(x => x.Delete(5))
            //     .ThrowsAsync(new InvalidOperationException());

            var controller = new DailyTasksController(mockRepo.Object);

            // act
            IActionResult result = await controller.Index();

            // assert
            // ...that the result is a ViewResult
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            // ...that the model of the view is a List<DailyTask>
            var list = Assert.IsAssignableFrom<List<DailyTask>>(viewResult.Model);
            // ...that the list has one element with ID 1 (based on the MockRepo's data)
            DailyTask dailyTask = Assert.Single(list);
            Assert.Equal(1, dailyTask.DailyTaskId);
            // we might also test that the correct view was chosen (DailyTasks/Index)

            mockRepo.Verify(x => x.GetAll(), Times.Once); // verify that the method was called once
        }
    }

    // rather than code like this, we can use Moq, much easier
    // (once you get past the learning curve)
    // class MockRepo : IRepository<DailyTask>
    // {
    //     public Task<DailyTask> Add(DailyTask entity)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public Task<DailyTask> Delete(int id)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public Task<DailyTask> Get(int id)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public Task<List<DailyTask>> GetAll()
    //     {
    //         return Task.FromResult(new List<DailyTask> {
    //             new DailyTask { DailyTaskId = 1 }
    //         });
    //     }

    //     public Task<DailyTask> Update(DailyTask ientity)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}
