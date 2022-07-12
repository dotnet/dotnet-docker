ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.7-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.7

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='8820c57926034b9397afb5eb72df490dd6b3081e16c94e6740737367f424def5fae43f81181c77889861e470e662ecfacfde08c148ed8631f46e26d342dca20e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='167011d5f0a9db7259c9aca377657ee267624551b0beec19aa88b7ec1d2c1eafb3588882754e5a110ddfd90fb11a4edfc57270526c69c3c77eedd11360924c2a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='bd597fdcdfb551fae146273787c7a95c97e6caef15fbbe7985df5aaccf14893c41f264c98c2632370ca3270a3b730bf30cf67d13e6c719722b198ea88c146f65' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
