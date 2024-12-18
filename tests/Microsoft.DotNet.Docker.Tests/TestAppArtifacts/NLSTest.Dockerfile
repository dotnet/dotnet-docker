ARG sdk_image
ARG runtime_image

FROM ${sdk_image} AS sdk
RUN dotnet new console -n App -o /src --no-restore
WORKDIR /src
COPY NLSTest.cs /src/Program.cs
RUN dotnet restore
RUN dotnet publish --no-restore -o /app

FROM ${runtime_image} AS runtime
ARG icu_expected
ENV ICU_EXPECTED=${icu_expected}
COPY --from=sdk /app /app/
ENTRYPOINT ["/app/App"]
