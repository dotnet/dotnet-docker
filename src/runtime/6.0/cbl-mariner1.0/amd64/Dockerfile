ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.6.21324.10

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2c16b116045405ad75805aef5dc193e25a65d376971e25922804f3991efc2371f31320ba358ecc7e8fe9ad85ae0d7ae16ce5afb6e467cf25c05ea3498c6e5e88' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f6f2a3f90a9af7d3a8afe4a806f454966369702cf86577c8fe77deed6d80e994687990e9ed6b405e432589020f2e149dc06f024706ad9e7bd3bfb2846b67efc0' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='702c2f5d04d362ab630303ea21641075aa3ad8e2cbccac29b7f6953d58696d0c8d3307e270ebce40841bd9e88fa02f70806e39bec05bc0b058660b5cb9e7b04c' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
