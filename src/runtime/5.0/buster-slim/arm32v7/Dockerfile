ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.8/dotnet-runtime-5.0.8-linux-arm.tar.gz \
    && dotnet_sha512='32eb73beb20e1bd4fa7eb35be779fa962df68921da14f9100a033911c40afc6c61ac99256b0d54cb35547f630bd7c0c7658b841224c9798e621bb7326ba0213e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.8-buster-slim-arm32v7

ENV DOTNET_VERSION=5.0.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
