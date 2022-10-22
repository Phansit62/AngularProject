using Backend.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportProblemsController : ControllerBase
    {
        private readonly ProjectroomContext _db;

        public ReportProblemsController(ProjectroomContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReportProblem>>> GetProblem()
        {
            var re = await _db.ReportProblem.ToListAsync();
            return re;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] ReportProblem data)
        {
            try
            {
                await _db.ReportProblem.AddAsync(data);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Create), new { statusCode = "1", mes = "แจ้งปัญหาสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Create), e.Message.ToString());
            }
        }

        [Route("SendLine")]
        public async Task<ActionResult> SendLine([FromForm] ReportProblem msg)
        {
            var find =await  _db.Orders.FindAsync(msg.OrderId);

                string token = "QNFBbj4PaVKYJx314QT7y3renhC4zczPs345aneazkJ";
                var request =  (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}","ห้องเลขที่"+find.RoomId+":"+msg.ProblemText);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();

                return CreatedAtAction(nameof(SendLine), new { statusCode = "1", mes = "แจ้งปัญหาสำเร็จ" });
            
        }

    }
}
