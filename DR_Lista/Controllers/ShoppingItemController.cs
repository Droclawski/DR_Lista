using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DR_Lista.Data;
using DR_Lista.Models;

namespace DR_Lista.Controllers
{
    public class ShoppingItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public ShoppingItemController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            List<ShoppingItem> objUserList = _context.ShoppingItems.Where(c => c.UserId == userId).ToList();
            return View(objUserList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] ShoppingItem shoppingItem)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User is not logged in.");
                return View(shoppingItem);
            }

            var item = new ShoppingItem
            {
                UserId = userId,
                Description = shoppingItem.Description,
                IsChecked = false
            };

            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); 
                }
                return View(shoppingItem);
            }

            _context.ShoppingItems.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int? id)
        {
            ShoppingItem shoppingItem = _context.ShoppingItems.FirstOrDefault(c => c.Id == id);
            return View(shoppingItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShoppingItem shoppingItem)
        {
            if (!ModelState.IsValid)
            {
                return View(shoppingItem);
            }

            _context.ShoppingItems.Update(shoppingItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult CheckItem(int id)
        {
            var obj = _context.ShoppingItems.FirstOrDefault(c =>  c.Id == id);
            if (obj == null)
            {
                ModelState.AddModelError("Name", "A category with the same name already exists.");
            }
            if (ModelState.IsValid)
            {
                obj.IsChecked = true;
                _context.ShoppingItems.Update(obj);
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var obj = _context.ShoppingItems.FirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                ModelState.AddModelError("Name", "A category with the same name already exists.");
            }
            if (ModelState.IsValid)
            {
                _context.ShoppingItems.Remove(obj);
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
