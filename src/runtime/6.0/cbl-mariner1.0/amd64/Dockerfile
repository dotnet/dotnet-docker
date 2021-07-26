ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21371.11

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='51bafde9cc5db0e7523b98d21574be86445fc16bc4b22cfc8ea624aa0690b589fda5e7a8f8a396688724cc2b3097d6c91ef3b73d6223a960e24a74b3ffa78acf' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='fb8243c342c77be0f6e85b1c49eed618252fb18e3903d48258bd07676932a826ca5a36ce0521f8fd0d68365966d9ef57c80c590c6f03aa45057579428e8c673d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='84668aa9513d5693d9db46cb2c1093ecddb5455ffda5bf5c15f43e8b410790cab371dd4052bfbaa4300b2dc077ea001b8b269067d455f148f28dd42220e19a02' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
