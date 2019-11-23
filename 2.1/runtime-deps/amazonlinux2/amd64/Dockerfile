FROM amazonlinux:2

RUN yum install -y \
    gzip \
    libicu \
    libssl-devel \
    tar \
    && rm -rf /var/cache/yum

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
