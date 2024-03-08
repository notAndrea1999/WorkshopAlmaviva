using System;
using System.Collections.Generic;

namespace WorkshopApi.Models.Db;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public bool Availability { get; set; }
}
