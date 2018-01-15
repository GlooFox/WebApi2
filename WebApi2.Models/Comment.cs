using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi2.Models
{
	public class Comment
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public int PostId { get; set; }

		public DateTime CreateTime { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
