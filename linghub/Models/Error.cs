using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class Error
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Description { get; set; } = null!;
}
