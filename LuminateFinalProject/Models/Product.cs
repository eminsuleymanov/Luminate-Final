using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuminateFinalProject.Models
{
    public class Product:BaseEntity
    {
        [StringLength(255)]
        public string Title { get; set; }
        [Column(TypeName = "money")]
        public double Price { get; set; }
        [Column(TypeName = "money")]
        public double DiscountedPrice { get; set; }
        public int Count { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(1500)]
        public string FullDescription { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public string MainImage { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}

