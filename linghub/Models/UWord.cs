using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class UWord
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdWord { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual Word IdWordNavigation { get; set; } = null!;
}
