ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21518.20/aspnetcore-runtime-6.0.0-rtm.21518.20-linux-arm64.tar.gz \
    && aspnetcore_sha512='5f95e0463d25afdd8661006c5612a8f424b6f32d2839f4ec8167c8fcabbd49b9ac6ce76302c05b5db1878c1553d771ccdd699d8b75ac96a193ef674fb28f771e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21518.20 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
