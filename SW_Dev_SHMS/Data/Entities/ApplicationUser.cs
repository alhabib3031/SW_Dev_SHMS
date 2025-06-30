using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SW_Dev_SHMS.Models.Entities;

public class ApplicationUser:IdentityUser
{
    /// <summary>
    /// First name of the user (Manager or Student).
    /// Required field with maximum length of 100 characters.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the user (Manager or Student).
    /// Required field with maximum length of 100 characters.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date when the user account was created.
    /// Automatically set during user registration.
    /// </summary>
    [Display(Name = "Date Created")]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Collection of notifications associated with this user.
    /// Navigation property - configured in DbContext.
    /// Can contain notifications for both Managers and Students.
    /// </summary>
    public virtual ICollection<Notification>? Notifications { get; set; }

}