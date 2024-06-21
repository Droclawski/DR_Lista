using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DR_Lista.Data;
using DR_Lista.Models;

[Authorize]
public class ShoppingListsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public ShoppingListsController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var shoppingLists = await _context.ShoppingLists
            .Where(c => c.UserId == userId)
            .Include(s => s.ShoppingItems)
            .ToListAsync();

        var allItems = await _context.ShoppingItems.Where(c => c.UserId == userId).ToListAsync();
        ViewBag.AllItems = allItems;

        return View(shoppingLists);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Date")] ShoppingList shoppingList)
    {
        var userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError(string.Empty, "User is not logged in.");
            return View(shoppingList);
        }

        var item = new ShoppingList
        {
            UserId = userId,
            Title = shoppingList.Title, 
            Date = shoppingList.Date
        };

        ModelState.Remove("UserId");

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(shoppingList);
        }

        _context.ShoppingLists.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(int shoppingListId, int selectedItemId)
    {
        if (selectedItemId > 0)
        {
            var shoppingList = await _context.ShoppingLists
                .Include(s => s.ShoppingItems)
                .FirstOrDefaultAsync(s => s.Id == shoppingListId);

            if (shoppingList != null)
            {
                var shoppingItem = await _context.ShoppingItems.FirstOrDefaultAsync(i => i.Id == selectedItemId);
                if (shoppingItem != null && !shoppingList.ShoppingItems.Contains(shoppingItem))
                {
                    shoppingList.ShoppingItems.Add(shoppingItem);
                    await _context.SaveChangesAsync();
                }
            }
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> CheckItem(int id)
    {
        var shoppingItem = await _context.ShoppingItems.FindAsync(id);
        if (shoppingItem == null)
        {
            return NotFound();
        }

        shoppingItem.IsChecked = !shoppingItem.IsChecked;
        _context.Update(shoppingItem);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "ShoppingLists");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var shoppingList = await _context.ShoppingLists
            .Include(s => s.ShoppingItems)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (shoppingList != null)
        {
            _context.ShoppingItems.RemoveRange(shoppingList.ShoppingItems);

            _context.ShoppingLists.Remove(shoppingList);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteItem(int id, int shoppingListId)
    {
        var shoppingList = await _context.ShoppingLists
            .Include(s => s.ShoppingItems)
            .FirstOrDefaultAsync(s => s.Id == shoppingListId);

        if (shoppingList == null)
        {
            return NotFound();
        }

        var shoppingItem = shoppingList.ShoppingItems.FirstOrDefault(i => i.Id == id);
        if (shoppingItem == null)
        {
            return NotFound();
        }

        shoppingList.ShoppingItems.Remove(shoppingItem);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


}


