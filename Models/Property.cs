using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LuxeLoft.Models
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyID { get; set; }

        [ForeignKey("User")]
        public string OwnerID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Type { get; set; }
        public string Thumbnail { get; set; }
        [NotMapped]
        [DisplayName("Thumbnail - Main Image")]
        public IFormFile ThumbnailFile { get; set; }

        // Navigation properties

        [ValidateNever]

        public virtual User Owner { get; set; }
        [ValidateNever]

        public virtual ICollection<Interested_Property> InterestedParties { get; set; } 

        public virtual ICollection<Feature> Features { get; set; }
        [ValidateNever]

        public virtual ICollection<Image> Images { get; set; }
    }
}
