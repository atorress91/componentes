﻿using System;
using System.Collections.Generic;

namespace Componentes.Data.Database.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
