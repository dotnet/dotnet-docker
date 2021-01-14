ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.12-arm32v7

# Install .NET
ENV DOTNET_VERSION=6.0.0-alpha.1.21052.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='c6520eb76b3cc59090c34791d30c746723be62500589b89a95449a946d918a196e36437430260b56401b47c1e266a379c936f71eeeacb6277a654f87b1fcb2d2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
