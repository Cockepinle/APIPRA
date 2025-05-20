FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY APIPRA.sln .
COPY APIPRA/APIPRA.csproj APIPRA/

RUN dotnet restore APIPRA.sln

COPY . .

RUN dotnet build APIPRA/APIPRA.csproj -c Release -o /app/build
RUN dotnet publish APIPRA/APIPRA.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "APIPRA.dll"]
