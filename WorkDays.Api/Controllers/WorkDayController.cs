using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkDays.Api.Interfaces;
using WorkDays.Api.Models;
using WorkDays.Api.Services;
using WorkDays.Data.Models;

namespace WorkDays.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkDayController : ControllerBase
    {
        private readonly IWorkDayService _service;

        public WorkDayController(IWorkDayService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(_service));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkDayDto>>> GetAll()
        {
            var result = await _service.GetAllWorkDaysAsync();
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkDayDto>> GetById(int id)
        {
            var result = await _service.GetWorkDayByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<WorkDayDto>> Add([FromBody] WorkDayDto dto)
        {
            if (dto is null)
                return BadRequest("WorkDay data is null.");
            var result = await _service.AddWorkDayAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.WorkDayId }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkDayDto>> Update(int id, [FromBody] WorkDayDto dto)
        {
            if (dto == null || dto.WorkDayId != id)
                return BadRequest("WorkDay data is null");
            var updateWorkDay = await _service.UpdateWorkDayAsync(dto);
            return Ok(updateWorkDay);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workDay = await _service.GetWorkDayByIdAsync(id);
            if (workDay == null)
                return NotFound();
            await _service.DeleteWorkDayAsync(id);
            return NoContent();
        }
    }
}
