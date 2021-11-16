ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=7.0.0-alpha.1.21551.1

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='783e2e451ec3f1a032d6ace57f3ed571f45dadaded7e5c936f3569e09cc0bda1c01077aec163908931962f1adf606d82c938c50391dec464fedd6157ca2e01d7' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a5332f58105adbf9b30fca6c6f7091b81e05c7e38d20a24397227a72c24f5b6e9486d6b3ecf25d95803ce485f4b224be12d57500072f43b07e2d482fc2573fd0' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d515781f489b0ed818e2ab196899307080972502525a44cfab805b4209f5d62ea18975f8c9de16697aef4225879f5ea945e1c18d4e05065ad32e364bdf214abc' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
