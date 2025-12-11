using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Storage.Data;
using Storage.Models;
using Storage.Models.ViewModels;

namespace Storage.Controllers;

public class ProductsController : Controller
{
    private readonly StorageContext _context;


    public ProductsController(StorageContext context)
    {
        _context = context;
    }

    // GET: Products/ProductSummary
    public async Task<IActionResult> ProductSummary()
    {
        // GroupBy i databasen → en rad per kategori
        var categoryData = await _context.Product
            .GroupBy(p => p.Category)
            .Select(g => new CategorySummaryViewModel
            {
                Category = g.Key,
                ProductCount = g.Sum(p => p.Count),
                InventoryValue = g.Sum(p => p.Price * p.Count)
            })
            .ToListAsync();

        var viewModel = new ProductSummaryViewModel
        {
            Categories = categoryData,
            TotalProductCount = categoryData.Sum(c => c.ProductCount),
            TotalInventoryValue = categoryData.Sum(c => c.InventoryValue)
        };

        return View(viewModel);
    }

    // GET: Products/Filter

    public async Task<IActionResult> ProductsFilter(string? searchString, string? selectedCategory)
    {
        var productsQuery = _context.Product.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            productsQuery = 
                productsQuery.Where(p => p.Name.Contains(searchString));
        }

        if (!string.IsNullOrWhiteSpace(selectedCategory))
        {
            productsQuery = 
                productsQuery.Where(x => x.Category == selectedCategory);

        }

        var products = await productsQuery.ToListAsync();

        var categories = await _context.Product
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        var viewModel = new ProductsFilterViewModel
        {
            SearchString = searchString,
            SelectedCategory = selectedCategory,
            Products = products,
            Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c,
                    Text = c
                })
        };

        return View(viewModel);
    }


    // GET: ProductByCategory

    public async Task<IActionResult> ProductsByCategory(string category)
    {
        var products = _context.Product.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            products = products.Where(x => x.Category == category);

        }

        var filteredProducts = await products.ToListAsync();

        return View("Index", filteredProducts);
    }

    // GET: Products/Inventory
    public async Task<IActionResult> Inventory()
    {
        IEnumerable<ProductViewModel> productViewModels = await _context.Product
            .Select(p => new ProductViewModel
            {
                Name = p.Name,
                Price = p.Price,
                Count = p.Count,
                InventoryValue = p.Price * p.Count
            })
            .ToListAsync();

        return View(productViewModels);
    }

    // GET: Products
    public async Task<IActionResult> Index(string? searchString, string? selectedCategory)
    {
        return View(await _context.Product.ToListAsync());
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Products/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Product.Any(e => e.Id == id);
    }
}
