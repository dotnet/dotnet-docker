ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.36-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.36

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://builds.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='cf4bc9725aa624863a1c7ac4582a669331753baf46e0376451fd4f089b7a76b7a7037e7162c95c109c125fe0c3a1b6106926500271679ea28460d923d771dd90' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://builds.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='553514bd8304b73458206b393e6dc8e69c859f635023770e21b7b3c56737860cec718a5fe6f8b57074670d5c042eb8d9bf34acd132aca64dfe9386d5d0f4ebd2' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://builds.dotnet.microsoft.com/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='204f99dd477d83e9b515414992e5f59a79070cd87698c23f6f2005c33a61243cd424896469e1ec135db34d623aadf36d94ffed64189e88ad2492bb44ef63519d' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
