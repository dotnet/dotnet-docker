# build image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
ARG TARGETARCH
# The rid must be version-specific to workaround a libgit2sharp issue (see https://github.com/dotnet/dotnet-docker/pull/2111)
ARG RID=linux-musl-$TARGETARCH
WORKDIR /update-dependencies

# copy csproj and restore as distinct layers
COPY eng/update-dependencies/*.csproj ./
COPY NuGet.config ./

# Set UseRidGraph=true to enable non-portable RIDs: https://aka.ms/netsdk1083
RUN dotnet restore -r $RID /p:UseRidGraph=true

# copy everything else and build
COPY eng/update-dependencies/. ./

# Set UseRidGraph=true to enable non-portable RIDs: https://aka.ms/netsdk1083
RUN dotnet publish -r $RID /p:UseRidGraph=true -c Release -o out --no-restore


# runtime image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine

# install Docker
RUN apk add --no-cache \
        docker

# copy update-dependencies
WORKDIR /update-dependencies
COPY --from=build-env /update-dependencies/out ./

# copy repo
WORKDIR /repo
COPY . ./

RUN ln -s /update-dependencies/update-dependencies /usr/local/bin/update-dependencies

ENTRYPOINT ["/update-dependencies/update-dependencies"]
