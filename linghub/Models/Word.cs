using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class Word
{
    public int IdWord { get; set; }

    public string Enword { get; set; } = null!;

    public string Ruword { get; set; } = null!;

    public string Ensent { get; set; } = null!;

    public string Rusent { get; set; } = null!;

    public string Ans1 { get; set; } = null!;

    public string Ans2 { get; set; } = null!;

    public string Ans3 { get; set; } = null!;

    public virtual ICollection<UWord> UWords { get; set; } = new List<UWord>();
}
