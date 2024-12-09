using System;
using System.Collections.Generic;

namespace DOTNETMVC.Models.Tables;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }
}
