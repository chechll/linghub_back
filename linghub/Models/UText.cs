using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class UText
{
    public int IdText { get; set; }

    public int Id { get; set; }

    public int IdUser { get; set; }

    public virtual Text IdTextNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
