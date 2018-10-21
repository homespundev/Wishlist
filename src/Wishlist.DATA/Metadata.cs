using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Wishlist.DATA
{
    class FamilyMetadata
    {
        [Required]
        [StringLength(50, ErrorMessage = "* Max Length 50 characters")]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [StringLength(500, ErrorMessage = "* Max Length 500 characters")]
        [Display(Name = "Description")]
        public string FamilyDescription { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "* Max Length 100 characters")]
        [Display(Name = "Family ID")]
        public string FamilyId { get; set; }
    }
    [MetadataType(typeof(FamilyMetadata))]
    public partial class Family { }

    class ItemMetadata
    {
        [Required]
        [StringLength(50, ErrorMessage = "* Max Length 50 characters")]
        [Display(Name ="Name")]
        public string ItemName { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "* Max Length 500 characters")]
        [Display(Name = "Description")]
        public string ItemDescription { get; set; }
        [Display(Name = "Link")]
        public string ItemLink { get; set; }
        [Display(Name = "Image")]
        public string ItemImage { get; set; }
    }
    [MetadataType(typeof(ItemMetadata))]
    public partial class Item { }
    class UserMetadata
    {
        [Required]
        [StringLength(50, ErrorMessage = "* Max Length 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "* Max Length 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        public Nullable<System.DateTime> DoB { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$",
            ErrorMessage = "* Please enter a valid email address")]
        [StringLength(256, ErrorMessage = "* Email must not exceed 256 characters")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Username")]
        [StringLength(256, ErrorMessage = "* Max Length 256 characters")]
        public string UserName { get; set; }
    }
    [MetadataType(typeof(UserMetadata))]
    public partial class AspNetUser { }
}
