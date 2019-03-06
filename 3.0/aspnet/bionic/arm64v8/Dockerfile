ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview3-19153-02

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='0f45de305aa1f640eedb8645fdb031ad90e22b5e6cddc4b4458ff6e080005255d5fc3de02c15015b3beee756601975b4daec2dfdc3774942f07ec8ecf36883bd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
