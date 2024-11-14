using CleanArc.Application.Models.ApiResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArc.WebFramework.Attributes;

/// <summary>
/// Documents the 200 OK response type of endpoint as ApiResult
/// </summary>
/// <typeparam name="TResponse">API response type that will be in data JSON property</typeparam>
public class ProducesOkApiResponseType<TResponse>:ProducesResponseTypeAttribute
{
    private ProducesOkApiResponseType(int statusCode) : base(statusCode)
    {
    }

    public ProducesOkApiResponseType() : base(typeof(ApiResult<TResponse>), StatusCodes.Status200OK)
    {
    }

    private ProducesOkApiResponseType(Type type, int statusCode, string contentType, params string[] additionalContentTypes) : base(type, statusCode, contentType, additionalContentTypes)
    {
    }
}

public class ProducesOkApiResponseType:ProducesResponseTypeAttribute
{
    private ProducesOkApiResponseType(int statusCode) : base(statusCode)
    {
    }

    public ProducesOkApiResponseType() : base(typeof(ApiResult), StatusCodes.Status200OK)
    {
    }

    private ProducesOkApiResponseType(Type type, int statusCode, string contentType, params string[] additionalContentTypes) : base(type, statusCode, contentType, additionalContentTypes)
    {
    }
}