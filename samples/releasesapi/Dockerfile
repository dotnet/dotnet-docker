FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:8.0-jammy-aot AS build
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY *.config .
RUN dotnet restore -r linux-$TARGETARCH

# copy and publish app and libraries
COPY . .
RUN dotnet publish -r linux-$TARGETARCH --no-restore -o /app releasesapi.csproj
RUN rm /app/*.dbg /app/*.Development.json


# final stage/image
FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-jammy-chiseled-aot
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./releasesapi"]
