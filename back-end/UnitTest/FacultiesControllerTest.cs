using Comp1640_Final.Controllers;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class FacultiesControllerTest
    {
        private readonly Mock<IFacultyService> _facultyServiceMock;
        private readonly FacultiesController _facultiesController;

        public FacultiesControllerTest()
        {
            _facultyServiceMock = new Mock<IFacultyService>();
            _facultiesController = new FacultiesController(_facultyServiceMock.Object);
        }

        [Test]
        public void GetFaculties_ReturnsOkResult()
        {
            // Arrange
            var faculties = new List<Faculty>
            {
                new Faculty { Id = 1, Name = "Faculty 1" },
                new Faculty { Id = 2, Name = "Faculty 2" }
            };
            _facultyServiceMock.Setup(x => x.GetFaculties()).Returns(faculties);

            // Act
            var result = _facultiesController.GetFalcuties();

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            //Assert.That(okResult.Value, Is.Not.Null.And.AssignableFrom<IEnumerable<Faculty>>());
            Assert.That(okResult.Value, Is.Not.Null.And.TypeOf<List<Faculty>>());
            var returnedFaculties = okResult.Value as IEnumerable<Faculty>;
            //Assert.Equals(faculties.Count, returnedFaculties.Count);
            //Assert.That(faculties.Count, Is.Not.Null.And.EqualTo(returnedFaculties.Count));
            Assert.That(returnedFaculties, Is.Not.Null.And.Count.EqualTo(faculties.Count));
        }

        [Test]
        public async Task GetContributionsByYear_ReturnsOkResult()
        {
            // Arrange
            ICollection<DashboardResponse> contributions = new List<DashboardResponse>
            {
                new DashboardResponse { Year = "2022", Values = new List<Values> { new Values { Faculty = "Faculty 1", value = 10 } } },
                new DashboardResponse { Year = "2023", Values = new List<Values> { new Values { Faculty = "Faculty 2", value = 15 } } }
            };


            _facultyServiceMock.Setup(x => x.GetContributionsByYear()).Returns(contributions); //tra ve returnsasync neu methods la async Task, else just Returns

            // Act
            var result = await _facultiesController.GetContributionsByYear();

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<IEnumerable<DashboardResponse>>());
            var returnedContributions = okResult.Value as IEnumerable<DashboardResponse>;
            Assert.That(returnedContributions, Is.Not.Null.And.Count.EqualTo(contributions.Count));
        }

        [Test]
        public async Task GetContributorsByYear_ReturnsOkResult()
        {
            // Arrange
            ICollection<DashboardResponse> contributors = new List<DashboardResponse>
    {
        new DashboardResponse { Year = "2022", Values = new List<Values> { new Values { Faculty = "Faculty 1", value = 5 } } },
        new DashboardResponse { Year = "2023", Values = new List<Values> { new Values { Faculty = "Faculty 2", value = 8 } } }
    };
            _facultyServiceMock.Setup(x => x.GetContributorsByYear()).Returns(contributors);

            // Act
            var result = await _facultiesController.GetContributorsByYear();

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<IEnumerable<DashboardResponse>>());
            var returnedContributors = okResult.Value as IEnumerable<DashboardResponse>;
            Assert.That(returnedContributors, Is.Not.Null.And.Count.EqualTo(contributors.Count));
        }

        [Test]
        public async Task GetPercentageContributions_ReturnsOkResult()
        {
            // Arrange
            ICollection<Values> percentages = new List<Values>
    {
        new Values { Faculty = "Faculty 1", value = 50 },
        new Values { Faculty = "Faculty 2", value = 30 }
    };
            _facultyServiceMock.Setup(x => x.GetPercentageContributions()).Returns(percentages);

            // Act
            var result = await _facultiesController.GetPercentageContributions();

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<IEnumerable<Values>>());
            var returnedPercentages = okResult.Value as IEnumerable<Values>;
            Assert.That(returnedPercentages, Is.Not.Null.And.Count.EqualTo(percentages.Count));
        }
    }
}
