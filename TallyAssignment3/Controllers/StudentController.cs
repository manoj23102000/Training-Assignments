using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using TallyAssignment3.Models;

namespace TallyAssignment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly TrainDb2Context _dbContext;
        public StudentController(TrainDb2Context dbContext)
        {
            _dbContext = dbContext;
        }

        //Get all students
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _dbContext.Students.ToListAsync();
        }

        //Get student by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            //var student = await _dbContext.Students.FindAsync(id);     //Other Ways
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.StudId == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        //Add new student
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Model state is Invalid");
                }
                if (student.StudId != 0)
                {
                    return BadRequest();
                }
                await _dbContext.Students.AddAsync(student);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction("GetStudents", new { id = student.StudId }, student);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Update existing student
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> UpdateStudent(int id,Student student)
        {
            try
            {
                if (id != student.StudId)
                {
                    return BadRequest();
                }
                var stud = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.StudId == student.StudId);
                if (stud == null)
                {
                    return NotFound();
                }
                _dbContext.Entry(student).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return NoContent();
                //return Ok(student);                     //To get created object in postman as output
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete student
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.StudId == id);
            if (student == null)
            {
                return NotFound();
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
