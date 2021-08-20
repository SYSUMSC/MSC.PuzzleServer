﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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
        public string? Level { get; set; }

        [Required]
        [MaxLength(250)]
        public string? Logger { get; set; }

        [MaxLength(25)]
        public string? RemoteIP { get; set; }

        [MaxLength(25)]
        public string? UserName { get; set; }

        public string Message { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Status { get; set; }

        public string? Exception { get; set; }
    }
}