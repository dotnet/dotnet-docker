ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rtm.21521.6

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='366256fd817f3b3c6c86793ee47661dec4b8d511f57260897be841dc88a6537c13a54c5fee8e814d10e0a06b15aacb889084b41ac0bb0edcb10e5fbd9948ebc5' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='24f334d860922eb7eda908e334d27202ade98724d48b65869d75b91b8dc14fee68d64342f4b5f129ddb1674407fff9b48f05dee196f966bc8944208aab86be46' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b850755e17f33ec1bd61e1939ceca4d265e820b3e9918814f69e732dcdb8c3e8716fb977ceee6d4994d0583210813441d3de26cfcac69aa42eaa7bdc1dea7a07' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
