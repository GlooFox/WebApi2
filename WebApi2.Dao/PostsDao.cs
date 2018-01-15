using System;
using System.Collections.Generic;
using System.Linq;
using WebApi2.Models;

namespace WebApi2.Dao
{
	public class PostsDao : IDao<Post>
	{
		private IStorage _storage;

		public PostsDao(IStorage storage)
		{
			_storage = storage;
		}

		public IEnumerable<Post> GetList(int? id, string sortField, int sortOrder, int pageNumber, int pageSize)
		{
			var result = _storage.GetPosts();
			if (!string.IsNullOrEmpty(sortField))
			{
				var field = typeof(Post).GetProperty(sortField);
				if (sortOrder != 0)
					result = result.OrderByDescending(x => field.GetValue(x)).ToList();
				else
					result = result.OrderBy(x => field.GetValue(x));
			}
			if (pageNumber > 0)
			{
				result = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			}
			return result;
		}

		public Post GetEntity(int id)
		{
			var result = _storage.GetPosts().Where(x => id == x.Id).FirstOrDefault();
			if (null == result)
				throw new KeyNotFoundException();
			return result;
		}

		public int InsertEntity(Post value)
		{
			return _storage.AddPost(value);
		}

		public void UpdateEntity(Post value)
		{
			var item = _storage.GetPosts().Where(x => value.Id == x.Id).FirstOrDefault();
			if (item != null)
			{
				item.UserId = value.UserId;
				item.EditTime = value.EditTime;
				item.Content = value.Content;
			}
			else
				throw new KeyNotFoundException();
		}

		public void DeleteEntity(int id)
		{
			_storage.DeletePost(id);
		}
	}
}
