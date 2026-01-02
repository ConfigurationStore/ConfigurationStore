using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [MaxLength(64)]
    public required string Username { get; set; }

    [MaxLength(128)]
    public required string DisplayName { get; set; }

    [MaxLength(256)]
    public required string PasswordHash { get; set; }

    public ICollection<UserGroup> Groups { get; set; } = new HashSet<UserGroup>();
    public ICollection<ProjectEnvironment> ProjectEnvironments { get; set; } = new HashSet<ProjectEnvironment>();
}