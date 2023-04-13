ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.4.23207.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='2e7cf66661f43981bd5a70747fe30d2d27af2baa186d69a0eafeca0ea4032d68576195fd4bce8dcc0b9cb7c8cec2395b674c7b2bd8ed94567c763865b09ab667' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.4-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.4.23207.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
