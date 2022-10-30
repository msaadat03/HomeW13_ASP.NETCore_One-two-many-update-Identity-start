using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.ProductViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }  
        public ICollection<ProductImage> ProductImages { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
