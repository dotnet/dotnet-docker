ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Acquire .NET
RUN dotnet_version='6.0.0-rtm.21518.8' \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='d0123a76598c28e6d88294c598921c78c11aad678e41f9891cbed3c301f4bc15801757d8c90183a99f8f14c6514078254f91c0d3654b23edb094b02a9acff56c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.0-cbl-mariner1.0-distroless-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21518.8

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
