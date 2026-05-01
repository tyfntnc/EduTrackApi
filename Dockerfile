FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["EduTrackApi.Api/EduTrackApi.Api.csproj", "EduTrackApi.Api/"]
COPY ["EduTrackApi.Application/EduTrackApi.Application.csproj", "EduTrackApi.Application/"]
COPY ["EduTrackApi.Domain/EduTrackApi.Domain.csproj", "EduTrackApi.Domain/"]
COPY ["EduTrackApi.Infrastructure/EduTrackApi.Infrastructure.csproj", "EduTrackApi.Infrastructure/"]
RUN dotnet restore "EduTrackApi.Api/EduTrackApi.Api.csproj"
COPY . .
WORKDIR "/src/EduTrackApi.Api"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EduTrackApi.Api.dll"]
