# Running Tests with Docker

You can use Docker to run your unit tests in a controlled environment using the [.NET SDK Docker image](../README.sdk.md).
Running tests in a container has several benefits:

- Tests are more insulated from dev and build machine configuration.
- Tests can run in an environment that more closely matches production - this is especially useful if your development and production environments don't match.
- Any additional setup needed for running tests can be configured in a Dockerfile.

[Building in an SDK container](./build-in-sdk-container.md) is a similar scenario and relies on similar patterns.

The following is just one example of how to test .NET apps with Docker and isn't intended to be comprehensive.
There are multiple ways to run unit tests in containers, most of which which aren't .NET-specific.
A good approach to automated testing is to make sure that tests run in an environment similar to production, make sure that failing tests fail the build/test/deployment pipeline that they run in, and make sure to collect detailed test output/logs regardless of test success or failure.

This example uses the [tests](./complexapp/tests) that are part of [complexapp](./complexapp).
The instructions assume that you have cloned this repo and are in the [complexapp](./complexapp) directory.

## Running tests in an executable stage

The [complexapp Dockerfile](./complexapp/Dockerfile) includes a `test` stage that demonstrates running via its `ENTRYPOINT`, as follows:

```Dockerfile
# test exposes tests as the default executable for the stage
FROM test-build AS test
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx"]
```

The `test` stage can be built directly using the `--target` argument:

```pwsh
# Build the test image
PS> docker build --pull --target test -t complexapp-tests .
 => [internal] load build definition from Dockerfile
 ...
 => => naming to docker.io/library/complexapp-tests:latest
 => => unpacking to docker.io/library/complexapp-tests:latest
```

Then, the `complexapp-tests` image can be run with the test results output directory mounted into the running container:

```pwsh
# Create output directory for test results
PS> mkdir TestResults


# Run the test image, mounting the TestResults directory into the container
PS> docker run --rm -v ${pwd}/TestResults:/source/tests/TestResults complexapp-tests
Test run for /source/tests/bin/Debug/net9.0/tests.dll (.NETCoreApp,Version=v9.0)
VSTest version 17.12.0-preview-24412-03 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
Results File: /source/tests/TestResults/_51029443fea7_2024-09-27_16_25_14.trx

Passed!  - Failed:     0, Passed:     3, Skipped:     0, Total:     3, Duration: 9 ms - tests.dll (net9.0)


# View the test results on the host machine
PS> ls TestResults

    Directory: C:\s\dotnet-docker\samples\complexapp\TestResults

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---           9/27/2024  9:25 AM           4962 _51029443fea7_2024-09-27_16_25_14.trx
```

## More Samples

- [.NET Docker Samples](../README.md)
- [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
