# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# build copies all project files and restores NuGet packages
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link Directory.Build.props .
COPY --link complexapp/*.csproj complexapp/
COPY --link libfoo/*.csproj libfoo/
COPY --link libbar/*.csproj libbar/
RUN dotnet restore -a $TARGETARCH complexapp/complexapp.csproj

# Copy source code and publish app
COPY --link complexapp/ complexapp/
COPY --link libfoo/ libfoo/
COPY --link libbar/ libbar/


# test-build builds the xUnit test project
FROM build AS test-build

COPY --link tests/*.csproj tests/
WORKDIR /source/tests
RUN dotnet restore

COPY --link tests/ .
RUN dotnet build --no-restore


# test-entrypoint exposes tests as the default executable for the stage
FROM test-build AS test
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx"]


# publish builds and publishes complexapp
FROM build AS publish
WORKDIR /source/complexapp
RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# final is the final runtime stage for running the app
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --link --from=publish /app .
ENTRYPOINT ["dotnet", "complexapp.dll"]
