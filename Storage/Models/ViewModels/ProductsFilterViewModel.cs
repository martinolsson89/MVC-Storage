using Microsoft.AspNetCore.Mvc.Rendering;

namespace Storage.Models.ViewModels;

public class ProductsFilterViewModel
{
    public int Id { get; set; }

    // Sök på produktnamn
    public string? SearchString { get; set; }

    // Vald kategori från dropdownen
    public string? SelectedCategory { get; set; }

    // Kategorier till dropdown-menyn
    public IEnumerable<SelectListItem> Categories { get; set; }
        = Enumerable.Empty<SelectListItem>();

    // Produkter som ska visas i tabellen
    public IEnumerable<Product> Products { get; set; }
        = Enumerable.Empty<Product>();
}
