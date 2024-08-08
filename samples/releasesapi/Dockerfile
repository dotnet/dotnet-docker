FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:8.0-jammy-aot AS build
ARG TARGETARCH
WORKDIR /source

# Copy all source assets
COPY . .
# Cache mounts caches runtime packs and NuGet packages across builds
RUN --mount=type=cache,target=/root/.nuget \
    --mount=type=cache,target=/source/bin \
    --mount=type=cache,target=/source/obj \
    dotnet publish -a $TARGETARCH -o /app releasesapi.csproj


# final stage/image
FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-jammy-chiseled-aot
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./releasesapi"]
