using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LuxeLoft.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string About { get; set; }

        // Navigation properties
        [ValidateNever]

        public virtual ICollection<Property> Properties { get; set; }

        [InverseProperty("Seller")]
        [ValidateNever]


        public virtual ICollection<Interested_Property> Interested_PropertiesAsSeller { get; set; } 

        [InverseProperty("Buyer")]
        [ValidateNever]
        public virtual ICollection<Interested_Property> Interested_PropertiesAsBuyer { get; set; }
    }
}
