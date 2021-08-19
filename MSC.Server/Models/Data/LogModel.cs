using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSC.Server.Models
{

	public class LogModel
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public DateTime TimeUTC { get; set; }
		[Required]
		[MaxLength(50)]
		public string Level { get; set; }
		[Required]
		[MaxLength(250)]
		public string Logger { get; set; }
		[MaxLength(25)]
		public string RemoteIP { get; set; }
		[MaxLength(25)]
		public string UserName { get; set; }
		public string Message { get; set; }
		[MaxLength(20)]
		public string Status { get; set; }
		public string Exception { get; set; }
	}
}
