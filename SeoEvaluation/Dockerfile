FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SeoEvaluation.sln"
RUN dotnet build "SeoEvaluation.sln" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/SeoEvaluation.Api
RUN dotnet publish "SeoEvaluation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeoEvaluation.Api.dll"]