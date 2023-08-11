ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.21-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.21

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2bbf45f5f677a03c238775d6708c12b08bcda4136cb3853ad8de71305fad6cbe02bcd16f2ab71f1c6622cca2fbf3a4022e528f75ddccf6edf03e74141a631884' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='6ffbcf05cadce6f7e9053ce5b923da59ba1741a1102423e949f449466c97ce78f9fffad29f2272335ba1dab8272c4c3231e12cc377355781199cf062239c6b7a' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='04cb03a24a409819b424dfc4f81748622370787acbacb5b349e6ad3c3628bfa83dc115780476ad4edead676f1b4fe0d510a87f38e4c66e1ff98ea8dd665367ff' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
