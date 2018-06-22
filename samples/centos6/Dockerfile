FROM centos:6

RUN yum -y update && \
    yum clean all && \
    yum -y install epel-release && \
    yum -y install openssl libidn libuuid lttng-ust zlib libunwind libnghttp2

# Install .NET Core
ENV DOTNET_VERSION 2.1.0-preview3-26319-04

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-rhel.6-x64.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Set the invariant mode since icu_libs isn't included (see https://github.com/dotnet/announcements/issues/20)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT true

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80
