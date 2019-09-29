FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS base
COPY . .
RUN dotnet publish --output /out --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=base /out .

ENTRYPOINT ["dotnet", "Publisher.dll"]