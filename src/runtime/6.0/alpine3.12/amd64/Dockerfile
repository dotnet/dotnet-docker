ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-alpine3.12-amd64

# Install .NET
ENV DOTNET_VERSION 6.0.0-alpha.1.21057.2

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='9d9db77c17f2e85bbd48b7c36a01ac976f6b6f18213d58ef436559f91b8a03c2025b3345987ef8c9be1be4fd265320666ee8a47b3955d54afa6909eafec672b4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
