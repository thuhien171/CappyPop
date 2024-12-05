using System;
using System.Collections.Generic;

namespace DOTNETMVC.Models.Tables;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
