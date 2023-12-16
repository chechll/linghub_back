using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class Word
{
    public int IdWord { get; set; }

    public string Enword { get; set; } = null!;

    public string Uaword { get; set; } = null!;

    public string Ensent { get; set; } = null!;

    public string Uasent { get; set; } = null!;

    public virtual ICollection<UWord> UWords { get; set; } = new List<UWord>();
}
