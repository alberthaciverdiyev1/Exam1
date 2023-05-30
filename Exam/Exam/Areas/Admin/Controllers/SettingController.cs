using Exam.DAL;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            List<Setting> settings = _context.Settings.ToList();
            return View(settings);
        }
        public async Task<IActionResult> Update(int id)
        {if (id == null || id < 1) return BadRequest();

            Setting settings =await _context.Settings.FirstOrDefaultAsync(x=>x.id==id);
            if (settings == null)return NotFound();



            return View(settings);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Setting setting)
        {
            if (id == null || id < 1) return BadRequest();

            Setting exists = await _context.Settings.FirstOrDefaultAsync(x => x.id == id);
            if (exists == null) return NotFound();

            exists.Value=setting.Value;
           await _context.SaveChangesAsync();
            return RedirectToAction("Index","setting");
        }
    }
}
