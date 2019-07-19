using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GMSAPI.DataContext;
using GMSAPI.DTO;
using GMSAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GMSAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public SampleController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }
        public string[] days = { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        [HttpGet("Days")]
        public IEnumerable<string> GetDays()
        {
            return days;
        }
        [HttpGet("Emps")]
        public async Task<IEnumerable<EmployeeRoot>> GetEmployees()
        {
            return await _repo.getAllEmps();
        }
        [HttpPost("registerdepartment")]
        public async Task<IActionResult> RegisterDepartment(DepartmentForRegisterDTO departmentForRegisterDTO)
        {
            departmentForRegisterDTO.deptName = departmentForRegisterDTO.deptName.ToUpper();
            if (await _repo.DeptExists(departmentForRegisterDTO.deptName))
                return BadRequest("Dept Already Exists");
            var deptToCreate = new Department
            {
                DeptName = departmentForRegisterDTO.deptName
            };

            var createdDept = await _repo.AddDepartments(deptToCreate);

            return StatusCode(201);
        }
        [HttpPost("registeremployee")]
        public async Task<IActionResult> Register(EmployeeForRegisterDTO employeeForRegisterDTO)
        {
            // validate the request

            employeeForRegisterDTO.UserName = employeeForRegisterDTO.UserName.ToLower();

            if (await _repo.UserExists(employeeForRegisterDTO.UserName))
                return BadRequest("User Name Taken");

            var empToCreate = new EmployeeRoot
            {
                    Fname = employeeForRegisterDTO.Fname,
                    Lname = employeeForRegisterDTO.Lname,
                    Gender = employeeForRegisterDTO.Gender,
                    DoJ = employeeForRegisterDTO.DoJ,
                    Email = employeeForRegisterDTO.Email,
                    IsAdmin = employeeForRegisterDTO.IsAdmin,
                    deptID = employeeForRegisterDTO.deptID,
                    UserName = employeeForRegisterDTO.UserName

            };

            var createdEmployee = await _repo.Register(empToCreate, employeeForRegisterDTO.Password);
            // return CreatedAtRoute();
            return StatusCode(201);


        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(EmployeeForLoginDto employeeForLoginDto)
        {
            var employeeFromRepo = await _repo.Login(employeeForLoginDto.Username.ToLower(), employeeForLoginDto.Password);
            if (employeeFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , employeeFromRepo.Eid.ToString()),
                new Claim(ClaimTypes.Name,employeeFromRepo.UserName),
                new Claim(ClaimTypes.Role,employeeFromRepo.IsAdmin.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // we create a jwt token handler which allows us to create a token
            var tokenHandler = new JwtSecurityTokenHandler();

            // using the token handler created above we create a token and pass the token description.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // finally we return the token
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}
