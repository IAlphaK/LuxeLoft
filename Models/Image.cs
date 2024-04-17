using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxeLoft.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }

        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        // Assuming the 'Image' column is a URL to the image
        [Required]
        public string Img { get; set; }

        // Navigation property
        [ValidateNever]

        public virtual Property Property { get; set; }
    }
}
