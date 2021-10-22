ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Acquire .NET
RUN dotnet_version='6.0.0-rtm.21521.6' \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-6.0.0-linux-x64.tar.gz \
    && dotnet_sha512='d29877eb91e997cd6e1582a74818ed0f21bd3bb81e0350bcc6dca769ea5f1d0c9d5a675618b6773ee6710c42fa050606b8e058af5f1da648f20e84822d746f6f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.0-cbl-mariner1.0-distroless-amd64

ENV DOTNET_VERSION=6.0.0

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
