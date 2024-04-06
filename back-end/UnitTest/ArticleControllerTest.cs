using AutoMapper;
using CloudinaryDotNet;
using Comp1640_Final.Controllers;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ArticleControllerTest
    {
        private ArticlesController _articleController;
        private Mock<IArticleService> _articleServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<ICommentService> _commentServiceMock;
        private Mock<ILikeService> _likeServiceMock;
        private Mock<IDislikeService> _dislikeServiceMock;
        private Mock<Cloudinary> _cloudinaryMock;

        [SetUp]
        public void SetUp()
        {
            _articleServiceMock = new Mock<IArticleService>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>(); 
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();// giả định lưu trữ và truy vấn thông tin người dùng
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _commentServiceMock = new Mock<ICommentService>();
            _likeServiceMock = new Mock<ILikeService>();
            _dislikeServiceMock = new Mock<IDislikeService>();
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            _cloudinaryMock = new Mock<Cloudinary>(cloudinaryUrl);

            _articleController = new ArticlesController(
                _articleServiceMock.Object,
                _mapperMock.Object,
                null,
                null,
                _userManagerMock.Object,
                _userServiceMock.Object,
                _commentServiceMock.Object,
                _likeServiceMock.Object,
                _dislikeServiceMock.Object,
                _cloudinaryMock.Object,
                null
                );
        }

        [Test]
        public async Task GetArticleByUserId_Test()
        {
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article> { 
                new Article
                {
                    Id = new Guid(),
                    Title = "AAAAAA",
                    StudentId = userId,
                    CloudImagePath = cloudinaryUrl,
                },
                new Article
                {
                    Id = new Guid(),
                    Title = "BBBBBB",
                    StudentId = userId,
                    CloudImagePath = cloudinaryUrl,
                    

                }
            }; 
            var expectedResponse = new SubmissionResponse();


            _articleServiceMock.Setup(x => x.GetArticles()).Returns(articles); //test method from service
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());//student
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);//student ava
            _commentServiceMock.Setup(x => x.GetCommentsCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.GetArticleLikesCount(articles[1].Id)).ReturnsAsync(1);
            _dislikeServiceMock.Setup(x => x.GetArticleDislikesCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.IsArticleLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(x => x.IsArticleDisLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map<SubmissionResponse>(It.IsAny<Article>())).Returns(expectedResponse);

            var result = await _articleController.GetArticles(userId);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<SubmissionResponse>>());//mong đợi rằng phương thức GetArticles sẽ trả về một OkObjectResult chứa List dưới dạng List<SubmissionResponse>
        }

        [Test]
        public async Task GetArticleByArticleId_ArticleIdExist_Test()
        {
            var articleId = Guid.NewGuid();
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var article = new Article
            {
                Id = articleId,
                StudentId = userId,
                CloudImagePath = cloudinaryUrl,
            };
            var expectedResponse = new SubmissionResponse();

            _articleServiceMock.Setup(x => x.ArticleExists(articleId)).Returns(true);
            _articleServiceMock.Setup(x => x.GetArticleByID(articleId)).Returns(article); //test method from service
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());//student
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);//student ava
            _commentServiceMock.Setup(x => x.GetCommentsCount(article.Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.GetArticleLikesCount(article.Id)).ReturnsAsync(1);
            _dislikeServiceMock.Setup(x => x.GetArticleDislikesCount(article.Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.IsArticleLiked(userId, article.Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(x => x.IsArticleDisLiked(userId, article.Id)).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map<SubmissionResponse>(article)).Returns(expectedResponse);

            var result = await _articleController.GetArticleByID(articleId, userId);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<SubmissionResponse>());
        }

        [Test]
        public async Task GetArticleByArticleId_ArticleIdNotExist_Test()
        {
            var articleId = Guid.NewGuid();
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var article = new Article();
            var expectedResponse = new SubmissionResponse();

            _articleServiceMock.Setup(x => x.ArticleExists(articleId)).Returns(false);
            _articleServiceMock.Setup(x => x.GetArticleByID(articleId)).Returns(article); //test method from service
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());//student
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);//student ava
            _commentServiceMock.Setup(x => x.GetCommentsCount(article.Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.GetArticleLikesCount(article.Id)).ReturnsAsync(1);
            _dislikeServiceMock.Setup(x => x.GetArticleDislikesCount(article.Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.IsArticleLiked(userId, article.Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(x => x.IsArticleDisLiked(userId, article.Id)).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map<SubmissionResponse>(article)).Returns(expectedResponse);

            var result = await _articleController.GetArticleByID(articleId, userId);

            // assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());

        }

        [Test]
        public async Task GetApprovedArticle_ArticleExist_Test()
        {
            var facultyId = 1;
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article> {
                new Article
                {
                    Id = new Guid(),
                    Title = "AAAAAA",
                    StudentId = userId,
                    CloudImagePath = cloudinaryUrl,
                    PublishStatusId = (int)EPublishStatus.Approval
                },
                new Article
                {
                    Id = new Guid(),
                    Title = "BBBBBB",
                    StudentId = userId,
                    CloudImagePath = cloudinaryUrl,
                    PublishStatusId = (int)EPublishStatus.Approval

                }
            };
            var expectedResponse = new ArticleResponse();

            _articleServiceMock.Setup(x => x.GetApprovedArticles(facultyId)).ReturnsAsync(articles); //test method from service
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());//student
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);//student ava
            _commentServiceMock.Setup(x => x.GetCommentsCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.GetArticleLikesCount(articles[1].Id)).ReturnsAsync(1);
            _dislikeServiceMock.Setup(x => x.GetArticleDislikesCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.IsArticleLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(x => x.IsArticleDisLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map<ArticleResponse>(It.IsAny<Article>())).Returns(expectedResponse);

            var result = await _articleController.GetApprovedAticles(facultyId, userId);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<ArticleResponse>>());
        }

        [Test]
        public async Task GetApprovedArticle_ArticleNotExist_Test()
        {
            var facultyId = 1;
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article>();
            var expectedResponse = new ArticleResponse();

            _articleServiceMock.Setup(x => x.GetApprovedArticles(facultyId)).ReturnsAsync(articles); //test method from service
           

            var result = await _articleController.GetApprovedAticles(facultyId, userId);

            // assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());


        }

        [Test]
        public async Task GetArticleByPublishStatusAndFaculty_ArticleExist_Test()
        {
            var facultyId = 1;
            var statusId = 1;
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article> {
                new Article
                {
                    Id = new Guid(),
                    Title = "AAAAAA",
                    StudentId = userId,
                    FacultyId = facultyId,
                    CloudImagePath = cloudinaryUrl,
                    PublishStatusId = (int)EPublishStatus.Approval,

                    
                },
                new Article
                {
                    Id = new Guid(),
                    Title = "BBBBBB",
                    StudentId = userId,
                    FacultyId = facultyId,
                    CloudImagePath = cloudinaryUrl,
                    PublishStatusId = (int)EPublishStatus.Approval

                }
            };
            var expectedResponse = new SubmissionResponse();

            _articleServiceMock.Setup(x => x.GetArticleByPublishStatusIdAndFacultyId(statusId ,facultyId)).ReturnsAsync(articles); //test method from service
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());//student
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);//student ava
            _commentServiceMock.Setup(x => x.GetCommentsCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.GetArticleLikesCount(articles[1].Id)).ReturnsAsync(1);
            _dislikeServiceMock.Setup(x => x.GetArticleDislikesCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.IsArticleLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(x => x.IsArticleDisLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map<SubmissionResponse>(It.IsAny<Article>())).Returns(expectedResponse);

            var result = await _articleController.GetArticleByPublishStatusIdAndFacultyId(statusId, facultyId, userId);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<SubmissionResponse>>());
        }

        [Test]
        public async Task GetArticleByPublishStatusAndFaculty_ArticleNotExist_Test()
        {
            var facultyId = 1;
            var statusId = 1;
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article>();
            var expectedResponse = new SubmissionResponse();

            _articleServiceMock.Setup(x => x.GetArticleByPublishStatusIdAndFacultyId(statusId, facultyId)).ReturnsAsync(articles); //test method from service

            var result = await _articleController.GetArticleByPublishStatusIdAndFacultyId(statusId, facultyId, userId);

            // assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        }

        [Test]
        public async Task GetArticleByCoordinatorStatusAndFacultyId_ArticleExitst_Test()
        {
            var facultyId = 1;
            var coordinatoStatus = true;
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article> {
                new Article
                {
                    Id = new Guid(),
                    Title = "AAAAAA",
                    StudentId = userId,
                    FacultyId = facultyId,
                    CoordinatorStatus = true,
                    CloudImagePath = cloudinaryUrl,
                    PublishStatusId = (int)EPublishStatus.Approval,


                },
                new Article
                {
                    Id = new Guid(),
                    Title = "BBBBBB",
                    StudentId = userId,
                    FacultyId = facultyId,
                    CoordinatorStatus = true,
                    CloudImagePath = cloudinaryUrl,
                    PublishStatusId = (int)EPublishStatus.Approval

                }
            };
            var expectedResponse = new SubmissionResponse();

            _articleServiceMock.Setup(x => x.GetArticleByCoordinatorStatusAndFacultyId(coordinatoStatus, facultyId)).ReturnsAsync(articles); //test method from service
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());//student
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);//student ava
            _commentServiceMock.Setup(x => x.GetCommentsCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.GetArticleLikesCount(articles[1].Id)).ReturnsAsync(1);
            _dislikeServiceMock.Setup(x => x.GetArticleDislikesCount(articles[1].Id)).ReturnsAsync(1);
            _likeServiceMock.Setup(x => x.IsArticleLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(x => x.IsArticleDisLiked(userId, articles[1].Id)).ReturnsAsync(true);
            _mapperMock.Setup(x => x.Map<SubmissionResponse>(It.IsAny<Article>())).Returns(expectedResponse);

            var result = await _articleController.GetArticleByCoordinatorStatusAndFacultyId(coordinatoStatus, facultyId, userId);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<SubmissionResponse>>());
        }

        [Test]
        public async Task GetArticleByCoordinatorStatusAndFacultyId_ArticleNotExitst_Test()
        {
            var facultyId = 1;
            var coordinatoStatus = true;
            var userId = "abcdef";
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var articles = new List<Article>();
            var expectedResponse = new SubmissionResponse();

            _articleServiceMock.Setup(x => x.GetArticleByCoordinatorStatusAndFacultyId(coordinatoStatus, facultyId)).ReturnsAsync(articles); //test method from service

            var result = await _articleController.GetArticleByCoordinatorStatusAndFacultyId(coordinatoStatus, facultyId, userId);

            // assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        }


    }
}
