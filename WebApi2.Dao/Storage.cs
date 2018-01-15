using System;
using System.Collections.Generic;
using System.Linq;
using WebApi2.Models;

namespace WebApi2.Dao
{
	public interface IStorage
	{
		IEnumerable<Post> GetPosts();
		int AddPost(Post value);
		void DeletePost(int id);

		IEnumerable<Comment> GetComments(int? postId);
		int AddComment(Comment value);
		void DeleteComment(int id);
	}

	public class Storage: IStorage
	{
		public readonly static IStorage Instance = new Storage();

		#region Posts data

		private readonly IList<Post> _postList = new List<Post>(new[]
		{
			new Post { Id = 1, UserId = 1, EditTime = DateTime.UtcNow.AddDays(-4), Content = "Post 1 content." },
			new Post { Id = 2, UserId = 1, EditTime = DateTime.UtcNow.AddDays(-3), Content = "Post 2 content." },
			new Post { Id = 3, UserId = 2, EditTime = DateTime.UtcNow.AddDays(-2), Content = "Post 3 content." },
			new Post { Id = 4, UserId = 2, EditTime = DateTime.UtcNow.AddDays(-1), Content = "Post 4 content." }
		});

		private readonly object _postSync = new object();

		#endregion

		#region Comments data

		private readonly IList<Comment> _cmntList = new List<Comment>(new[]
		{
			new Comment { Id = 1, UserId = 3, PostId = 1, CreateTime = DateTime.UtcNow.AddDays(-3), Content = "Post 1 comment 1." },
			new Comment { Id = 2, UserId = 3, PostId = 2, CreateTime = DateTime.UtcNow.AddDays(-2), Content = "Post 2 comment 2." },
			new Comment { Id = 3, UserId = 4, PostId = 2, CreateTime = DateTime.UtcNow.AddDays(-1), Content = "Post 2 comment 3." },
			new Comment { Id = 4, UserId = 4, PostId = 3, CreateTime = DateTime.UtcNow, Content = "Post 3 comment 4. " }
		});

		private readonly object _cmntSync = new object();

		#endregion

		#region Posts methods

		public IEnumerable<Post> GetPosts()
		{
			lock (_postSync)
			{
				return _postList.ToList();
			}
		}

		public int AddPost(Post value)
		{
			lock (_postSync)
			{
				int id = _postList.Max(x => x.Id) + 1;
				value.Id = id;
				_postList.Add(value);
				return id;
			}
		}

		public void DeletePost(int id)
		{
			lock (_postSync)
			{
				var item = _postList.FirstOrDefault(x => id == x.Id);
				if (item != null)
					_postList.Remove(item);
				else
					throw new KeyNotFoundException();
			}
		}

		#endregion

		#region Comments methods

		public IEnumerable<Comment> GetComments(int? postId)
		{
			lock (_cmntSync)
			{
				return _cmntList.Where(x => null == postId || postId.Value == x.PostId).ToList();
			}
		}

		public int AddComment(Comment value)
		{
			lock (_cmntSync)
			{
				int id = _cmntList.Max(x => x.Id) + 1;
				value.Id = id;
				_cmntList.Add(value);
				return id;
			}
		}

		public void DeleteComment(int id)
		{
			lock (_cmntSync)
			{
				var item = _cmntList.FirstOrDefault(x => id == x.Id);
				if (item != null)
					_cmntList.Remove(item);
				else
					throw new KeyNotFoundException();
			}
		}

		#endregion
	}
}
