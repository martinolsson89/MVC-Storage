namespace Storage.Models.ViewModels;

public class CategorySummaryViewModel
{
    public string Category { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public int InventoryValue { get; set; } // Summa Price * Count i kategorin
}
