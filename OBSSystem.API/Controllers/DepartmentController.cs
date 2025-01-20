using Microsoft.AspNetCore.Mvc;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;

namespace OBSSystem.API.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class DepartmentController : ControllerBase
        {
            private readonly OBSContext _context;

            public DepartmentController(OBSContext context)
            {
                _context = context;
            }

            // Yeni bir bölüm ekleme
            [HttpPost]
            public IActionResult CreateDepartment([FromBody] Department department)
            {
                if (department == null)
                {
                    return BadRequest(new { message = "Invalid department data" });
                }

                _context.Departments.Add(department);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentID }, department);
            }

            // Bölümleri listeleme
            [HttpGet]
            public IActionResult GetAllDepartments()
            {
                var departments = _context.Departments.ToList();
                return Ok(departments);
            }

            // Bölüm detaylarını alma
            [HttpGet("{id}")]
            public IActionResult GetDepartmentById(int id)
            {
                var department = _context.Departments.Find(id);
                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                return Ok(department);
            }

            // Bölüm güncelleme
            [HttpPut("{id}")]
            public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
            {
                var department = _context.Departments.Find(id);
                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                department.DepartmentName = updatedDepartment.DepartmentName;
                department.Faculty = updatedDepartment.Faculty;

                _context.SaveChanges();
                return Ok(department);
            }

            // Bölüm silme
            [HttpDelete("{id}")]
            public IActionResult DeleteDepartment(int id)
            {
                var department = _context.Departments.Find(id);
                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                _context.Departments.Remove(department);
                _context.SaveChanges();

                return NoContent();
            }
        }

    }
