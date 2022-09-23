ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-rc.2.22464.1

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='3aa3205b04bb3def247c7f1200edc4bd9a0bf9b54096e2bce20d00ce88a4b65a43735bb032db30e88ea6b52d43fd325614194be4ff2b1eb61b24c64a95aee1da' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0b30f795a2471a8628ce8deeba495dbfe9c89515ef1b5123a2ec51203d8493e1be791efc96821e70db0bf1c655cbcc7f8f8a4199edee2f9e498b73921661dc48' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='a7e212926513fdb8b207168bc3b5a47e4d2affab4605fdbb328a77845262dc98404423c09f723b9d542025ab653fa01ca023369f5b8d8c81eb9539a8c157b90e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
