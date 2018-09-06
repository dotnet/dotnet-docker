FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.5-rtm-30998

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='14ab2cc41afd4cb55689e4939988786a793db8d2f31215f3c3548172095299eb57bbe8b667a1c74c6301c8a676dc234de32efade0cbed7f7867fd27e9abd7685' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
