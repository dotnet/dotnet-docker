ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21519.6/aspnetcore-runtime-6.0.0-rtm.21519.6-linux-arm64.tar.gz \
    && aspnetcore_sha512='5877d29f4900717741fdd3bf1c6a693ac671f20b5cd28e53f80532b5520b2c4171fe796db0bee14d09d09e7ba266323888b33343286e6b75e8661c6cbf6bfa2c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-bullseye-slim-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21519.6 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
