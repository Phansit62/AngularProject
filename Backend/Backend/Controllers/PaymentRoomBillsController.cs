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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentRoomBillsController : ControllerBase
    {
        private readonly ProjectroomContext _db;
        private IWebHostEnvironment _environment;


        public PaymentRoomBillsController(ProjectroomContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<List<PaymentBill>>> GetPayments()
        {
            var pay = await _db.PaymentBill
                .Include(x => x.RoomBill)
                .ThenInclude(x=>x.Order)
                .ThenInclude(x=>x.Room)
                .ThenInclude(c=>c.CateRoom)
                  .Include(x => x.RoomBill)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Cus)
                .ToListAsync();
            return pay;

        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] PaymentBill data, [FromForm] IFormFile UpFile)
        {

            #region ImageManageMent
            var path = _environment.WebRootPath + Constants.DirectoryPaymentsBill;
            if (UpFile?.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    //ตัดเอาเฉพาะชื่อไฟล์
                    var fileName = data.PaymentPic + Constants.PaymentsBillImage;
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
                        data.PaymentPic = Constants.DirectoryPaymentsBill + fileName;
                    }

                }
                catch (Exception ex)
                {
                    return CreatedAtAction(nameof(Create), ex.Message.ToString());
                }

            }
            #endregion
            data.PaymentBillDate = DateTime.Now;
            await _db.PaymentBill.AddAsync(data);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new { statusCode = "1", mes = "ชำระเงินสำเร็จ", data });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentBill>> GetDetaillPayments(int id)
        {
            var pay = await _db.PaymentBill
                .Include(x => x.RoomBill)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Room)
                .ThenInclude(c => c.CateRoom)
                  .Include(x => x.RoomBill)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Cus)
                .FirstOrDefaultAsync(x => x.PaymentBillId == id);
            return pay;

        }

    }


}
