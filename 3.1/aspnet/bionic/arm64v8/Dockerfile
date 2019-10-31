ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview3.19530.9

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='71d4ed812a95b8807277d67dd241402747914faac35db17ec1f51bb053258c75523b3c5945fdbd72fcb6b2d4209857021388bb95d2e6e72c2844e46f87cc0f37' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
