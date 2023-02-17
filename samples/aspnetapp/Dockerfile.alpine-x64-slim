# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY aspnetapp/*.csproj .
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true

# copy everything else and build app
COPY aspnetapp/. .
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# To enable globalization:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine-amd64
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./aspnetapp"]
