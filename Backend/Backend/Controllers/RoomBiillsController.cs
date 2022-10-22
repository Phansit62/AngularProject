using Backend.Models.Data;
using Microsoft.AspNetCore.Http;
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
    public class RoomBiillsController : ControllerBase
    {
        private readonly ProjectroomContext _db;

        public RoomBiillsController(ProjectroomContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomBill>>> GetRoomBills()
        {
            var list = await _db.RoomBill
                .Include(o=>o.Order)
                .ThenInclude(c=>c.Cus)
                .Include(o=>o.Order)
                .ThenInclude(r=>r.Room)
                .ToListAsync();
            return list;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] RoomBill data)
        {
            var find = await _db.Orders.Include(r=>r.Room).ThenInclude(c=>c.CateRoom)
                .FirstOrDefaultAsync(x=>x.OrderId == data.OrderId);
            try
            {
                data.Total = data.Water + data.Electricity + find.Room.CateRoom.Price;
                data.BillDate = DateTime.Now;
                data.StatusBill = false;
                await _db.RoomBill.AddAsync(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Create), new { statusCode = "1", mes = "เพิ่มบิลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Create), e.Message.ToString());
            }
        }


        [HttpPut]
        public async Task<ActionResult> Edit([FromForm] RoomBill data)
        {
            var find = await _db.RoomBill.AsNoTracking().FirstOrDefaultAsync(x => x.RoomBillId == data.RoomBillId);

            try
            {
                _db.RoomBill.Update(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Edit), new { statusCode = "1", mes = "แก้ไขข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Edit), e.Message.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomBill>> GetRoomBill(int id)
        {
            var get = await _db.RoomBill
                .Include(x=>x.Order)
                .ThenInclude(o=>o.Cus)
                .Include(x => x.Order)
                .ThenInclude(o => o.Room)
                .FirstOrDefaultAsync(x=>x.RoomBillId == id)
                ;

            if (get == null)
            {
                return NotFound();
            }

            return get;
        }

        [HttpGet("Confirm/{id}")]
        public async Task<ActionResult> Confirm(int id)
        {
            var con = await _db.RoomBill.Where(a => a.RoomBillId == id).FirstOrDefaultAsync();
            con.StatusBill = true;
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Confirm), new { statusCode = "1", mes = "ยืนยันสำเร็จ" });
        }



        [Route("SendLine")]
        public async Task<ActionResult> SendLine(int billId)
        {
            var find = await _db.RoomBill.Include(b => b.Order)
                .ThenInclude(o => o.Room)
                .Include(x=>x.Order)
                .ThenInclude(c=>c.Cus)
                .FirstOrDefaultAsync(x => x.RoomBillId == billId);

            string token = find.Order.Cus.Token;
            var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
            var postData = string.Format("message={0}", "ห้องเลขที่" + find.Order.RoomId   + " กรุณาชำระเงินจำนวน " + find.Total+" บาท" + " ภายในวันที่ " + find.BillDate.Value.ToLongDateString());
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add("Authorization", "Bearer " + token);

            using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
            var response = (HttpWebResponse)request.GetResponse();

            return CreatedAtAction(nameof(SendLine), new { statusCode = "1", msg = "แจ้งการชำระเงินสำเร็จ" });

        }
    }
}
