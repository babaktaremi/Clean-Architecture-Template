using CleanArc.Application.Models.Jwt;

namespace CleanArc.Application.Features.Admin.Queries.GetToken;

public record AdminGetTokenQueryResult(AccessToken Token,string[] Roles);