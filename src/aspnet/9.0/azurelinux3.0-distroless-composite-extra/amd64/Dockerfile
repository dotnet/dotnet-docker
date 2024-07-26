ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM azurelinuxpreview.azurecr.io/public/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-preview.7.24373.9  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='7f8ac80353cc7149e6e47ffdc8e88d0206517820f8a5037c84a05e26da63c3b7c2a829efe8f36f84f677587468abe4df66691002903f949bdeafae3b8c299e5c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-preview.7-azurelinux3.0-distroless-extra-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.7.24374.12 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.7.24373.9

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
