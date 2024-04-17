using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxeLoft.Models
{
    public class Interested_Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InterestedID { get; set; }

        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        [ForeignKey("User")]
        public string SellerID { get; set; }

        [ForeignKey("User")]
        public string BuyerID { get; set; }

        // Navigation properties
        [ValidateNever]

        public virtual Property Property { get; set; }
        [ValidateNever]

        public virtual User Seller { get; set; }
        [ValidateNever]

        public virtual User Buyer { get; set; }
    }
}
