# Use this Dockerfile to create a Linux testrunner image
# Usage: docker run --rm -v /var/run/docker.sock:/var/run/docker.sock testrunner pwsh -File build-and-test.ps1 <params....>

FROM mcr.microsoft.com/dotnet-buildtools/prereqs:debian-stretch-docker-testrunner-63f2145-20184325094343

ENV RUNNING_TESTS_IN_CONTAINER=true
WORKDIR src
COPY . .
