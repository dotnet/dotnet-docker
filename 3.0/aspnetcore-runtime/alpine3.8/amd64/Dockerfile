ARG REPO=microsoft/dotnet
FROM $REPO:3.0-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview-19074-0437

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='65a09a7316e0a63474c517d0030ce5f0c202f46993491903aec2c80c0ffa9ae750bb200ea416632563de288fc93647fc665aab2f996745589d110a6600b1ca46' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
