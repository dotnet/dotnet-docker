ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.4-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.4

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d58b30bdacc0489e904a2652f4d17344bfd45197940b45e1a43583ae4ade6307d04c4d06bfd6982c663187ec2ba87d365d55caa006d199ff4851e445bc00c360' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f6d4c4b1e7c98d0589944dc3356f15b6f5636a32a3279ae7d57f5beb1f2e16677a6d2ba32cd5b8e04d3b6463b1adc7398c52cfaf247e8276cd1cdc135522d50c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b58569bce85a15e8d101e1e1844a8a65951fa7cf7ae4fea082149a7428778b2462001ce58424e70c350a4863703a02488ec6550b4021aa19107be133f0e11179' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
