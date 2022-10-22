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
    public class TitlesController : Controller
    {
        private readonly ProjectroomContext _db;
        public TitlesController(ProjectroomContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Title>>> GetTitles()
        {
            var titles = await _db.Title.ToListAsync();
            return titles;
        }


        [HttpPost]
        public async Task<ActionResult> CreateTitles([FromForm] Title data)
        {
            try
            {
                await _db.Title.AddAsync(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateTitles), new { statusCode = "1", mes = "เพิ่มข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(CreateTitles), e.Message.ToString());
            }
        }

        [HttpPut]
        public async Task<ActionResult> EditTitle([FromForm] Title data)
        {
            try
            {
                var find = await _db.Title.AsNoTracking().FirstOrDefaultAsync(x => x.TitleId == data.TitleId);
                _db.Title.Update(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(EditTitle), new { statusCode = "1", mes = "แก้ไขข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(EditTitle), e.Message.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Title>> DeleteTitle(int id)
        {
            var titles = await _db.Title.FindAsync(id);

            try
            {
                _db.Title.Remove(titles);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(DeleteTitle), new { statusCode = "1", mes = "ลบข้อมูลสำเร้จ", titles });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(DeleteTitle), e.Message.ToString());
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Title>> GetTitle(int id)
        {
            var users = await _db.Title.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }
    }
}
