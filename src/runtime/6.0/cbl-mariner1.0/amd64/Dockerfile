ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rtm.21518.8

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5eff5cf25dc13734e7e3cdba8ea0e6eac505fb4e6f0edeb1e6c319bcdabd63e7b25d7107dd22362010289f904f95161c83f53431e1aa0e40dd30ea5cbd3f880a' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d1f69342015309af67b2af9f2a025b8f1242851471d307b8c40e6f9ca2f991c93aa7601132d0164b2ec2901ae5649e8cb447b7de431972a9b4927f4ccdb3529f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c6bae12e08b5a16b941e9fd9bd3c05b04ff36e389531e666d1a3d58e3b535193fe5d34b5cc489b6e783d38fb9831a6497f2d37801517bbd9a48fe8a73d3e09f5' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
