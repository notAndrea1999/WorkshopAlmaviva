using System;
using System.Collections.Generic;

namespace WorkshopApi.Models.Db;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Role { get; set; }
}
