using System.ComponentModel.DataAnnotations;
using GMSAPI.Model;

namespace GMSAPI.DTO
{
    public class EmployeeForRegisterDTO
    {
         public int Eid { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Gender { get; set; }
        public string DoJ { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public int deptID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(12,MinimumLength=6,ErrorMessage="You must have password length between 6 to 8")]
        public string Password { get; set; }

    }
}