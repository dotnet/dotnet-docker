# How to onboard a new repo

> [!CAUTION] This guide is for .NET team members. If you are not a .NET team
> member and you think we should add a new .NET image repo, please [file an
> issue](https://github.com/dotnet/dotnet-docker/issues/new) detailing your
> request and we will consider it!

## Checklist

1. - [ ] Read [CONTRIBUTING.md](https://github.com/dotnet/dotnet-docker/blob/main/CONTRIBUTING.md)
1. - [ ] Decide on important characteristics of your container image offering proposal
        - Support policy:
            - Will you support only a single version or multiple versions at once?
            - When a new version is released, how long are old releases supported for?
            - Images must be published in the `nightly` branch before they can be added to the
              `main` branch where they will be officially supported. Decide on what the bar is for
              moving the new image repo from `nightly` to `main`.
            - Decide how your team will handle the responsibility of responding to issues and
              discussions about your images filed on this repo.
        - Release process and cadence:
            - Consider how your product's release cycle interacts with .NET's release cycle.
            - How does you product handle security releases?
        - Supported OSes:
            - If your product is not intended to be extended or used as a base
              image for users to build on, you may not need to publish
              containers for more than one operating system.
            - Azure Linux support is expected for Microsoft products.
        - Distroless, non-distroless, or both:
        - Repo name:
        - Desired tags:
            - See [supported tags policy](https://github.com/dotnet/dotnet-docker/blob/nightly/documentation/supported-tags.md) for existing images.
1. - [ ] Document the above information in an [issue](https://github.com/dotnet/dotnet-docker/issues/new) proposing the new image repo.
1. - [ ] [Create a prototype](#creating-a-prototype-image) and test out
         your image. Gather feedback on the issue and make adjustments if necessary.
1. - [ ] Once you have sign-off on proceeding with the implementation, then [create the new repo](#creating-a-new-repo).

## Creating a prototype image

This repo does not build any product source code. This is for a number of
reasons:

- Keeps container build times short
- Keeps product build issues from blocking this repo's build and release
  process

With that in mind, you will need:

- A binary archive of the product you want to ship in the container.
  - On Linux, this should be a `.tar.gz` file.
  - For Windows, it may be a `.zip` file.
- The sha512 checksum of that binary.

> [!TIP]
> If you would like to gate the release of your product on container image
> functionality, you should set up automation for pushing daily builds of your
> product to this repo's `nightly` branch. That way, issues in the image build
> can be surfaced every time there is a new build of the product.

When creating the new Dockerfile, follow the [Dockerfile instructions best practices](https://docs.docker.com/build/building/best-practices/#dockerfile-instructions).
Most images in this repo follow a simple pattern:

1. Install dependencies
1. Download an archive of the shipping product
1. Validate its integrity with a checksum
1. Install the product

You can reference the various Dockerfiles in this repo for inspiration.

## Creating a new repo

For general instructions for adding new images to this repo, follow the
[New Image Release](/.github/ISSUE_TEMPLATE/releases/new-image-release.md) checklist.

1. - [ ] Create an issue for adding the new repo to the `nightly` branch.
         It should contain the contents of this checklist as well as the contents of the
         [New Image Release](/.github/ISSUE_TEMPLATE/releases/new-image-release.md) checklist.
1. - [ ] Create a PR to add new repo and images to the `nightly` branch.
         Follow the [New Image Release](/.github/ISSUE_TEMPLATE/releases/new-image-release.md)
         checklist. The PR must include:
    1. - [ ] Unit tests.
    1. - [ ] Scenario tests.
    1. - [ ] README content. This content will show up on
        GitHub ([example](https://github.com/dotnet/dotnet-docker/blob/main/README.monitor.md)),
        MAR, ([example](https://mcr.microsoft.com/artifact/mar/dotnet/monitor/about)), and
        DockerHub ([example](https://hub.docker.com/r/microsoft/dotnet-monitor)).
        It should contain:
            - [ ] An "About" section explaining what the image is and why it's useful.
            - [ ] A "Usage" section explaining the very basics for getting started with the image.
            - [ ] Links to official documentation and examples.
            - [ ] Links to official support policy.
1. - [ ] Create a PR to add the new repo to the [MCR](https://github.com/microsoft/mcr) repo.
    Example: [#3017](https://github.com/microsoft/mcr/pull/3017)
1. - [ ] Merge the PR to the nightly branch
1. - [ ] Create an issue for moving the new repo from the `nightly` to `main` branch according to
         the guidelines that were decided on above.
