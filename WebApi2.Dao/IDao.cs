using System;
using System.Collections.Generic;

namespace WebApi2.Dao
{
	public interface IDao<T>
	{
		IEnumerable<T> GetList(int? id, string sortField, int sortOrder, int pageNumber, int pageSize);

		T GetEntity(int id);

		int InsertEntity(T value);

		void UpdateEntity(T value);

		void DeleteEntity(int id);
	}
}
