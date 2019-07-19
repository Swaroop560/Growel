using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GMSAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace GMSAPI.DataContext
{
    /*
    This Auth Repo is Responsible for interacting with the EF to fetch the Data.
     */

    public class AuthRepository : IAuthRepository
    {
        private readonly EmployeeContext _context;
        public AuthRepository(EmployeeContext context)
        {
            _context = context;

        }
       
        public async Task<EmployeeRoot> Login(string username, string password)
        {
            var employee = await _context.EmployeeRoots.FirstOrDefaultAsync(x => x.UserName == username);

            if(employee == null)
                return null;
            if(!VerifyPasswordHash(password , employee.PasswordHash , employee.PasswordSalt))
                return null;
            return employee;
            
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i=0; i < computedHash.Length ;i++)
                {
                    if(computedHash[i] != passwordHash[i])   return false;
                }
            }   
            return true;
            
        }

        public async Task<EmployeeRoot> Register(EmployeeRoot employeeroot, string password)
        {

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash , out passwordSalt);

            employeeroot.PasswordHash=passwordHash;
            employeeroot.PasswordSalt=passwordSalt;

            await _context.EmployeeRoots.AddAsync(employeeroot);
            await _context.SaveChangesAsync();

            return employeeroot;


        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.EmployeeRoots.AnyAsync(x => x.UserName == username))
                return true;
            
            return false;
        }

        public async Task<Department> AddDepartments(Department depts)
        {
             await _context.Departments.AddAsync(depts);
             await _context.SaveChangesAsync();

             return depts;
        }

        public async Task<bool> DeptExists(string deptname)
        {
            if(await _context.Departments.AnyAsync(d => d.DeptName == deptname))
                return true;
            return false;
        }

        public async Task<IEnumerable<EmployeeRoot>> getAllEmps()
        {
            return await _context.EmployeeRoots.ToListAsync();
        }
    }
}



