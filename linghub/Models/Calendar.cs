using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class Calendar
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public DateTime Datum { get; set; }

    public virtual User IdNavigation { get; set; } = null!;
}
