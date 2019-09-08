FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS base
COPY . .
RUN dotnet publish --output /out --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=base /out .

ENTRYPOINT ["dotnet", "TCCC23.Publisher.dll"]