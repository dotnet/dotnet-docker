ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.1.20124.5 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='4ec2c9e8ed5e8ddaa52ed291c263412b754691ae2b95362ddc6fa75cfe087dbe59c7e4f65f21b0f096bc93967d04f4ea612a0c42c8a124ef4583d0de8daac01c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-focal

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
