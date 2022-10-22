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
    public class OrdersController : Controller
    {
        private readonly ProjectroomContext _db;
        public OrdersController(ProjectroomContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<List<Orders>>> GetOrders()
        {
            var o = await _db.Orders.Include(x=>x.Cus)
                .Include(r=>r.Room)
                .ThenInclude(c=>c.CateRoom)
                .ToListAsync();
            return o;
        }

        [HttpGet("Confirm/{id}")]
        public async Task<ActionResult> Confirm(int id)
        {
            var con = await _db.Orders.Where(a => a.OrderId == id).FirstOrDefaultAsync();
            con.StatusOrder = true;
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Confirm), new { statusCode = "1", mes = "ยืนยันสำเร็จ" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrder(int id)
        {
            var order = await _db.Orders.Include(x => x.Cus)
                .Include(r => r.Room)
                .ThenInclude(c=>c.CateRoom)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

    }
}
