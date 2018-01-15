using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi2.Models;

namespace WebApi2.Dao.Tests
{
	[TestClass]
	public class CommentsDaoTests
	{
		[TestMethod]
		public void GetList_GettingData_ReturnList()
		{
			var data = CreateCommentData();
			var storMock = new Mock<IStorage>();
			storMock.Setup(x => x.GetComments(It.IsNotNull<int?>())).Returns(data);

			var sut = new CommentsDao(storMock.Object);
			var result = sut.GetList(1, null, 0, 0, 0).ToList();

			Assert.AreEqual(data.Count, result.Count);
			Assert.AreSame(data[0], result[0]);

			result = sut.GetList(1, "Id", 1, 0, 1).ToList();
			Assert.AreEqual(data.Count, result.Count);
			Assert.AreSame(data[data.Count - 1], result[0]);
		}

		private static IList<Comment> CreateCommentData()
		{
			return new[]
			{
				new Comment { Id = 2, UserId = 1, PostId = 1, CreateTime = DateTime.UtcNow.AddDays(-1), Content = "Post 1 comment 2." },
				new Comment { Id = 4, UserId = 1, PostId = 3, CreateTime = DateTime.UtcNow, Content = "Post 3 comment 4. " },
				new Comment { Id = 6, UserId = 1, PostId = 1, CreateTime = DateTime.UtcNow.AddDays(-1), Content = "Post 1 comment 6." }
			};
		}
	}
}
