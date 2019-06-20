ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19319.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='ac0fdb5ce49e7f92320e440265a1cf1a89d53c9d0de9fa43a62a92c00233933b465f90ef7548b3c0b3fa43ee6e8eb91d550c5497dfe45f91ab707bebdac6a612' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
