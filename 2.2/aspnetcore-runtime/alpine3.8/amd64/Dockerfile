FROM microsoft/dotnet-nightly:2.2-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.2.0-preview3-35374

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='0f30b5b4039824cbf68ec80672d1347568acb21c713e096526ac840fd21a284c0b6d4f7326dc78d6bc963ebbbb43d4d0792c9a06250377924f7f3c9d64e99cef' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
