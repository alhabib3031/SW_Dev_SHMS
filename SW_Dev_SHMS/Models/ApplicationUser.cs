﻿using Microsoft.AspNetCore.Identity;

namespace SW_Dev_SHMS.Models;

public class ApplicationUser : IdentityUser
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}