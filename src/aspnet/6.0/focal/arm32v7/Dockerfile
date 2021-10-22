ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21521.10/aspnetcore-runtime-6.0.0-linux-arm.tar.gz \
    && aspnetcore_sha512='99fb5dcef86299f583736fa8e23242a8bf54ef3aa722d3d3ff2de4cf90d9ec5991619a4eb238cf79161b6536aa7dada947e64e701601a406a4d7a1081ff6bcd9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21521.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
