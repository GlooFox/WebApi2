using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi2.Models
{
	public class Post
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public DateTime EditTime { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
