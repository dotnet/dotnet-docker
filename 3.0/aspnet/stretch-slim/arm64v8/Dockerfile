ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-stretch-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview3-19122-14

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='17fbdfa9b36c1d58fdf8533bf5ba46dc0853060d778eb924d97bc7ebdfa2d1861de7cc83fdca968fdca64fe0f38f40414e220a1fbb64769ff6f5ae868706688a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
