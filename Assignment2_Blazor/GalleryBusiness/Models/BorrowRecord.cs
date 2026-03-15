using System;
using System.Collections.Generic;

namespace GalleryBusiness.Models;

public partial class BorrowRecord
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public bool? Returned { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
