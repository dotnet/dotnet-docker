ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.12-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.12 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.12


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='44b22ee55213537adfeaed8c94bf295769ac96d3d2bc244cb20811cf4aa7c215f6c2e31856a2c100c4fb1eb0f9080f07c57fcea7e985232d32c0520e7c02449a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
