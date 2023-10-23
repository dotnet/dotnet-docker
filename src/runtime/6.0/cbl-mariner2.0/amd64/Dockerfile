ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.24-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.24

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3ad33b54ed208cd8e0c8bff8bf2fbdb4aabb2170900539702fb5bfdf005f1bd84e41a2831144a7d6d9dd6adb7dd36fec685fb9470f39479fe42e1691b0e0e736' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a33463440dba12f2cbfa5d742191853459aba57e3fc0fbdb5174c4e00fe7732de349497d5dbc1744e75735158b69bd13b940b742727f893da19e50ef2b000a77' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1c6559ef8501aaac81519b0b178d06ef27120ad9b38fb931b76b612adef2a296908dc4ef2d4cee3bd97fdc93941a73355a8aa5961bf835f5981e4e59e4756c3d' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
