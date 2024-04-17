using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxeLoft.Models
{
    public class Feature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeatureID { get; set; }

        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        [Required]
        public string Feature_Name { get; set; }

        // Navigation property.
        [ValidateNever]
        public virtual Property Property { get; set; }
    }
}
