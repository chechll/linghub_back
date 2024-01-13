using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class Text
{
    public string Text1 { get; set; } = null!;

    public string TextName { get; set; } = null!;

    public string Ans { get; set; } = null!;

    public string Ans1 { get; set; } = null!;

    public string Ans2 { get; set; } = null!;

    public string Ans3 { get; set; } = null!;

    public int IdText { get; set; }

    public string Question { get; set; } = null!;

    public int IdAns { get; set; }

    public virtual ICollection<UText> UTexts { get; set; } = new List<UText>();
}
