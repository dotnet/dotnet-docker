# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test

COPY tests/*.csproj tests/
WORKDIR /source/tests
RUN dotnet restore

COPY tests/ .
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-build"]


FROM build AS publish
WORKDIR /source/complexapp
RUN dotnet publish --no-restore -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "complexapp.dll"]
