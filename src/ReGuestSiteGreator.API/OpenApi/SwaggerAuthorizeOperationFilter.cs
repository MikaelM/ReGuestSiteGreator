using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReGuestSiteGreator.API.OpenApi;

public sealed class SwaggerAuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var allowsAnonymous = metadata.OfType<IAllowAnonymous>().Any();

        if (allowsAnonymous)
        {
            operation.Security = [];
            return;
        }

        var requiresAuthorization = metadata.OfType<IAuthorizeData>().Any();

        if (!requiresAuthorization)
        {
            return;
        }

        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
    }
}
