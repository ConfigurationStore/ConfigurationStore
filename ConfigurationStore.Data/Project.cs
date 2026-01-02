using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigurationStore.Data;

public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    [MaxLength(128)]
    [Required]
    public required string Name { get; set; }

    public ICollection<ProjectEnvironment> Environments { get; set; } = new HashSet<ProjectEnvironment>();
}