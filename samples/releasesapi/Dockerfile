FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:9.0-noble-aot AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link *.csproj NuGet.config .
RUN dotnet restore -r linux-$TARGETARCH

# Copy source code and publish app
COPY --link . .
RUN dotnet publish -r linux-$TARGETARCH --no-restore -o /app releasesapi.csproj
RUN rm /app/*.dbg /app/*.Development.json


# Final stage/image
FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:9.0-noble-chiseled-aot
WORKDIR /app
COPY --link --from=build /app .
USER $APP_UID
ENTRYPOINT ["./releasesapi"]
