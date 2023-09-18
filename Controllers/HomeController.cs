using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySaleDbContext context = new MySaleDbContext();
        public IActionResult Index()
        {
            List<Category> categories = context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult Process(int cateID,string cateName,string action)
        {
            if (action.Equals("Add"))
            {
                Category category = new Category()
                {
                    CategoryName = cateName,
                };
                context.Categories.Add(category);
            }
            else
            {
                Category? category = context.Categories.Where(n => n.CategoryId == cateID).FirstOrDefault();
                if (category != null)
                {
                    category.CategoryName = cateName;
                }
            }
            context.SaveChanges();
            return RedirectToAction("");
        }
        public IActionResult Delete(int id)
        {
            Category? category = context.Categories.Where(n => n.CategoryId == id).Include(n => n.Products).FirstOrDefault();
            if(category != null)
            {
                foreach (var item in category.Products)
                {
                    context.Remove(item);
                }
                context.Remove(category);
            }
            context.SaveChanges();
            return RedirectToAction("");
        }
    }
}
