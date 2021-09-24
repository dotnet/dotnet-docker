ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/alpine:latest AS installer

# Acquire .NET
RUN dotnet_version='6.0.0-rtm.21514.7' \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='053c74c3b24b416981863a6967bcde71bf15300572b13e5aafb7839e39f47bf641807d1c684387ec47b602d9a238d072fd08e329802f43baa6dc651f74fb3226' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.0-cbl-mariner1.0-distroless-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21514.7

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
