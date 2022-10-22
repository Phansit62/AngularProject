using Backend.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly ProjectroomContext _db;

        public CustomersController(ProjectroomContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            var customers = await _db.Customer.Include(x=>x.Title).ToListAsync();
            return customers;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> CreateCustomer([FromForm] Customer data)
        {
            try {
                var chk = await _db.Customer.FirstOrDefaultAsync(c => c.Username.Equals(data.Username));
                if (chk != null)
                {
                    return CreatedAtAction(nameof(CreateCustomer), new { statusCode = "0", msg = "ไม่สามารถสมัครสมาชิกได้เนื่องจากชื่อผู้ใช้ซ้ำ" });
                }
                _db.Customer.Add(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateCustomer), new { statusCode = "1", msg = "สมัครสมาชิกสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(CreateCustomer), e.Message.ToString());
            }
        }
        [HttpPut]
        public async Task<ActionResult> EditCustomer ([FromForm] Customer data)
        {
            var result = await _db.Customer.AsNoTracking().FirstOrDefaultAsync(p => p.CusId == data.CusId);

            _db.Customer.Update(data);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(EditCustomer), new { statusCode = "1", msg = "แก้ไขข้อมูลสำเร็จ", data });
        }

        [HttpGet("SaveToken")]
        public async Task<ActionResult<Customer>> SaveToken(int id,string Token)
        {
            var find = await _db.Customer.FindAsync(id);
            find.Token = Token;
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(SaveToken), new { statusCode = '1', msg = "เพิ่มโทเคนสำเร็จ" });
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] Customer data)
        {
            var result = await _db.Customer
                .Include(t=>t.Title)
                .Include(o=>o.Orders)
                .ThenInclude(r=>r.Room)
                .FirstOrDefaultAsync(user => user.Username.Equals(data.Username) && user.Password.Equals(data.Password));

            if (result == null)
                return CreatedAtAction(nameof(Login), new { statusCode = '0',msg = "ไม่พบผู้ใช้งาน" });
            return CreatedAtAction(nameof(Login), new { statusCode = '1',msg = "เข้าสุ่ระบบสำเร็จ", result });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var users = await _db.Customer.Include(t=>t.Title)
                .Include(o=>o.Orders)
                .FirstOrDefaultAsync(x=>x.CusId == id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }


        [HttpGet("Booking")]
        public async Task<ActionResult> AddOrder(string roomId,int cusId)
        {
            var orders = new Orders();
            var room = await _db.Room.FindAsync(roomId);
            orders.CusId = cusId;
            orders.RoomId = roomId;
            room.StatusRoom = true;
            orders.DateIn = DateTime.Now;
            
            orders.StatusOrder = false;
            await _db.Orders.AddAsync(orders);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(AddOrder), new { statusCode = '1', msg = "จองห้องพักสำเร็จ",orderId=orders.OrderId });
        }

        [HttpGet("GetBill/{id}")]
        public async Task<ActionResult<List<RoomBill>>> GetBillForUser(int id)
        {
            var bill = await _db.RoomBill.
                Include(x => x.Order)
                .ThenInclude(o => o.Room)
                .Include(x => x.Order)
                .ThenInclude(o => o.Cus)
                .Where(p => p.Order.CusId == id)
                .ToListAsync();

            if (bill == null)
            {
                return NotFound();  
            }

            return bill;
        }



    }
}
