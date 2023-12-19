using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class Calendar
{
    public int IdUser { get; set; }

    public DateTime Datum { get; set; }

    public int Id { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
