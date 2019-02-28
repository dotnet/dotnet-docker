ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-stretch-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview-19075-0444

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='4e47098a9512c79985954a7fff1751c178ec10ab3c89f1b7ebd1e838eecb2b64b750503d72d72366f99e54ed000d5085d1e08114bd7c4f0f4f732c418601ef14' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
