using System;
using System.Collections.Generic;

namespace AppWithDatabase.Models;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;
}
