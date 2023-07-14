# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-preview-jammy AS build
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore -a $TARGETARCH

# copy and publish app and libraries
COPY . .
RUN dotnet publish -a $TARGETARCH -o /app releases.csproj


# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0-preview-jammy-chiseled
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./releases"]
