using Client.Business.Core.Application.Features.Users.Queries;
using Client.Business.Core.Domain.Models.Users;
using Client.Business.Extensions.ViewModels.Features.User;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System.Diagnostics;
using Assert = Xunit.Assert;

namespace Client.Tests
{
    public class ViewModelTest
    {
        [Fact]
        public async Task LoadUsersCommand_Should_Load_Users()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var userQueryResult = new List<UserModel>
            {
                new UserModel {Id = 1, Name="장동계1", Password="pw", Email = "email1"},
                new UserModel {Id = 2, Name="장동계2", Password="pw", Email = "email2"},
            };

            mediatorMock.Setup(mediator => 
                mediator.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userQueryResult);

            var viewModel = new UserViewModel(mediatorMock.Object);

            // Act
            await viewModel.LoadUsersAsync();

            // Assert
            Assert.Equal(userQueryResult, viewModel.Users);
            mediatorMock.VerifyAll();
        }
    }
}