using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Data;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Validation.Projects;

public class UniqueProjectName : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? name = (value as string)?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(name))
        {
            return ValidationResult.Success; // [Required] will handle this
        }

        IDbContextFactory<MainDbContext> dbContextFactory = validationContext.GetService<IDbContextFactory<MainDbContext>>();
        using MainDbContext dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Projects
           .Any(p => p.Name.ToLower() == name) ? new ValidationResult("Project name already exists") : ValidationResult.Success;
    }
}