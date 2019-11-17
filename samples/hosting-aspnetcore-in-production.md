## Deploying to Production vs Development

The approach for running containers differs between development and production.

In production, you will typically start your container with `docker run -d`. This argument starts the container as a service, without any console interaction. You then interact with it through other Docker commands or APIs exposed by the containerized application.

In development, you will typically start containers with `docker run --rm -it`. These arguments enable you to see a console (important when there are errors), terminate the container with `CTRL-C` and cleans up all container resources when the container is terminated. You also typically don't mind blocking the console. This approach is demonstrated in prior examples in this document.

We recommend that you do not use `--rm` in production. It cleans up container resources, preventing you from collecting logs that may have been captured in a container that has either stopped or crashed.