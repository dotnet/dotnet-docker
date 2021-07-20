ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-preview.7-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21369.8

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='afeb18635f5dcedbed78b503f39206994f6319f9f35982b364a59d835959af5fc1fb06c614cc1a33e365f7e09cf282a90e08c65101187b7f7fcfdd72401add3f' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='cfaa8df309bf9ee446718b7d262760ff3afa8bb04a7f3b38d40fa7eed930926e3c0cd7c10ee9141e1a87a6510ef3d097e5336cc35b484f2787eacd661cd8e60c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c3178d327d4e5c69bb86370dd6e250bf52ee41e3df5acd30d7c3ae1ec2bb518c7e235df769cf969beb0732614e72900fedad58d074527bdad2d48131954af92b' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
