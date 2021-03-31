# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY complexapp/*.csproj complexapp/
COPY libfoo/*.csproj libfoo/
COPY libbar/*.csproj libbar/
RUN dotnet restore complexapp/complexapp.csproj

# copy and build app and libraries
COPY complexapp/ complexapp/
COPY libfoo/ libfoo/
COPY libbar/ libbar/
WORKDIR /source/complexapp
RUN dotnet build -c release --no-restore

# test stage for .NET 5.0 -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test
WORKDIR /source/tests
COPY tests/ .
ENTRYPOINT ["dotnet", "test", "-f", "net5.0", "--logger:trx"]

# test stage for .NET Core 3.1 -- exposes optional entrypoint
# target entrypoint with: docker build --target test31
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS test31
WORKDIR /source
COPY --from=build /source .
COPY tests/ .
WORKDIR /source/tests
COPY tests/ .
ENTRYPOINT ["dotnet", "test", "-f", "netcoreapp3.1", "--logger:trx"]

FROM build AS publish
RUN dotnet publish -c release -f net5.0 --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "complexapp.dll"]
