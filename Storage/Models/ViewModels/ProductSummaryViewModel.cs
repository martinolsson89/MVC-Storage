namespace Storage.Models.ViewModels;

public class ProductSummaryViewModel
{
    public int TotalProductCount { get; set; }
    public int TotalInventoryValue { get; set; }

    public List<CategorySummaryViewModel> Categories { get; set; }
        = new();
}
