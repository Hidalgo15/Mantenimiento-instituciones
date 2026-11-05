using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MantenimientoPresentation.Filters
{
    public sealed class FluentValidationActionFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationActionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                    continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                if (_serviceProvider.GetService(validatorType) is not IValidator validator)
                    continue;

                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

                foreach (var error in result.Errors)
                    context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(entry => entry.Value?.Errors.Count > 0)
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
                {
                    success = false,
                    message = errors.Values.SelectMany(messages => messages).FirstOrDefault()
                              ?? "Los datos enviados no son válidos.",
                    errors
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return;
            }

            await next();
        }
    }
}
