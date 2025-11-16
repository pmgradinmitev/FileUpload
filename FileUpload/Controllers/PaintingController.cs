using FileUpload.Common.Utils;
using FileUpload.Data;
using FileUpload.Data.Entities;
using FileUpload.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FileUpload.Controllers
{
    public class PaintingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public PaintingController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IQueryable<Painting> query = _context.Set<Painting>();
            PaintingsTableViewModel model = new PaintingsTableViewModel();
            model.Data = query.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            PaintingViewModel model = new PaintingViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PaintingViewModel viewModel)
        {
            Painting entity = new Painting();
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.ImagePath = SaveFile(viewModel.File);
                    viewModel.MapTo(entity);
                    _context.Paintings.Add(entity);
                    _context.SaveChanges();
                    TempData["success"] = $"Картина \"{entity.Name}\" е добавена успешно!";
                }
                catch
                {
                    TempData["error"] = "Картината не може да бъде добавена!";
                }
            }
            return View(viewModel);
        }

        public IActionResult Update(int Id)
        {
            Painting? entity = _context.Paintings.Find(Id);
            if (entity == null)
            {
                TempData["error"] = "картината не може да бъде намерена!";
                return RedirectToAction("Index");
            }
            PaintingViewModel viewModel = new PaintingViewModel();
            viewModel.MapFrom(entity);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(PaintingViewModel viewModel)
        {
            Painting entity = _context.Paintings.Find(viewModel.Id);
            viewModel.ImagePath = entity.ImagePath; //We do not want to lose the image preview
            if (ModelState.IsValid)
            {
                try
                {
                    string oldImagePath = "";
                    if (viewModel.File != null)
                    {
                        oldImagePath = viewModel.ImagePath;
                        viewModel.ImagePath = SaveFile(viewModel.File);
                    }
                    viewModel.MapTo(entity);
                    _context.Paintings.Update(entity);
                    _context.SaveChanges();

                    if (!String.IsNullOrEmpty(oldImagePath))
                        DeleteFile(oldImagePath);
        
                    TempData["success"] = $"Картина \"{entity.Name}\" е записана успешно!";
                }
                catch
                {
                    TempData["error"] = "Промените не бяха записани!";
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Painting? entity = _context.Paintings.Find(Id);
            if (entity == null)
            {
                TempData["error"] = "Такава картина не съществува!";
                return RedirectToAction("Index");
            }
            try
            {
                _context.Paintings.Remove(entity);
                _context.SaveChanges();
                DeleteFile(entity.ImagePath);
                TempData["success"] = $"Картина \"{entity.Name}\" е изтрита успешно!";
            }
            catch
            {
                TempData["error"] = $"Възникна грешка при изтриването!";
            }
            return RedirectToAction("Index");
        }
        private string SaveFile(IFormFile file)
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, FileUtils.GetPaintingImageDir());
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var fileStream = new FileStream(Path.Combine(fullPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string filePath = FileUtils.GetPaintingImageDir() + Path.DirectorySeparatorChar + fileName;
            return filePath;
        }

        private void DeleteFile(string imagePath)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fullPath = Path.Combine(wwwRootPath, imagePath);
                if (!String.IsNullOrEmpty(fullPath) && System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }
            catch
            {
                //add logs here
            }
        }
    }
}
