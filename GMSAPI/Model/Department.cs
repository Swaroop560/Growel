using System.ComponentModel.DataAnnotations;

namespace GMSAPI.Model
{
    public class Department
    {
        [Key]
        public int DeptID { get; set; }
        public string DeptName { get; set; }

    }
}

