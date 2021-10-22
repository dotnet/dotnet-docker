ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21521.6/dotnet-runtime-6.0.0-linux-arm64.tar.gz \
    && dotnet_sha512='3991a1dff156488ee19fdafae573f8130ad3f78bcf0e434eb5bb8bcd7e890258c27996c9f1e756df4726d437470984f59bc6e80f0f8069f81ae122fdd32225ea' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21521.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
