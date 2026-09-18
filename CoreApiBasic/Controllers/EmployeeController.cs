using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiBasic.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiBasic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        // Action Method with Multiple Routes
        [HttpGet("GetAll")]
        [HttpGet("All")]
        [HttpGet("GetEmployees")]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            return EmployeeData.Employees;
        }
    }
}