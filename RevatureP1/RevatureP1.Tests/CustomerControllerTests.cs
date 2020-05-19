using Microsoft.AspNetCore.Mvc;
using Moq;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Models;
using RevatureP1.Web.Controllers;
using RevatureP1.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RevatureP1.Tests
{
    //public class CustomerControllerTests
    //{
    //    [Fact]
    //    public async Task IndexShouldReturnViewWithData()
    //    {
    //        // arrange
    //        var mockRepo = new Mock<IRepository<Customer>>();
    //        // Moq gives us Mock class
    //        // which can implement interfaces at runtime
    //        // 1. you create the Mock
    //        // 2. you Setup the mock (giving it behavior)
    //        // 3. you give mock.Object to your test subject object
    //        // (4. you can use methods like Verify ot make sure that things were called on the mock object)
    //        mockRepo.Setup(x => x.All())
    //            .ReturnsAsync(new List<Customer> { new Customer { CustomerId = 1 } });

    //        // mockRepo.Setup(x => x.Delete(It.IsAny<int>()))
    //        // mockRepo.Setup(x => x.Delete(It.IsInRange<int>(-99999, 0, Moq.Range.Inclusive)))
    //        // mockRepo.Setup(x => x.Delete(5))
    //        //     .ThrowsAsync(new InvalidOperationException());

    //        var controller = new CustomersController(mockRepo.Object, null);

    //        // act
    //        IActionResult result = await controller.Index(null, null);

    //        // assert
    //        // ...that the result is a ViewResult
    //        var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
    //        // ...that the model of the view is a CustomersViewModel
    //        var viewModel = Assert.IsAssignableFrom<CustomersViewModel>(viewResult.Model);
    //        // ...that the list has one element with ID 1 (based on the MockRepo's data)
    //        Customer customer = Assert.Single(viewModel.Customers);
    //        Assert.Equal(1, customer.CustomerId);
    //        // we might also test that the correct view was chosen (DailyTasks/Index)

    //        mockRepo.Verify(x => x.All(), Times.Once); // verify that the method was called once
    //    }

    //    [Fact]
    //    public async Task CreateNewRedirectsToIndex()
    //    {
    //        // arrange
    //        var mockRepo = new Mock<IRepository<Customer>>();

    //        //mockRepo.Setup(x => x.Add(new Customer { CustomerId = 1 }))
    //        //    .ReturnsAsync(new Customer { CustomerId = 1 });

    //        var controller = new CustomersController(mockRepo.Object, null);

    //        // act
    //        IActionResult result = await controller.CreateNew(new Customer { CustomerId = 1 });

    //        // assert
    //        // ...that the result is a ViewResult
    //        var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(result);
    //        // ...that the model of the view is a CustomersViewModel
    //        Assert.Equal("Index", viewResult.ActionName);
    //    }

    //    [Fact]
    //    public async Task EditRedirectsToIndex()
    //    {
    //        // arrange
    //        var mockRepo = new Mock<IRepository<Customer>>();

    //        //mockRepo.Setup(x => x.Add(new Customer { CustomerId = 1 }))
    //        //    .ReturnsAsync(new Customer { CustomerId = 1 });

    //        var controller = new CustomersController(mockRepo.Object, null);

    //        // act
    //        IActionResult result = await controller.Edit(1, new Customer { CustomerId = 1 });

    //        // assert
    //        // ...that the result is a ViewResult
    //        var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(result);
    //        // ...that the model of the view is a CustomersViewModel
    //        Assert.Equal("Index", viewResult.ActionName);
    //    }

    //    [Fact]
    //    public async Task DeleteConfirmedRedirectsToIndex()
    //    {
    //        // arrange
    //        var mockRepo = new Mock<IRepository<Customer>>();

    //        //mockRepo.Setup(x => x.Add(new Customer { CustomerId = 1 }))
    //        //    .ReturnsAsync(new Customer { CustomerId = 1 });

    //        var controller = new CustomersController(mockRepo.Object, null);

    //        // act
    //        IActionResult result = await controller.DeleteConfirmed(1);

    //        // assert
    //        // ...that the result is a ViewResult
    //        var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(result);
    //        // ...that the model of the view is a CustomersViewModel
    //        Assert.Equal("Index", viewResult.ActionName);
    //    }
    //}
}
