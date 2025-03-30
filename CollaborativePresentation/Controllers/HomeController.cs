using CollaborativePresentation.Data;
using CollaborativePresentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollaborativePresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var presentations = await _context.Presentations.ToListAsync();
            return View(presentations);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePresentation(string name, string creatorName)
        {
            var presentation = new Presentation
            {
                Name = name,
                CreatorName = creatorName
            };

            _context.Presentations.Add(presentation);
            await _context.SaveChangesAsync();

            // Add first slide
            var slide = new Slide
            {
                PresentationId = presentation.Id,
                Order = 0
            };
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction("Presentation", new { id = presentation.Id, userName = creatorName });
        }

        public async Task<IActionResult> Presentation(string id, string userName)
        {
            var presentation = await _context.Presentations
                .Include(p => p.Slides)
                .ThenInclude(s => s.TextElements)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (presentation == null)
            {
                return NotFound();
            }

            ViewBag.UserName = userName;
            ViewBag.IsCreator = presentation.CreatorName == userName;
            return View(presentation);
        }

        [HttpGet]
        public async Task<IActionResult> GetSlide(string id)
        {
            var slide = await _context.Slides
                .Include(s => s.TextElements)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (slide == null) return NotFound();

            return Json(new
            {
                id = slide.Id,
                textElements = slide.TextElements.Select(e => new {
                    id = e.Id,
                    content = e.Content,
                    positionX = e.PositionX,
                    positionY = e.PositionY,
                    width = e.Width,
                    height = e.Height
                })
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddSlide(string presentationId, string userName)
        {
            var presentation = await _context.Presentations
                .Include(p => p.Slides)
                .FirstOrDefaultAsync(p => p.Id == presentationId);

            if (presentation == null || presentation.CreatorName != userName)
            {
                return Forbid();
            }

            var slide = new Slide
            {
                PresentationId = presentationId,
                Order = presentation.Slides.Count
            };

            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction("Presentation", new { id = presentationId, userName });
        }
    }
}