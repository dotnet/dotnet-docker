ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25120.3 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='015564ad968b0916f122aaebcc71bd6800ac480179cb7423dca45450b9e61d271207a86be5a7cc0c1628b1e7fa4906133819d47beaa180ca75c04101afcee9b8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25120.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
