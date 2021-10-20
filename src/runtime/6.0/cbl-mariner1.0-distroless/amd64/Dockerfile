ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Acquire .NET
RUN dotnet_version='6.0.0-rtm.21518.12' \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='7384dcd19df4c0d9cc9154c4f934e1cacd0a8ead72fa833b31ace2d074c90d8af83831ba5aa6e6865e963d300d5452b2abf511f7e0d508b4002f2975365de0e7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.0-cbl-mariner1.0-distroless-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21518.12

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
