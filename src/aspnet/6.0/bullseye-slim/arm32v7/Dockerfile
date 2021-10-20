ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21518.20/aspnetcore-runtime-6.0.0-rtm.21518.20-linux-arm.tar.gz \
    && aspnetcore_sha512='693c4c71fbc9e840885f03637ce920fa99f94c94eec744fa9ca78316c8530c81160a1a9d8a129ba8912d42b426121e94374fe4a4ba8c52b4d6a1070fd341d4e9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-bullseye-slim-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21518.20 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
