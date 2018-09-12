FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.5-rtm-31008

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='4fd114c60444aef15ed0b06a54489eaa865128e3277f64532f925dd063439f648cf803adc3a6e6b25c23ff4a58477f89340fea69a4e26ba9d941841ac2332237' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
