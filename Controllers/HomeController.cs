using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAliona.Data;
using WebAliona.Models;

namespace WebAliona.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppAlionaContext _context;

        public HomeController(ILogger<HomeController> logger,
            AppAlionaContext context)
        {
            _logger = logger;
            _context = context;
            if (!_context.Banans.Any())
            {
                DataBaseManager dbm = new DataBaseManager();
                dbm.AddBanans(_context); 
            }
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Thread main id " + Thread.CurrentThread.ManagedThreadId);
            List<Banan> list = await GetListBanansAsync();
            //Task<List<Banan>> list = GetListBanansAsync();
            return View(list);
        }

        [HttpGet] //Цей метод буде відображати сторінку де можвка вказаи інформацію про користувача
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] //Цей метод буде відображати сторінку де можвка вказаи інформацію про користувача
        public async Task<IActionResult> Create(IFormFile ImageFile, [FromForm] string FirstName, [FromForm] string LastName,[FromForm] string Phone, [FromForm] bool Sex)
        {
            string? image = null;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                image = await SaveImageAsync(ImageFile);
            }

            Banan banan = new Banan
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Sex = Sex,
                Image = image
            };
            await _context.AddAsync(banan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Task<List<Banan>> GetListBanansAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Child Thread: {Thread.CurrentThread.ManagedThreadId}");
                return _context.Banans.ToList();
            });
        }
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            string upload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(upload))
            {
                Directory.CreateDirectory(upload);
            }
               

            string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(upload, FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + FileName; 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}