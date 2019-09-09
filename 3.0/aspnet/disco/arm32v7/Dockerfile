ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-rc1.19457.4

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='426b9a0496cf21d2b27d3aee91a3f518f3a1080223c91af11eb88bd59ef503257280b4942756a8f491dc64ac57cdde72ee2961e542e2faa09b06feebc9d7f080' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
