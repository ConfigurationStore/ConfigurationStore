using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

[Index(nameof(Name), IsUnique = true)]
public class UserGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [MaxLength(128)]
    public required string Name { get; set; }

    public ICollection<User> Users { get; set; } = new HashSet<User>();
    public ICollection<ProjectEnvironment> ProjectEnvironments { get; set; } = new HashSet<ProjectEnvironment>();
}