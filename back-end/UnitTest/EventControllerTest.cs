using AutoMapper;
using Comp1640_Final.Controllers;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    public class EventControllerTests
    {
        private EventsController eventsController;
        private Mock<IEventService> eventServiceMock;
        private IMapper mapper;
        private Mock<UserManager<ApplicationUser>> userManagerMock;

        [SetUp]
        public void Setup()
        {
            eventServiceMock = new Mock<IEventService>();
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Event, EventDTO>().ReverseMap();
            }).CreateMapper();

            userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);
            eventsController = new EventsController(null, mapper, userManagerMock.Object, eventServiceMock.Object);
        }

        [Test]
        public async Task GetEvents_ReturnsOkResult()
        {
            // Arrange
            var events = new List<Event> {
                new Event { Id = 1, EventName = "Long Hoang" },
                new Event { Id = 2, EventName = "Good Boy"}
            };
            eventServiceMock.Setup(x => x.GetEvents()).Returns(events);

            // Act
            IActionResult result = await eventsController.GetEvents();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetStudentEvent_WithValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = "validUserId";
            var user = new ApplicationUser { Id = userId, UserName = "testuser" };
            var testEvent = new Event { Id = 1, EventName = "Event 1" };

            userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            eventServiceMock.Setup(x => x.GetFirstEventByFaculty(It.IsAny<int>())).ReturnsAsync(testEvent);

            // Act
            //var result = await _eventsController.GetStudentEvent(userId);
            var actionResult = await eventsController.GetStudentEvent(userId);

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.InstanceOf<OkObjectResult>());

            // Extract the value from the ActionResult
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.InstanceOf<EventDTO>());
        }

        [Test]
        public async Task PostEvent_WithValidEvent_ReturnsOkResult()
        {
            // Arrange
            var eventDto = new EventDTO { Id = 1, EventName = "Test Event" };
            var createdEvent = new Event { Id = 1, EventName = "Test Event" };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Event>(eventDto)).Returns(createdEvent);

            eventsController = new EventsController(null, mapperMock.Object, null, eventServiceMock.Object);

            eventServiceMock.Setup(x => x.CreateEvent(createdEvent)).ReturnsAsync(true);

            // Act
            var result = await eventsController.PostEvent(eventDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            //// Arrange
            //var eventDto = new EventDTO { Id = 1, EventName = "Test Event" };
            //var createdEvent = new Event { Id = 1, EventName = "Test Event" };

            //eventServiceMock.Setup(x => x.CreateEvent(It.IsAny<Event>())).ReturnsAsync(true);

            //var mapperMock = new Mock<IMapper>();
            //mapperMock.Setup(m => m.Map<Event>(eventDto)).Returns(createdEvent);

            ////mapper.Setup(m => m.Map<Event>(eventDto)).Returns(createdEvent);

            //// Act
            //var result = await eventsController.PostEvent(eventDto);

            //// Assert
            //Assert.That(result, Is.Not.Null);
            //Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }


        [Test]
        public async Task Put_WithValidEvent_ReturnsOkResult()
        {
            // Arrange
            int eventId = 1;
            var eventDto = new EventDTO { Id = eventId, EventName = "Updated Event" };
            var updatedEvent = new Event { Id = eventId, EventName = "Updated Event" };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Event>(eventDto)).Returns(updatedEvent);

            eventsController = new EventsController(null, mapperMock.Object, null, eventServiceMock.Object);

            eventServiceMock.Setup(x => x.EditEvent(updatedEvent)).ReturnsAsync(true);

            // Act
            var result = await eventsController.Put(eventId, eventDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
