FROM microsoft/dotnet-nightly:3.0-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-alpha1-10663

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='aa296d10f4b1f1042eff8949c3c2f325fff46d1cd34dc1c06d1d56cf966a569db73266e3bd9baaebaf594951f2eeea00d2a05711ce85f98a5877c5aa63f3c5a1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
