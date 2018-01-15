using System;
using System.Collections.Generic;
using System.Linq;
using WebApi2.Models;

namespace WebApi2.Dao
{
	public class CommentsDao : IDao<Comment>
	{
		private IStorage _storage;

		public CommentsDao(IStorage storage)
		{
			_storage = storage;
		}

		public IEnumerable<Comment> GetList(int? id, string sortField, int sortOrder, int pageNumber, int pageSize)
		{
			var result = _storage.GetComments(id);
			if (!string.IsNullOrEmpty(sortField))
			{
				var field = typeof(Comment).GetProperty(sortField);
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

		public Comment GetEntity(int id)
		{
			var result = _storage.GetComments(null).Where(x => id == x.Id).FirstOrDefault();
			if (null == result)
				throw new KeyNotFoundException();
			return result;
		}

		public int InsertEntity(Comment value)
		{
			return _storage.AddComment(value);
		}

		public void UpdateEntity(Comment value)
		{
			var item = _storage.GetComments(value.PostId).Where(x => value.Id == x.Id).FirstOrDefault();
			if (item != null)
			{
				item.UserId = value.UserId;
				item.CreateTime = value.CreateTime;
				item.Content = value.Content;
			}
			else
				throw new KeyNotFoundException();
		}

		public void DeleteEntity(int id)
		{
			_storage.DeleteComment(id);
		}
	}
}
