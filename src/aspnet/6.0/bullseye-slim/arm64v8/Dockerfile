ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21512.5/aspnetcore-runtime-6.0.0-rtm.21512.5-linux-arm64.tar.gz \
    && aspnetcore_sha512='803f9696e94748fd8671f204eefbff5f7257800ac57b00c0c61acd01d2cde7b8f296ef6f352236539175707de55509aa705d41c1bf2d0dad3ef09330921fdf51' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-bullseye-slim-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21512.5 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
