ARG REPO=microsoft/dotnet
FROM $REPO:3.0-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview-19067-0383

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='a72cf20ca58fbfb37e556cf09e36a71dc56503bd45c749d33278faa363784faf74a91bb7f6dd9dc70b0c3c73e83e5cfab0b8674ecf16e179b90ca744ebcc7969' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
