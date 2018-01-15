using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi2.Dao;
using WebApi2.Filters;
using WebApi2.Models;

namespace WebApi2.Controllers
{
	[Route("api/[controller]")]
	public class PostsController : Controller
	{
		private ILogger _logger;

		private IDao<Post> _postsDao;
		private IDao<Comment> _commentsDao;

		public PostsController(ILogger<PostsController> logger, IDao<Post> postsDao, IDao<Comment> commentsDao)
		{
			_logger = logger;
			_postsDao = postsDao;
			_commentsDao = commentsDao;
		}

		#region Posts

		[HttpGet]
		public async Task<IEnumerable<Post>> GetPosts(string sortField, int sortOrder, int pageNumber, int pageSize)
		{
			IEnumerable<Post> result = _postsDao.GetList(null, sortField, sortOrder, pageNumber, pageSize);
			return await Task.FromResult(result);
		}

		[HttpGet("{id}")]
		public Post GetPost(int id)
		{
			return _postsDao.GetEntity(id);
		}

		[HttpPost]
		[ValidateModel]
		public void PostPost([FromBody]Post value)
		{
			int result = _postsDao.InsertEntity(value);
			PostSuccess(result.ToString());
		}

		[HttpPut]
		[ValidateModel]
		public void PutPost([FromBody]Post value)
		{
			_postsDao.UpdateEntity(value);
		}

		[HttpDelete("{id}")]
		public void DeletePost(int id)
		{
			_postsDao.DeleteEntity(id);
		}

		#endregion

		#region Comments

		[HttpGet("{postId}/comments")]
		public async Task<IEnumerable<Comment>> GetComments(int postId, string sortField, int sortOrder, int pageNumber, int pageSize)
		{
			IEnumerable<Comment> result = _commentsDao.GetList(postId, sortField, sortOrder, pageNumber, pageSize);
			return await Task.FromResult(result);
		}

		[HttpGet("{postId}/comments/{commentId}")]
		public Comment GetComment(int postId, int commentId)
		{
			return _commentsDao.GetEntity(commentId);
		}

		[HttpPost("{postId}/comments")]
		[ValidateModel]
		public void PostComment(int postId, [FromBody]Comment value)
		{
			int result = _commentsDao.InsertEntity(value);
			PostSuccess($"{postId}/comments/{result}");
		}

		[HttpPut("{postId}/comments")]
		[ValidateModel]
		public void Put(int postId, [FromBody]Comment value)
		{
			_commentsDao.UpdateEntity(value);
		}

		[HttpDelete("{postId}/comments/{commentId}")]
		public void DeleteComment(int postId, int commentId)
		{
			_commentsDao.DeleteEntity(commentId);
		}

		#endregion

		private void PostSuccess(string location)
		{
			location = "/api/controller/" + location;
			if (HttpContext != null)
			{
				HttpContext.Response.Headers["Location"] = location;
				HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
			}
			_logger?.LogDebug("Resource has been created: " + location);
		}
	}
}
