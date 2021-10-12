ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21480.5/dotnet-runtime-6.0.0-rc.2.21480.5-linux-x64.tar.gz \
    && dotnet_sha512='45062417c6111af4d635868927e8f69d43f66c9e0f111cb71c1861eaf5ceda4aefa99d97c6ce3b13fac2bc7c57c435e6f8b2d43c51a3bb3304b42081d98f7047' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.2.21480.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
