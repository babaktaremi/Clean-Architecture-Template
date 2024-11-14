using CleanArc.Application.Models.Common;
using CleanArc.SharedKernel.Extensions;
using CleanArc.WebFramework.EndpointFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CleanArc.WebFramework.WebExtensions;

public static class EndpointExtensions
{
    public static IResult ToEndpointResult<TModel>(this OperationResult<TModel> result)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(OperationResult<TModel>));

        if (result.IsSuccess) return result.Result is bool ?  Results.Ok() :Results.Ok(result.Result);

        if (result.IsNotFound)
            return result.ErrorMessages.Any()
                ? Results.NotFound(result.ErrorMessages.ToGroupedDictionary())
                : Results.NotFound();

        return string.IsNullOrEmpty(result.GetErrorMessage()) ? Results.BadRequest() : Results.BadRequest(result.ErrorMessages.ToGroupedDictionary());
    }


    public static RouteHandlerBuilder MapEndpoint(this IEndpointRouteBuilder app
        , Func<IEndpointRouteBuilder, RouteHandlerBuilder> handler
        , double apiVersion
        , string name
        , params string[] tags)
    {
       var versionedEndpoint= app.NewVersionedApi();

      return handler(versionedEndpoint)
           .WithOpenApi()
           .HasApiVersion(apiVersion)
           .AddEndpointFilter<OkResultEndpointFilter>()
           .AddEndpointFilter<NotFoundResultEndpointFilter>()
           .AddEndpointFilter<BadRequestResultEndpointFilter>()
           .AddEndpointFilter<ModelStateValidationEndpointFilter>()
           .WithName(name)
           .WithTags(tags);


    }
}