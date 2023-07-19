# Release Json Report App

This app demonstrates publishing an app as [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) in containers.

It demonstrates key aspects:

- Container images to use
- Packages to install
- Project properties to use
- Configuring JSON (de)serialization for trimming and native AOT

## Usage

You can build and run the sample:

```bash
docker build --pull -t app .
docker run --rm -it -p 8000:8080 app
```

It exposes to endpoints:

- `http://localhost:8000/releases`
- `http://localhost:8000/healthz`
