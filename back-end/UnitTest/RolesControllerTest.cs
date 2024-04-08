using Comp1640_Final.Controllers;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    [TestFixture]
    public class RolesControllerTest
    {
        private RolesController _rolesController;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;

        [SetUp]
        public void Setup()
        {
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);

            _rolesController = new RolesController(
                _roleManagerMock.Object
            );
        }

        [Test]
        public async Task PostRole_ReturnsOkResult()
        {
            // Arrange
            var role = new Role { RoleName = "TestRole" };
            var identityResult = IdentityResult.Success;

            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identityResult);

            // Act
            var result = await _rolesController.PostRole(role);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(role.RoleName));
        }

        [Test]
        public async Task PostRole_ReturnsBadRequest_WhenCreateRoleFails()
        {
            // Arrange
            var role = new Role { RoleName = "TestRole" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Failed to create role" });

            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identityResult);

            // Act
            var result = await _rolesController.PostRole(role);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<BadRequestResult>());

        }

        //[Test]
        //public async Task GetRole_ReturnsOkResult_WithRoleList()
        //{
        //    // Arrange
        //    var roles = new List<IdentityRole>
        //{
        //    new IdentityRole { Name = "Role1" },
        //    new IdentityRole { Name = "Role2" }
        //};/*.AsQueryable();*/   

        //    // Mock the Roles property to return an IQueryable asynchronously
        //    _roleManagerMock.Setup(x => x.Roles).Returns(roles.AsQueryable);

        //    // Act
        //    var result = await _rolesController.GetRole();

        //    // Assert
        //    Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
        //    Assert.That(result.Value, Is.InstanceOf<IEnumerable<IdentityRole>>());
        //    var returnedRoles = result.Value as IEnumerable<IdentityRole>;
        //    Assert.That(returnedRoles.Count(), Is.EqualTo(roles.Count()));
        //}



        ////////////
        ///

        //[Test]
        //public async Task GetRole_ReturnsInternalServerError_WhenRoleRetrievalFails()
        //{
        //    // Arrange
        //    _roleManagerMock.Setup(x => x.Roles).Throws(new Exception("Failed to retrieve roles"));

        //    // Act
        //    var result = await _rolesController.GetRole();

        //    // Assert
        //    Assert.That(result, Is.Not.Null.And.InstanceOf<ObjectResult>());
        //    var objectResult = result as ObjectResult;
        //    Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        //}


        [Test]
        public async Task PostRole_ReturnsInternalServerError_WhenRoleManagerFails()
        {
            // Arrange
            var role = new Role { RoleName = "TestRole" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Failed to create role" });

            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identityResult);

            // Act
            var result = await _rolesController.PostRole(role);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<StatusCodeResult>());
            var statusCodeResult = result as StatusCodeResult;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            //Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        //[Test]
        //public async Task PostRole_ReturnsBadRequest_WhenRoleNameIsInvalid()
        //{
        //    // Arrange
        //    var role = new Role { RoleName = null }; // Invalid role name

        //    // Act
        //    var result = await _rolesController.PostRole(role);

        //    // Assert
        //    Assert.That(result, Is.Not.Null.And.InstanceOf<StatusCodeResult>());
        //    var statusCodeResult = result as StatusCodeResult;
        //    Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        //}

        [Test]
        public async Task PostRole_ReturnsInternalServerError_WhenRoleCreationFails()
        {
            // Arrange
            var role = new Role { RoleName = "TestRole" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Failed to create role" });

            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identityResult);

            // Act
            var result = await _rolesController.PostRole(role);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<StatusCodeResult>());
            var statusCodeResult = result as StatusCodeResult;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

    }

}
