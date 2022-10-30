﻿
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkProject.Models
{
    public class SliderDetail : BaseEntity
    {
        [Required(ErrorMessage = "Can't be empty")]
        public string Header { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public string Description { get; set; }
        public string SignImage { get; set; }
        [NotMapped]
        public IFormFile SignPhoto { get; set; }
    }
}
