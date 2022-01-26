ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-preview.2.22074.8/aspnetcore-runtime-7.0.0-preview.2.22074.8-linux-arm.tar.gz \
    && aspnetcore_sha512='f6ac23daf4906ab935a24fd547721194de8f366f40ccc1a7382b83d48f395182c22fec4183f47661535ec9d09f8acf3dfac37486b9af42eb66d88b83cda1c660' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.2-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-preview.2.22074.8 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
