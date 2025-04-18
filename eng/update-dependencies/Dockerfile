FROM mcr.microsoft.com/dotnet/sdk:9.0-azurelinux3.0 AS build-env
WORKDIR /update-dependencies

# Build update-dependencies
COPY --link . .
RUN --mount=type=cache,target=/root/.nuget \
    --mount=type=cache,target=/source/bin \
    --mount=type=cache,target=/source/obj \
    dotnet publish eng/update-dependencies/update-dependencies.csproj -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0-azurelinux3.0

RUN tdnf install -y \
        ca-certificates \
        # Install Docker
        # Docker is needed to run ImageBuilder to regenerate Dockerfiles and Readmes
        moby-engine \
        docker-buildx \
        docker-cli \
        # Azure CLI is needed for BAR authentication
        # https://github.com/dotnet/arcade-services/pull/4700
        azure-cli \
    && tdnf clean all

# Copy update-dependencies app
WORKDIR /update-dependencies
COPY --link --from=build-env /update-dependencies/out ./

# Copy repo contents
WORKDIR /repo
COPY --link . ./

RUN ln -s /update-dependencies/update-dependencies /usr/local/bin/update-dependencies

ENTRYPOINT ["/update-dependencies/update-dependencies"]
