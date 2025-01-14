ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.1-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.1 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.1


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='46deba46b4458a3546da4db8118798d8baf5a230f1691c9a9745771a80f48a50c93a0ea56d0348fdbbdd9952eb397f8e1153274e0aeccb8e1d91f68f700f30cf' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
