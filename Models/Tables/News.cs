using System;
using System.Collections.Generic;

namespace DOTNETMVC.Models.Tables;

public partial class News
{
    public int Id { get; set; }

    public string? Thumb { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Content { get; set; }

    public int CountViews { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Source { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
