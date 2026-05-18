using System;
using System.Collections.Generic;

namespace Рабочка_beta_1._0.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; } = new List<Job>();
}
