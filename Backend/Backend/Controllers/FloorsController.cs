using Backend.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FloorsController : Controller
    {
        private readonly ProjectroomContext _db;

        public FloorsController(ProjectroomContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Floor>>> GetFloors()
        {
            var floors = await _db.Floor.ToListAsync();
            return floors;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] Floor data)
        {
            try {
                await _db.Floor.AddAsync(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Create), new { statusCode ="1", mes="เพิ่มข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Create), e.Message.ToString());
            }
        }

        [HttpPut]
        public async Task<ActionResult> Edit([FromForm] Floor data)
        {
            var find = await _db.Floor.AsNoTracking().FirstOrDefaultAsync(x => x.FloorId == data.FloorId);

            try
            {
                _db.Floor.Update(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Edit), new { statusCode = "1", mes = "แก้ไขข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Edit), e.Message.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Floor>> DeleteFloor(int id)
        {
            var floor = await _db.Floor.FindAsync(id);

            try
            {
                _db.Floor.Remove(floor);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(DeleteFloor), new { statusCode="1",mes="ลบข้อมูลสำเร้จ",floor});
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(DeleteFloor), e.Message.ToString());
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Floor>> GetFloor(int id)
        {
            var floors = await _db.Floor.FindAsync(id);

            if (floors == null)
            {
                return NotFound();
            }

            return floors;
        }
    }
}
