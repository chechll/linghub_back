using System;
using System.Collections.Generic;

namespace linghub.Models;

public partial class User
{
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int Admin { get; set; }

    public string? UserPassword { get; set; }

    public int IdUser { get; set; }

    public byte[]? Photo { get; set; }

    public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();

    public virtual ICollection<UText> UTexts { get; set; } = new List<UText>();

    public virtual ICollection<UWord> UWords { get; set; } = new List<UWord>();
}
