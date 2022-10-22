using Backend.Helpers;
using Backend.Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        private readonly ProjectroomContext _db;
        private IWebHostEnvironment _environment;

        public PaymentsController(ProjectroomContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetPayments()
        {
            var pay = await _db.Payment
                .Include(x => x.PaymentOrder)
                .ThenInclude(o=>o.Room)
                .ThenInclude(c=>c.CateRoom)
                .Include(x=>x.PaymentOrder)
                .ThenInclude(c=>c.Cus)
                .ToListAsync();
            return pay;
            
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] Payment data, [FromForm] IFormFile UpFile)
        {
            
            #region ImageManageMent
            var path = _environment.WebRootPath + Constants.DirectoryPayments;
            if (UpFile?.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    //ตัดเอาเฉพาะชื่อไฟล์
                    var fileName = data.PaymentId + Constants.PaymentsImage;
                    if (UpFile.FileName != null)
                    {
                        fileName +=
                        UpFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                    }
                    using (FileStream filestream =
                    System.IO.File.Create(path + fileName))
                    {
                        UpFile.CopyTo(filestream);
                        filestream.Flush();
                        data.PaymentPic = Constants.DirectoryPayments + fileName;
                    }

                }
                catch (Exception ex)
                {
                    return CreatedAtAction(nameof(Create), ex.Message.ToString());
                }

            }
            #endregion
            data.PaymentDate = DateTime.Now;
            await _db.Payment.AddAsync(data);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new { statusCode = "1", mes = "ชำระเงินสำเร็จ", data });
        }
        [HttpGet("GetDetaillPayments/{id}")]
        public async Task<ActionResult<List<Payment>>> GetDetaillPayments(int id)
        {
            var pay = await _db.Payment.Where(a=>a.PaymentId == id).ToListAsync();
            return pay;

        }

        [HttpGet("Confirm/{id}")]
        public async Task<ActionResult> Confirm(int id)
        {
            var con = await _db.Payment.FindAsync(id);
            con.StatusPay = true;
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Confirm), new { statusCode = "1", mes = "ยืนยันสำเร็จ"});
        }

    }
}
