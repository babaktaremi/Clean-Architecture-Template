using System.ComponentModel.DataAnnotations;

namespace CleanArc.Web.Api.ApiModels.User;

public record RefreshTokenViewModel([Required(ErrorMessage = "Please Enter Refresh Token")]Guid RefreshToken);