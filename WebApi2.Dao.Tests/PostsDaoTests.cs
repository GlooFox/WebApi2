using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi2.Models;

namespace WebApi2.Dao.Tests
{
	[TestClass]
	public class PostsDaoTests
	{
		[TestMethod]
		public void GetList_GettingData_ReturnList()
		{
			var data = CreatePostData();
			var storMock = new Mock<IStorage>();
			storMock.Setup(x => x.GetPosts()).Returns(data);

			var sut = new PostsDao(storMock.Object);
			var result = sut.GetList(null, null, 0, 0, 0).ToList();

			Assert.AreEqual(data.Count, result.Count);
			Assert.AreSame(data[0], result[0]);

			result = sut.GetList(null, "Id", 1, 0, 1).ToList();
			Assert.AreEqual(data.Count, result.Count);
			Assert.AreSame(data[data.Count - 1], result[0]);
		}

		[TestMethod]
		public void GetEntity_GettingData_ReturnEntity()
		{
			var data = CreatePostData();
			var storMock = new Mock<IStorage>();
			storMock.Setup(x => x.GetPosts()).Returns(data);

			int entityId = 6;
			var sut = new PostsDao(storMock.Object);
			var result = sut.GetEntity(entityId);

			Assert.AreSame(data.Where(x => entityId == x.Id).First(), result);
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void GetEntity_GettingData_ThrowException()
		{
			var data = CreatePostData();
			var storMock = new Mock<IStorage>();
			storMock.Setup(x => x.GetPosts()).Returns(data);

			int entityId = 248;
			var sut = new PostsDao(storMock.Object);
			sut.GetEntity(entityId);
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
