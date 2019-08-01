ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview8.19381.6

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='ecd329764ec368e1dcdd5bf5410f22c84443b67fd2539dfde1f79ad335ac1a3d6ec616c71b1b76dbf8d3266e62fa28b2443a0c535e640dfcae4404848cc60343' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
