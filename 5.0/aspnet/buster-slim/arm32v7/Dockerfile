ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.3.20207.4 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='2a5724d23eada1db26597c6c2bba5473e511ca970fc8a561000058f1bb329ac77f8f4d84c7aa0af1b07ed775a269be1ba123a22a649a157cef4143c10e282b6e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim-arm32v7

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
