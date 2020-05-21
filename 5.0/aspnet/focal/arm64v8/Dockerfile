ARG REPO=mcr.microsoft.com/dotnet/core/runtime
ARG ASPNET_VERSION=5.0.0-preview.5.20269.6

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='40aec433c497e7cf8b4ae0742f74f8b0ec0a565a1c913eb79d68cf70a524ada4526b853a8595cd0cd625ca709877d224ff4383eb63b9d7f3978711bb3d79fea0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-focal-arm64v8
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
