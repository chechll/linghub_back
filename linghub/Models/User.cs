using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int Admin { get; set; }

    public string UserPassword { get; set; } = null!;

    public virtual Calendar? Calendar { get; set; }

    public virtual ICollection<UText> UTexts { get; set; } = new List<UText>();

    public virtual ICollection<UWord> UWords { get; set; } = new List<UWord>();
}
