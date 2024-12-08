using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;
using ProjectWeb.Models;

namespace ProjectWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class BookController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(ApplicationDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Book> listBook = _dbContext.Books.Include("Category").ToList();
            return View(listBook);
        }
        public IActionResult Add()
        {
            BookVM bookVM = new BookVM()
            {
                Categories = _dbContext.Categories.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Book = new Book()
            };
            return View(bookVM);
        }
        [HttpPost]
        public IActionResult Add(Book Book, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string bookPath = Path.Combine(wwwRootpath, @"image\books");
                    using (var fileStream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Book.ImageUrl = @"\image\books\" + fileName;
                }
                _dbContext.Books.Add(Book);
                _dbContext.SaveChanges();
                TempData["success"] = "Book is added succesfully";
                return RedirectToAction("Index");
            }
            BookVM bookVM = new BookVM()
            {
                Categories = _dbContext.Categories.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Book = new Book()
            };
            return View(bookVM);
        }
        public IActionResult Edit(int id)
        {
            BookVM bookVM = new BookVM()
            {
                Categories = _dbContext.Categories.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Book = _dbContext.Books.FirstOrDefault(c => c.Id == id)
            };
            if (bookVM.Book == null)
            {
                return NotFound();
            }
            return View(bookVM);
        }
        [HttpPost]
        public IActionResult Edit(Book Book, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string bookPath = Path.Combine(wwwRootpath, @"image\books");
                    if (!string.IsNullOrEmpty(Book.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootpath, Book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Book.ImageUrl = @"\image\books\" + fileName;
                }
                _dbContext.Books.Update(Book);
                _dbContext.SaveChanges();
                TempData["success"] = "Book is updated succesfully";
                return RedirectToAction("Index");
            }
            TempData["failed"] = "Book can not be updated";
            BookVM bookVM = new BookVM()
            {
                Categories = _dbContext.Categories.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Book = _dbContext.Books.FirstOrDefault(c => c.Id == Book.Id)
            };
            if (bookVM.Book == null)
            {
                return NotFound();
            }
            return View(bookVM);
        }
        public IActionResult Delete(int id)
        {
            Book? Book = _dbContext.Books.FirstOrDefault(c => c.Id == id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }
        [HttpPost]
        public IActionResult Delete(Book Book)
        {
            _dbContext.Books.Remove(Book);
            _dbContext.SaveChanges();
            TempData["success"] = "Book is deleted succesfully";
            return RedirectToAction("Index");

        }
    }
}
