ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.31-alpine3.16

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET Core Runtime
RUN dotnet_version=3.1.31 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='d81ffebcb8eda65bb3a1f60832d0f4f6eae746bfa3734a94915485f8e77af7bc0d33660173008cd3d8c6d527ae9f8d6b4dd64cde50b7576787c7ad25e5073419' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
