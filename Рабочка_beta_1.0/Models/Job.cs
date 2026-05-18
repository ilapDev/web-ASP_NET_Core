using System;
using System.Collections.Generic;

namespace Рабочка_beta_1._0.Models;

public partial class Job
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public decimal Salary { get; set; }

    public string PaymentPeriod { get; set; } = null!;

    public decimal? PaymentFrom { get; set; }

    public decimal? PaymentTo { get; set; }

    public int CategoryId { get; set; }

    public int EmployerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ContactPhone { get; set; } = null!;

    public int Views { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User Employer { get; set; } = null!;
}
