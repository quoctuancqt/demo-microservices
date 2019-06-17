FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Demo.OAuthServer.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
RUN dotnet publish -c Release -o ./out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /src/out/ .
ENTRYPOINT ["dotnet", "Demo.OAuthServer.dll"]