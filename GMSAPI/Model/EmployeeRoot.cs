using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GMSAPI.Model
{
    public class EmployeeRoot
    {
        [Key]
        public int Eid { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Gender { get; set; }
        public string DoJ { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public int deptID { get; set; }
        public string UserName { get; set; }
        public byte[]  PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [ForeignKey("deptID")]
        public Department Dept { get; set; }
        

    }
}