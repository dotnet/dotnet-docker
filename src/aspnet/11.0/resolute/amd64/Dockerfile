ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:resolute-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=11.0.0-alpha.1.25609.108 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz.sha512)  aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:11.0.0-alpha.1-resolute-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=11.0.0-alpha.1.25609.108

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
