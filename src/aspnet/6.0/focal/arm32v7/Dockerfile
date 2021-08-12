ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.1.21411.15/aspnetcore-runtime-6.0.0-rc.1.21411.15-linux-arm.tar.gz \
    && aspnetcore_sha512='a210099e5a1bc26a1ff617e96a25a1f721819ed7b08249060ddebf2fc8fe25c71989e55d1065beda7e3d77a63857121bb1c6ecfd60e7a8dd80b724a5a4dc9744' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.1-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21411.15 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
