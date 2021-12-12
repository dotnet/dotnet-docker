# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS restore
WORKDIR /source

# copy csproj and restore as distinct layers
COPY complexapp/*.csproj complexapp/
COPY libfoo/*.csproj libfoo/
COPY libbar/*.csproj libbar/
RUN dotnet restore complexapp/complexapp.csproj

# dependencies vulnerability scan
FROM ollijanatuinen/trivy AS trivy
COPY --from=restore /source /source
RUN trivy fs \
    --exit-code 1 \
    --no-progress \
    --ignore-unfixed \
    --severity "HIGH,CRITICAL" \
    --security-checks vuln /source

# copy and build app and libraries
FROM restore AS build
COPY complexapp/ complexapp/
COPY libfoo/ libfoo/
COPY libbar/ libbar/
WORKDIR /source/complexapp
RUN dotnet build -c release --no-restore

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test
WORKDIR /source/tests
COPY tests/ .
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "complexapp.dll"]
