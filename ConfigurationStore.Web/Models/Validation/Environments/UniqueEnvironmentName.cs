using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Data;

using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Web.Models.Validation.Environments;

public class UniqueEnvironmentName : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? name = (value as string)?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(name))
        {
            return ValidationResult.Success; // [Required] will handle this
        }

        var model = (EditProjectEnvironmentDialogModel)validationContext.ObjectInstance;

        IDbContextFactory<MainDbContext> dbContextFactory = validationContext.GetService<IDbContextFactory<MainDbContext>>();
        using MainDbContext dbContext = dbContextFactory.CreateDbContext();
        return dbContext.ProjectEnvironments
           .Any(pe => pe.Name.ToLower() == name && pe.ProjectId == model.ProjectId) ? new ValidationResult($"Project {model.ProjectName} already has an environment with that name") : ValidationResult.Success;
    }
}