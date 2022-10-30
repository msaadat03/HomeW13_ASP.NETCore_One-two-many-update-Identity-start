using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkProject.Models
{
    public class Blog : BaseEntity
    {
        [Required(ErrorMessage = "Can't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public string Desc { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public DateTime Date { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Can't be empty")]
        public IFormFile Photo { get; set; }
    }
}
