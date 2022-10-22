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
    public class CategorysRoomController : Controller
    {
        private readonly ProjectroomContext _db;
        private  IWebHostEnvironment _environment;
        public CategorysRoomController(ProjectroomContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryRoom>>> GetCategoryRooms()
        {
            var cate = await _db.CategoryRoom
                .Include(c=>c.CateImage)
                .Include(r=>r.Room)
                .ToListAsync();
            return cate;
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CategoryRoom data, [FromForm] IFormFileCollection UpFile)
        {
            var chk = _db.CategoryRoom.Where(a => a.CateRoomName == data.CateRoomName).FirstOrDefault();
   
            if (chk != null)
                return CreatedAtAction(nameof(Create), new { statusCode = "0", mes = "พบข้อมูลซ้ำ", data });
            try
            {
                await _db.CategoryRoom.AddAsync(data);
                await _db.SaveChangesAsync();
                #region ImageManageMent
                var path = _environment.WebRootPath + Constants.DirectoryRooms;
                if (UpFile?.Count > 0)
                {
                
                        foreach (var file in UpFile)
                        {
                            var img = new CateImage();

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            //ตัดเอาเฉพาะชื่อไฟล์
                            var fileName = data.CateRoomId + Constants.RoomsImage;
                            if (file.FileName != null)
                            {
                                fileName +=
                                file.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                            }
                            using (FileStream filestream =
                            System.IO.File.Create(path + fileName))
                            {
                                file.CopyTo(filestream);
                                filestream.Flush();
                                img.Path = Constants.DirectoryRooms + fileName;
                            }
                            img.CateRoomId = data.CateRoomId;
                            await _db.CateImage.AddAsync(img);
                            await _db.SaveChangesAsync();
                        }

    

                }
                #endregion
                return CreatedAtAction(nameof(Create), new { statusCode = "1", mes = "เพิ่มข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Create), e.Message.ToString());
            }
        }

        [HttpPut]
        public async Task<ActionResult> Edit([FromForm] CategoryRoom data, [FromForm] IFormFileCollection UpFile)
        {
            var find = await _db.CategoryRoom.AsNoTracking()
                .Include(p=>p.CateImage)
                .Include(r=>r.Room)
                .FirstOrDefaultAsync(x => x.CateRoomId == data.CateRoomId);
            var img = await _db.CateImage.Where(x=>x.CateRoomId == data.CateRoomId).ToArrayAsync();
            try
            {
                _db.CategoryRoom.Update(data);
                await _db.SaveChangesAsync();
                #region ImageManageMent
                var path = _environment.WebRootPath + Constants.DirectoryRooms;
                if (UpFile?.Count > 0)
                {

                    for (int i=0;i<UpFile.Count;i++)
                    {

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        //ตัดเอาเฉพาะชื่อไฟล์
                        var fileName = data.CateRoomId + Constants.RoomsImage;
                        if (UpFile[i].FileName != null)
                        {
                            fileName +=
                            UpFile[i].FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                        }
                        using (FileStream filestream =
                        System.IO.File.Create(path + fileName))
                        {
                            UpFile[i].CopyTo(filestream);
                            filestream.Flush();
                            img[i].Path = Constants.DirectoryRooms + fileName;
                        }
                        _db.CateImage.Update(img[i]);
                        await _db.SaveChangesAsync();
                    }



                }
                #endregion
                return CreatedAtAction(nameof(Edit), new { statusCode = "1", mes = "แก้ไขข้อมูลสำเร็จ", data });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Edit), e.Message.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryRoom>> Delete(int id)
        {
            var cate = await _db.CategoryRoom    
                .Include(c => c.CateImage)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(x => x.CateRoomId == id);

         
            try
            {
                _db.CategoryRoom.Remove(cate);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Delete), new { statusCode = "1", mes = "ลบข้อมูลสำเร้จ", cate });
            }
            catch (Exception e)
            {
                return CreatedAtAction(nameof(Delete), e.Message.ToString());
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryRoom>> GetCategory(int id)
        {
            var cate = await _db.CategoryRoom
                .Include(c => c.CateImage)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(x => x.CateRoomId == id);

            if (cate == null)
            {
                return NotFound();
            }

            return cate;
        }

        [HttpPut("UploadImageSelect")]
        public async Task<ActionResult<CategoryRoom>> UploadImage(int id,[FromForm] IFormFile UpFile)
        {
            #region ImageManageMent
            var path = _environment.WebRootPath + Constants.DirectoryRooms;
            if (UpFile?.Length > 0)
            {


                    var img = await _db.CateImage.FindAsync(id);

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    //ตัดเอาเฉพาะชื่อไฟล์
                    var fileName = img.CateRoomId + Constants.RoomsImage;
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
                        img.Path = Constants.DirectoryRooms + fileName;
                    }
                    _db.CateImage.Update(img);
                    await _db.SaveChangesAsync();
                



            }
            #endregion
            return CreatedAtAction(nameof(UploadImage), new { statusCode = "1", mes = "แก้ไขข้อมูลสำเร็จ" });
        }
    }
}
