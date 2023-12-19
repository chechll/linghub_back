using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class UWord
{
    public int IdUser { get; set; }

    public int IdWord { get; set; }

    public int Id { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual Word IdWordNavigation { get; set; } = null!;
}
