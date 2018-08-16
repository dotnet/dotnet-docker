FROM microsoft/dotnet-nightly:2.1-runtime-deps-buster-slim-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.2.0-preview1-34992

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='7ca598d2d46bdcc0027664d4f9344dfda7f7f3c2efc251cd1688c1bc831fd32bee2343f316edffcd9a0e451f156cb2d5ae7408d0632ef2f1b993b2ac34c85748' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
