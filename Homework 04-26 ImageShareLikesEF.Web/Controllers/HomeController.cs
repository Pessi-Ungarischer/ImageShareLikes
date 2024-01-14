using ImageShareLikes.Web.Models;
using ImageShareLikes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace ImageShareLikes.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString;
        private IWebHostEnvironment _webHostEnvironment;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            ImageRepository imageRepo = new(_connectionString);
            IndexViewModel vm = new()
            {
                Images = imageRepo.GetImages()
            };
            return View(vm);
        }


        public IActionResult ViewImage(int imageId)
        {
            ImageRepository imageRepo = new(_connectionString);
            Image image = imageRepo.GetImageById(imageId);
            if (image == null)
            {
                return Redirect("/");
            }

            List<int> likedIds = HttpContext.Session.Get<List<int>>("likedIds");
            bool canLike = false;
            if (likedIds != null)
            {
                canLike = likedIds.Any(id => id == imageId);
            }

            ViewImageViewModel vm = new()
            {
                Image = image,
                CanLike = canLike
            };
            return View(vm);
        }


        [HttpPost]
        public IActionResult Like(int imageId)
        {
            ImageRepository imageRepo = new(_connectionString);
            Image image = imageRepo.GetImageById(imageId);

            image.Likes++;
            imageRepo.Update(image);

            List<int> likedIds = HttpContext.Session.Get<List<int>>("likedIds") ?? new();
            likedIds.Add(imageId);
            HttpContext.Session.Set("likedIds", likedIds);

            return Json(image.Likes);
        }


        public IActionResult GetLikes(int imageId)
        {
            ImageRepository imageRepo = new(_connectionString);
            Image image = imageRepo.GetImageById(imageId);

            return Json(image.Likes);
        }


        [Authorize]
        public IActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Upload(IFormFile imageFile, string title)
        {
            string name = $"{Guid.NewGuid()}-{imageFile.FileName}";
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", name);
            using var fileStream = new FileStream(FilePath, FileMode.CreateNew);
            imageFile.CopyTo(fileStream);

            Image image = new()
            {
                Title = title,
                ImageFile = name,
                Date = DateTime.Now
            };

            ImageRepository imageRepo = new(_connectionString);
            imageRepo.Upload(image);

            return Redirect("/");
        }


      
    }
}