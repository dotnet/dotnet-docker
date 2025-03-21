# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:9.0-azurelinux3.0 AS build
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link *.sln .
COPY --link OtlpTestListener/*.csproj ./OtlpTestListener/
RUN dotnet restore

# Copy source code and publish app
COPY --link OtlpTestListener/. ./OtlpTestListener/
WORKDIR /source/OtlpTestListener
RUN dotnet publish -c release --no-restore -o /app


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-azurelinux3.0-distroless
EXPOSE 8080
EXPOSE 4317
WORKDIR /app
COPY --link --from=build /app .
USER $APP_UID
ENTRYPOINT ["./OtlpTestListener"]
