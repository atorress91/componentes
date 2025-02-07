﻿using System;
using System.Collections.Generic;

namespace Componentes.Data.Database.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
