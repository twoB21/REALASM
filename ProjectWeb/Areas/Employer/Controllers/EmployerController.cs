using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Data;
using ProjectWeb.Models;
using System.Threading.Tasks;

namespace ProjectWeb.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployerController(ApplicationDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var employerId = User.Identity.Name; 
            var jobListings = _dbContext.JobListings.Where(j => j.EmployerId == employerId).ToList();

            return View(jobListings); 
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(JobListing jobListing)
        {
            if (ModelState.IsValid)
            {
                jobListing.EmployerId = User.Identity.Name; // Gán EmployerId cho JobListing

                _dbContext.JobListings.Add(jobListing); // Thêm JobListing mới vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                TempData["success"] = "Job listing added successfully!";
                return RedirectToAction("Index");
            }

            // Nếu model không hợp lệ, trả lại form
            return View(jobListing);
        }
    }
}
