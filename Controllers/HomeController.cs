using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebAliona.Data;
using WebAliona.Models;

namespace WebAliona.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppAlionaContext _context;
        public HomeController(ILogger<HomeController> logger, AppAlionaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Thred id " + Thread.CurrentThread.ManagedThreadId);
            if (!_context.News.Any())
            {
                DataBaseManager db = new DataBaseManager();
                await db.AddNews(_context);
            }
            List<News> list = await GetListBananAsync();
            return View(list);
        }
        private Task<List<News>> GetListBananAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Child Thread: {Thread.CurrentThread.ManagedThreadId}");
                return _context.News.ToList();
            });
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
