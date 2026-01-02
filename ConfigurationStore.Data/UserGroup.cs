using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigurationStore.Data;

public class UserGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(128)]
    public required string Name { get; set; }

    public ICollection<User> Users { get; set; } = new HashSet<User>();
}