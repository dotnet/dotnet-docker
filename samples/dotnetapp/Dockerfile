# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS restore
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# dependencies vulnerability scan
FROM ollijanatuinen/trivy AS trivy
COPY --from=restore /source /source
RUN trivy fs \
    --exit-code 1 \
    --no-progress \
    --ignore-unfixed \
    --severity "HIGH,CRITICAL" \
    --security-checks vuln /source

# copy and publish app and libraries
FROM restore AS build
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
