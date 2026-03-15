using System;
using System.Collections.Generic;

namespace GalleryBusiness.Models;

public partial class Session
{
    public string SessionId { get; set; } = null!;

    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public virtual User User { get; set; } = null!;
}
