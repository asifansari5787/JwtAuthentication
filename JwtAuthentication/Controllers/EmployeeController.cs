using JwtAuthentication.Data;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        //private static List<Employee> employees = new List<Employee>()
        //{
        //    new Employee() { Id = 1, Name = "Asif", Email = "asif@gmail.com", Phone = "9359660967", Salary = 20000},
        //    new Employee() { Id = 2, Name = "Adeel", Email = "adeel@gmail.com", Phone = "9876345609", Salary = 10000},
        //    new Employee() { Id = 3, Name = "Suffyaan", Email = "suffyaan@gmail.com", Phone = "9559670154", Salary = 15000},
        //    new Employee() { Id = 4, Name = "Adnan", Email = "adnan@gmail.com", Phone = "8408964592", Salary = 30000},
        //    new Employee() { Id = 4, Name = "Shadub", Email = "shadub@gmail.com", Phone = "9956298436", Salary = 300}
        //};

        private readonly DataContext _context;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Employee>> GetAllEmployees()
        {
            var employee = _context.Employees.ToList();

            return Ok(employee);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
               return NotFound("Employee does not exist");
            }

            return Ok(employee);
        }

        [HttpPost("Add")]
        public ActionResult<List<Employee>> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            //employees.Add(employee);
            var result = _context.Employees;

            return Ok(result);
        }

        [HttpPut("Update")]
        public ActionResult<Employee> UpdateEmployee(Employee request)
        {
            var employee = _context.Employees.FirstOrDefault(c => c.Id == request.Id);
            if(employee == null)
            {
                return NotFound("Employee not found");
            }

            employee.Name = request.Name;
            employee.Email = request.Email;
            employee.Phone = request.Phone;
            employee.Salary = request.Salary;

            _context.SaveChanges();
            return Ok(employee);

        }

        [HttpDelete("Delete")]
        public ActionResult<List<Employee>> DeleteEmployee(int id)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id);
            if(employee == null)
            {
                return BadRequest("Employee not found");
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            var result = _context.Employees;
            return Ok(result);
        }
    }
}
