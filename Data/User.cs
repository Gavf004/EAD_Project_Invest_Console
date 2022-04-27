using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectInvestClient
{
    public enum UserType 
    { 
        [Display(Name ="Basic user")]
        basic,
        [Display(Name = "Admin user")]
        admin 
    }
    public class User
    {
        [Key]
        [DisplayName("User ID")]
        public int UserID { get; set; }
        [Required(ErrorMessage ="Name field is required")]
        [DisplayName("User Name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="User Type Required")]
        [DisplayName("User Type")]
        public UserType UserType { get; set; }
        [EmailAddress]
        [Required(ErrorMessage ="Email required")]
        public  string Email { get; set; }


        // navigation properties

        public virtual ICollection<Order> Order { get; set; }
    }
}
