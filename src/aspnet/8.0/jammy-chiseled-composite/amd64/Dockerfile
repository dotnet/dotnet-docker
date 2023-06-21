ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.0-preview.5-jammy-chiseled-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.5.23280.8 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-preview.5.23302.2


# Install ASP.NET Composite Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

