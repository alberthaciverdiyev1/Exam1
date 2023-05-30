using Exam.DAL;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            ViewBag.Page = page;
            ViewBag.Total =Math.Ceiling((decimal) _context.Portfolios.Count() / 5);


            List<Portfolio> portfolios = await _context.Portfolios.Skip(page*5).Take(5).ToListAsync();
            return View(portfolios);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Portfolio portfolio)
        {
            if (portfolio.Photo.ContentType.Contains("image/"))
            {
                if (portfolio.Photo.Length < 1024 * 500)
                {
                    string filename = Guid.NewGuid().ToString() + portfolio.Photo.FileName;
                    string path = Path.Combine(_env.WebRootPath, "assets/img", filename);
                    FileStream stream = new FileStream(path, FileMode.Create);
                    await portfolio.Photo.CopyToAsync(stream);
                    stream.Close();
                    portfolio.ImageUrl = filename;
                    await _context.AddAsync(portfolio);
                    await _context.SaveChangesAsync();
                }
                else
                {
                   ModelState.AddModelError(string.Empty, "Seklin olcusu 500kbdan boyuk ola bilmez");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Seklin formati duzgun deyil");
                return View();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null || id < 1) return BadRequest();
            Portfolio portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            if (portfolio == null) { return NotFound(); }
            string filename = Guid.NewGuid().ToString() + portfolio.ImageUrl;
            string path = Path.Combine(_env.WebRootPath, "assets/img", filename);
            System.IO.File.Delete(path);
            _context.Remove(portfolio);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Update(int id)
        {


            if (id == null || id < 1) return BadRequest();
            Portfolio portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            if (portfolio == null) { return NotFound(); }
            return View(portfolio);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Portfolio portfolio)
        {
            if (id == null || id < 1) return BadRequest();
            Portfolio existed = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            if (portfolio == null) { return NotFound(); }

            string filename = Guid.NewGuid().ToString() + portfolio.ImageUrl;
            string path = Path.Combine(_env.WebRootPath, "assets/img", filename);
            if (portfolio.Photo != null)
            {


                System.IO.File.Delete(path);
                string newfile = Guid.NewGuid().ToString() + portfolio.Photo.FileName;
                string newPath = Path.Combine(_env.WebRootPath, "assets/img", newfile);
                FileStream stream = new FileStream(newPath, FileMode.Create);
                await portfolio.Photo.CopyToAsync(stream);
                existed.ImageUrl = newfile;



            }
            existed.Name = portfolio.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
