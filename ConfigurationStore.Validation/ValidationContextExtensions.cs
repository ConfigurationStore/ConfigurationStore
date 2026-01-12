using System.ComponentModel.DataAnnotations;

namespace ConfigurationStore.Validation;

public static class ValidationContextExtensions
{
    extension(ValidationContext context)
    {
        public T GetService<T>()
            where T : class
        {
            T? service = context.TryGetService<T>();
            if (service is null)
            {
                throw new InvalidOperationException($"No service of type '{typeof(T).FullName}' is registered");
            }

            return service;
        }

        public T? TryGetService<T>()
            where T : class
            => context.GetService(typeof(T)) as T;
    }
}