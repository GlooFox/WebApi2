using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi2.Controllers;
using WebApi2.Dao;
using WebApi2.Models;

namespace WebApi2.Tests
{
	[TestClass]
	public class PostsControllerTests
	{
		[TestMethod]
		public async Task GetPosts_RequestingData_ReturnList()
		{
			var data = CreatePostData();
			var daoMock = new Mock<IDao<Post>>();
			daoMock.Setup(x =>
				x.GetList(It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
				.Returns(data);

			var sut = new PostsController(null, daoMock.Object, null);
			var response = await sut.GetPosts(null, 0, 0, 0);
			var result = response.ToList();

			Assert.AreEqual(data.Count, result.Count);
			Assert.AreSame(data[0], result[0]);
			Assert.AreSame(data[1], result[1]);
		}

		[TestMethod]
		public void GetPost_RequestingData_ReturnEntity()
		{
			int entityId = 6;
			var data = CreatePostData().Where(x => entityId == x.Id).First();
			var daoMock = new Mock<IDao<Post>>();
			daoMock.Setup(x => x.GetEntity(It.Is<int>(id => entityId == id))).Returns(data);

			var sut = new PostsController(null, daoMock.Object, null);
			var result = sut.GetPost(entityId);

			Assert.AreSame(data, result);
		}

		[TestMethod]
		public void PostPost_InsertingData_Success()
		{
			int entityId = 6;
			var data = CreatePostData().Where(x => entityId == x.Id).First();
			var daoMock = new Mock<IDao<Post>>();
			daoMock.Setup(x => x.InsertEntity(It.Is<Post>(value => value == data))).Returns(entityId);

			var sut = new PostsController(null, daoMock.Object, null);
			sut.PostPost(data);

			daoMock.Verify(x => x.InsertEntity(It.Is<Post>(value => value == data)), Times.Once);
		}

		private static IList<Post> CreatePostData()
		{
			return new[]
			{
				new Post { Id = 2, UserId = 4, EditTime = DateTime.UtcNow.AddDays(-2), Content = "Post 2 content." },
				new Post { Id = 6, UserId = 4, EditTime = DateTime.UtcNow.AddDays(-1), Content = "Post 6 content." }
			};
		}
	}
}
