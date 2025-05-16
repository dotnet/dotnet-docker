ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25265.103 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='fad39f709b6f7cb0e3678657c3af953c408342b29781ec21d20356f92c9d9345477c754e6a823c74ac1d403eac01991d8115b5c42cd57d160ca9dcdcdb6bc555' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-azurelinux3.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25265.103

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
