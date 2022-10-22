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
    public class AdminController : Controller
    {
        private readonly ProjectroomContext _db;
        public AdminController(ProjectroomContext db)
        {
            
            _db = db;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] Admin data)
        {

            var chk = await _db.Admin.Where(a => a.Username == data.Username && a.Password == data.Password).FirstOrDefaultAsync();
            if(chk != null)
                return CreatedAtAction(nameof(Login), new { statusCode = "1", mes = "เข้าสู่ระบบสำเร็จ", data });
            else
                return CreatedAtAction(nameof(Login), new { statusCode = "0", mes = "เข้าสู่ระบบไม่สำเร็จ", data });
        }
    }
}
