using Microsoft.AspNetCore.Mvc;
using Moq;
using RevatureP1.Domain.Interfaces;
using RevatureP1.Models;
using RevatureP1.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RevatureP1.Tests
{
    public class LoginControllerTests
    {
        [Fact]
        public async Task CreateNewRedirectsToIndex()
        {
            // arrange
            var mockRepo = new Mock<IUnitOfWork>();

            mockRepo.Setup(x => x.CustomerRepository.Add(new Customer { CustomerId = 1 }))
                .ReturnsAsync(new Customer { CustomerId = 1 });

            var controller = new CustomersController(mockRepo.Object, null);

            // act
            IActionResult result = await controller.Edit(1, new Customer { CustomerId = 1 });

            // assert
            // ...that the result is a ViewResult
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(result);
            // ...that the model of the view is a CustomersViewModel
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
