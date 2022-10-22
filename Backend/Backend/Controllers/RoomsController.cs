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
    public class RoomsController : Controller
    {
        private readonly ProjectroomContext _db;

        public RoomsController(ProjectroomContext db) {

            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _db.Room.Include(x=>x.CateRoom)
                .ThenInclude(img=>img.CateImage)
                .Include(f=>f.Floor)
                .ToListAsync();
            return rooms;

        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] Room data)
        {
            try
            {
              
                data.StatusRoom = false;

                await _db.Room.AddAsync(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Create), new { statusCode = "1", mes = "เพิ่มข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Create), e.Message.ToString());
            }
        }

        [HttpPut]
        public async Task<ActionResult> Edit([FromForm] Room data)
        {
            try
            {
                var find = await _db.Room.AsNoTracking().FirstOrDefaultAsync(x => x.RoomId == data.RoomId);
                _db.Room.Update(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Edit), new { statusCode = "1", mes = "แก้ไขข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Edit), e.Message.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Room>> Delete(string id)
        {
            var room = await _db.Room.FindAsync(id);

            try
            {
                _db.Room.Remove(room);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Delete), new { statusCode = "1", mes = "ลบข้อมูลสำเร้จ", room });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Delete), e.Message.ToString());
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(string id)
        {
            var users = await _db.Room.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }
    }
}
