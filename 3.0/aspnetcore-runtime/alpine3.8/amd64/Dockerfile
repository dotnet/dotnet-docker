ARG REPO=microsoft/dotnet
FROM $REPO:3.0-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview-18579-0056

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='ff66488463dc34f6757239de584992df555c1f7ae3d12090acf2391b2b614b570dd38d383e8859126fdae2e85b816ce8c157c0613a584190530d37bd08ae08f1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
