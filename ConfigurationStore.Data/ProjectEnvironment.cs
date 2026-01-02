using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigurationStore.Data;

public class ProjectEnvironment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public required int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    [MaxLength(128)]
    public required string Name { get; set; }

    public ICollection<User> Users { get; set; } = new HashSet<User>();
    public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
}