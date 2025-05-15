ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.4-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25258.107 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='e805b2ab603d5ecb116ffb598302712e761f1cbd92f81ca11cfe10458c35abdcb889391d585e2be65934eb35e6cb598a25e9fdda6eaeadd34655702154425f84' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.4-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25258.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
