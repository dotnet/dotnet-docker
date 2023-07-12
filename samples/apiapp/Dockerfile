# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview-alpine AS build
RUN apk add build-base clang zlib-dev

WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
RUN dotnet publish -o /app
RUN rm /app/*.dbg /app/*.Development.json


# final stage/image
FROM alpine
ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080

WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./apiapp"]
