ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:2.1.29-alpine3.13

# Install .NET Core
ENV DOTNET_VERSION=2.1.29

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='bda42e525afd693e0a417c8b5f4bf6e7e172158e8d515df85aa86c83e7d35404cde31306dc7a5585aa1246fe40b8286fbdac98e624deede54e2949a89c437794' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
