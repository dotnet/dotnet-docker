ARG REPO=microsoft/dotnet
FROM $REPO:2.1-runtime-deps-alpine3.7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.7

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='08320fd139768557fe962c06c0316df8d30312d35a354f5d4dd6dc98c5093bda7b0facd694bd7406f3c6543d383296f7cba7d759367ee4cfdbc8ac98062cd289' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apk del .build-deps
