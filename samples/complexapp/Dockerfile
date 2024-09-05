# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY complexapp/*.csproj complexapp/
COPY libfoo/*.csproj libfoo/
COPY libbar/*.csproj libbar/
RUN dotnet restore -a $TARGETARCH complexapp/complexapp.csproj

# Copy source code and publish app
COPY complexapp/ complexapp/
COPY libfoo/ libfoo/
COPY libbar/ libbar/

# Test stage -- exposes optional entrypoint
# Target entrypoint with: docker build --target test
FROM build AS test

COPY tests/*.csproj tests/
WORKDIR /source/tests
RUN dotnet restore

COPY tests/ .
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-build"]


FROM build AS publish
WORKDIR /source/complexapp
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/nightly/runtime:9.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "complexapp.dll"]
