ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-preview.4.21222.14

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='6662385b64eca5e4b7fff3041beaf36b05a15e42d74a3d65c955bd87b67355b0ba6b01ccc4ab3951ba8872dc69ff7bbda1fdfc6e5962b42cea5b4bfac17f90da' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-bullseye-slim-arm32v7
ARG ASPNET_VERSION

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=$ASPNET_VERSION \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
