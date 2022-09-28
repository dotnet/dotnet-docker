# build image
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env

WORKDIR /update-dependencies

# Need to publish with version-specific RID. See https://github.com/libgit2/libgit2sharp/issues/1798
ARG RID=alpine.3.14-x64

# copy csproj and restore as distinct layers
COPY eng/update-dependencies/*.csproj ./
COPY NuGet.config ./
RUN dotnet restore -r $RID

# copy everything else and build
COPY eng/update-dependencies/. ./

RUN dotnet publish -c Release -o out --no-restore


# runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine

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
