
using System.ComponentModel.DataAnnotations;

namespace Storage.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0, 10000)]
    public int Price { get; set; }

    [DataType(DataType.Date)]
    public DateTime Orderdate { get; set; }
    public string Category { get; set; }
    public string Shelf { get; set; }

    [Range(0, 1000)]
    public int Count { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
}
