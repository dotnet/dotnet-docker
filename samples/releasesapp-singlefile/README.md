# .NET Release Report App

This app demonstrates publishing an app with [Single file](https://learn.microsoft.com/dotnet/core/deploying/single-file/) in containers.

It demonstrates key aspects:

- Container images to use
- Project properties to use
- Configuring JSON (de)serialization for trimming

## Usage

You can build and run the sample:

```bash
docker build --pull -t app .
docker run --rm -it -p 8000:8080 app
```
