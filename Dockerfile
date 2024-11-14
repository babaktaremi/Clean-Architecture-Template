#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/API/CleanArc.Web.Api/CleanArc.Web.Api.csproj", "src/API/CleanArc.Web.Api/"]
COPY ["src/API/CleanArc.WebFramework/CleanArc.WebFramework.csproj", "src/API/CleanArc.WebFramework/"]
COPY ["src/API/Plugins/CleanArc.Web.Plugins.Grpc/CleanArc.Web.Plugins.Grpc.csproj", "src/API/Plugins/CleanArc.Web.Plugins.Grpc/"]
COPY ["src/Core/CleanArc.Application/CleanArc.Application.csproj", "src/Core/CleanArc.Application/"]
COPY ["src/Core/CleanArc.Domain/CleanArc.Domain.csproj", "src/Core/CleanArc.Domain/"]
COPY ["src/Infrastructure/CleanArc.Infrastructure.CrossCutting/CleanArc.Infrastructure.CrossCutting.csproj", "src/Infrastructure/CleanArc.Infrastructure.CrossCutting/"]
COPY ["src/Infrastructure/CleanArc.Infrastructure.Identity/CleanArc.Infrastructure.Identity.csproj", "src/Infrastructure/CleanArc.Infrastructure.Identity/"]
COPY ["src/Infrastructure/CleanArc.Infrastructure.Persistence/CleanArc.Infrastructure.Persistence.csproj", "src/Infrastructure/CleanArc.Infrastructure.Persistence/"]
COPY ["src/Infrastructure/CleanArc.Infrastructure.Monitoring/CleanArc.Infrastructure.Monitoring.csproj", "src/Infrastructure/CleanArc.Infrastructure.Monitoring/"]
COPY ["src/Shared/CleanArc.SharedKernel/CleanArc.SharedKernel.csproj", "src/Shared/CleanArc.SharedKernel/"]
COPY ["src/Tests/CleanArc.Test.Infrastructure.Identity/CleanArc.Test.Infrastructure.Identity/CleanArc.Test.Infrastructure.Identity.csproj", "src/Tests/CleanArc.Test.Infrastructure.Identity/CleanArc.Test.Infrastructure.Identity/"]
COPY ["src/Tests/CleanArc.Tests.Setup/CleanArc.Tests.Setup.csproj", "src/Tests/CleanArc.Tests.Setup/"]


RUN dotnet restore "src/API/CleanArc.Web.Api/CleanArc.Web.Api.csproj"
COPY . .
WORKDIR "src/API/CleanArc.Web.Api"
RUN dotnet build "CleanArc.Web.Api.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "CleanArc.Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArc.Web.Api.dll"]
