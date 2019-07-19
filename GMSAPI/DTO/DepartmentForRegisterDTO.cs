using System.ComponentModel.DataAnnotations;

namespace GMSAPI.DTO
{
    public class DepartmentForRegisterDTO
    {
        public int deptID { get; set; }
        [Required]
        [StringLength(10,MinimumLength=4,ErrorMessage="Invalid Department Name")]
        public string deptName { get; set; }
        
    }
}