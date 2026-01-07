# Developer Guide: Using the docker-tools Infrastructure

This guide walks you through the practical scenarios and workflows for using the docker-tools infrastructure. The `eng/docker-tools` directory is a **shared infrastructure layer** used across all .NET Docker repositories (dotnet-docker, dotnet-buildtools-prereqs-docker, dotnet-framework-docker). It solves a fundamental challenge: building, testing, and publishing Docker images across multiple operating systems (Alpine, Ubuntu, Azure Linux, Windows Server variants), multiple CPU architectures (amd64, arm64, arm32), and multiple .NET versions—all while maintaining consistency and reliability.

At its core, the infrastructure provides:

- **PowerShell scripts** for local image building and Docker operations—so you can test Dockerfile changes on your machine before committing
- **Azure Pipelines templates** for CI/CD (build, test, publish)—a composable template system that orchestrates builds across dozens of OS/architecture combinations in parallel
- **ImageBuilder orchestration**—a specialized .NET tool that understands manifest files, manages image dependencies, handles multi-arch manifest creation, and coordinates the entire build process
- **Caching and optimization**—intelligent systems that skip unchanged images and minimize redundant work
- **SBOM generation**—automatic Software Bill of Materials creation for supply chain security

The infrastructure handles complexity that would otherwise be overwhelming: a single commit to a repo can trigger builds of hundreds of image variants across Linux and Windows agents, each requiring proper build sequencing, testing, and eventual publication to Microsoft Artifact Registry (MAR).

**Important:** Files in `eng/docker-tools/` are synchronized across repositories by automation in the [dotnet/docker-tools](https://github.com/dotnet/docker-tools) repository. If you need to make changes to this infrastructure, submit them there—changes made directly in consuming repos will be overwritten.

---

## Local Development Scenarios

### Scenario: Building Docker Images Locally

The most common local task is building images to test Dockerfile changes before pushing.

**Quick Build - All Images:**
```powershell
./eng/docker-tools/build.ps1
```

**Filter by OS:**
```powershell
# Build only Alpine images
./eng/docker-tools/build.ps1 -OS "alpine"

# Build Ubuntu 24.04 images
./eng/docker-tools/build.ps1 -OS "noble"
```

**Filter by Architecture:**
```powershell
# Build arm64 images only
./eng/docker-tools/build.ps1 -Architecture "arm64"
```

**Filter by Path:**
```powershell
# Build images from a specific directory
./eng/docker-tools/build.ps1 -Paths "src/runtime/8.0/alpine3.20"

# Build all 8.0 runtime images using glob pattern
./eng/docker-tools/build.ps1 -Paths "*runtime*8.0*"
```

**Combine Filters:**
```powershell
# Build .NET 8.0 Alpine arm64 images
./eng/docker-tools/build.ps1 -Version "8.0" -OS "alpine" -Architecture "arm64"
```

**Filter by Product Version (if applicable):**
```powershell
# Build only .NET 8.0 images
./eng/docker-tools/build.ps1 -Version "8.0"

# Build .NET 6.0 and 8.0 images
./eng/docker-tools/build.ps1 -Version "6.0","8.0"
```

### Understanding What Happens Under the Hood

When you run [`build.ps1`](build.ps1), here's the chain of execution:

```
build.ps1
    │
    ├── Translates your filter parameters into ImageBuilder CLI args
    │
    └── Calls Invoke-ImageBuilder.ps1 "build --version X --os-version Y ..."
            │
            ├── On Linux: Runs ImageBuilder in a Docker container
            │   └── Builds image: microsoft-dotnet-imagebuilder-withrepo
            │   └── Mounts Docker socket and repo contents
            │
            └── On Windows: Extracts ImageBuilder locally (due to Docker-in-Docker limitations)
                └── Runs Microsoft.DotNet.ImageBuilder.exe directly
```

### Scenario: Running ImageBuilder Directly

For advanced scenarios, you may want to invoke ImageBuilder with specific commands:

```powershell
# Run any ImageBuilder command
./eng/docker-tools/Invoke-ImageBuilder.ps1 "build --help"

# Generate the build matrix (useful for debugging pipeline behavior)
./eng/docker-tools/Invoke-ImageBuilder.ps1 "generateBuildMatrix --manifest manifest.json --type platformDependencyGraph"

# Validate manifest syntax
./eng/docker-tools/Invoke-ImageBuilder.ps1 "validateManifest --manifest manifest.json"
```

---

## Understanding the Pipeline Architecture

### The Build Flow

The pipeline behaves differently depending on the build context:

**Public PR Builds**:
```
Build Stage
  ├── PreBuildValidation
  ├── GenerateBuildMatrix
  └── Build Jobs (dry-run, no push)
        └── Inline tests after each build
              │
              ▼
          Post_Build Stage
            └── Merge artifacts
                    │
                    ▼
              Publish Stage (dry-run)
                └── All publish operations run but skip actual pushes
                        │
                        ▼
                      (end)
```
- Images are built but **not pushed** to any registry
- Tests run inline within each build job
- Publish stage runs in dry-run mode (validates publish logic without pushing)
- Validates that Dockerfiles build successfully

**Internal Official Builds**:
```
Build Stage
  ├── PreBuildValidation
  ├── CopyBaseImages → staging ACR
  ├── GenerateBuildMatrix
  └── Build Jobs (push to staging ACR)
              │
              ▼
          Post_Build Stage
            ├── Merge image info files
            └── Consolidate SBOMs
                    │
                    ▼
              Test Stage
                ├── GenerateTestMatrix
                └── Test Jobs
                        │
                        ▼
                  Publish Stage
                    ├── Copy images to production ACR
                    ├── Create multi-arch manifests
                    ├── Wait for MAR ingestion
                    ├── Update READMEs
                    ├── Publish image info to versions repo
                    └── Apply EOL annotations
```
- Full pipeline with all stages
- Images flow: `BuildRegistry` → `PublishRegistry` → MAR (see [`publish-config-prod.yml`](templates/stages/dotnet/publish-config-prod.yml) for ACR definitions)
- Tests run against staged images
- Only successful builds get published

### Build Matrix Generation

The `generateBuildMatrix` command is key to understanding how builds are parallelized. It:

1. **Reads the manifest.json** - Understands which images exist
2. **Builds a dependency graph** - Knows that `runtime-deps` must build before `runtime`
3. **Groups by platform** - Creates jobs for each OS/Architecture combo
4. **Optimizes with caching** - Can detect and exclude unchanged images

### Controlling Which Build Stages Run

The `stages` variable is a comma-separated string that controls which pipeline stages execute:

```yaml
variables:
- name: stages
  value: "build,test,publish"    # Run all stages
```

Common patterns:
- `"build"` - Build only, no tests or publishing
- `"build,test"` - Build and test, but don't publish
- `"publish"` - Publish only (when re-running a failed publish from a previous build)
- `"build,test,publish"` - Full pipeline

**Note:** The `Post_Build` stage is implicitly included whenever `build` is in the stages list. You don't need to specify it separately—it automatically runs after Build to merge image info files and consolidate SBOMs.

The stages variable is useful for:
- Re-running just the publish stage after fixing a transient failure
- Skipping tests during initial development
- Running isolated stages for debugging

### Image Info Files: The Build's Memory

Image info files (defined by [`ImageArtifactDetails`](https://github.com/dotnet/docker-tools/blob/main/src/ImageBuilder/Models/Image/ImageArtifactDetails.cs)) are the mechanism that tracks what was built:

```json
{
  "repos": [{
    "repo": "dotnet/runtime",
    "images": [{
      "platforms": [{
        "dockerfile": "src/runtime/8.0/alpine3.20/amd64/Dockerfile",
        "digest": "sha256:abc123...",
        "created": "2024-01-15T10:30:00Z",
        "commitUrl": "https://github.com/dotnet/dotnet-docker/commit/..."
      }]
    }]
  }]
}
```

**How they flow through the pipeline:**
1. **Build stage**: Each build job produces an image-info fragment
2. **Post_Build stage**: Fragments are merged into a single `image-info.json`
3. **Test stage**: Uses merged info to know which images to test
4. **Publish stage**: Uses info to know which images to copy/publish
5. **Versions repo**: Final info is committed to the versions repo

The [versions repo](https://github.com/dotnet/versions) stores the "source of truth" image info. Future builds compare against this to determine what's changed and skip unchanged images.

**Using Image Info for Investigations**

Image info files are invaluable when you need to track down information about a specific image, particularly when starting from a digest reported by a customer or security scan.

*Scenario: "Which commit produced this image?"*

Given a digest like `sha256:abc123...`, you can trace it back to its source:

1. **Check the versions repo history** - The `dotnet/versions` repo contains historical image info committed after each publish. Use `git log -p --all -S 'sha256:abc123'` to find the commit that introduced this digest.

2. **From the image info entry**, you'll find:
   - `commitUrl` - The exact source commit that built this image
   - `dockerfile` - Which Dockerfile produced it
   - `created` - When it was built
   - `simpleTags` - The tags applied to this image

*Scenario: "What was in the last successful build?"*

Download the `image-info` artifact from a pipeline run in Azure DevOps:
1. Navigate to the pipeline run
2. Go to the "Published" artifacts section
3. Download `image-info` (merged) or individual `*-image-info-*` fragments

*Scenario: "When did we last publish updates to a specific image?"*

Use the versions repo git history:
```bash
# In the dotnet/versions repo
git log --oneline -- build-info/docker/image-info.dotnet-dotnet-docker-main.json
```

Each commit corresponds to a publish operation and includes the full image info at that point in time.

*Scenario: "Compare what changed between two publishes"*

```bash
git diff <commit1> <commit2> -- build-info/docker/image-info.dotnet-dotnet-docker-main.json
```

This shows which images were added, removed, or rebuilt (new digests) between the two publishes.

### The Publish Flow in Detail

The publish stage does more than just push images. Here's the sequence:

1. **Copy Images** — `copyAcrImages` copies from build ACR to publish ACR
2. **Publish Manifest** — `publishManifest` creates multi-arch manifest lists
3. **Wait for MAR Ingestion** — Polls MAR until images are available (timeout configurable)
4. **Publish READMEs** — Updates documentation in the registry
5. **Wait for Doc Ingestion** — Ensures README changes are live
6. **Merge & Publish Image Info** — Updates the versions repo with new image metadata
7. **Ingest Kusto Image Info** — Sends telemetry to Kusto for analytics
8. **Generate & Apply EOL Annotations** — Marks images with end-of-life dates
9. **Post Publish Notification** — Creates GitHub issues/notifications about the publish

### Dry-Run Mode

For testing pipeline changes without actually publishing:

```yaml
# In pipeline variables or at runtime
variables:
- name: dryRunArg
  value: "--dry-run"
```

Or the infrastructure automatically enables dry-run for:
- Pull request builds
- Builds from non-official branches
- Public project builds

The [`set-dry-run.yml`](templates/steps/set-dry-run.yml) step template determines this automatically based on context.

---

## Automatic Image Rebuilds

The infrastructure includes automation that monitors for base image updates and triggers rebuilds when dependencies change.

### How It Works

A scheduled pipeline ([`check-base-image-updates.yml`](https://github.com/dotnet/docker-tools/blob/main/eng/pipelines/check-base-image-updates.yml)) runs every 4 hours and:

1. **Checks for stale images** — Compares the base image digests used in our published images against the current digests in upstream registries
2. **Identifies affected images** — Determines which of our images need rebuilding because their base image changed
3. **Queues targeted builds** — Automatically triggers builds for only the affected images, not the entire repo

This ensures that security patches and updates in base images (like `alpine`, `ubuntu`, `mcr.microsoft.com/windows/nanoserver`) flow through to images without manual intervention.

### Failure Handling and Recovery

The system has built-in retry logic but requires manual intervention after repeated failures:

**Automatic retry behavior:**
- If a triggered build fails, the system will attempt to rebuild every 4 hours
- After **3 unsuccessful attempts**, the system stops queuing new builds for that image
- This prevents endless rebuild loops when there's a genuine issue requiring human attention

**After fixing the issue:**

Once you've fixed the underlying problem (Dockerfile change, test fix, etc.) and have a successful build:

1. Navigate to the successful pipeline run in Azure DevOps
2. Add the `autobuilder` label to that run
3. This signals to the infrastructure that a successful build has occurred
4. The system will resume automatic rebuilds for that image as needed

The `autobuilder` label is how the infrastructure tracks that the failure cycle has been broken and normal operations can resume.

---

## Common Customization Patterns

### Pattern: Adding Build Arguments

Pass additional arguments to Docker builds via ImageBuilder:

```yaml
customBuildInitSteps:
- powershell: |
    $args = "--build-arg MY_VAR=value"
    echo "##vso[task.setvariable variable=imageBuilderBuildArgs]$args"
```

### Pattern: Re-running Stages with `stages` and `sourceBuildPipelineRunId`

A powerful pattern is combining the `stages` variable with the `sourceBuildPipelineRunId` pipeline parameter to run specific stages using artifacts from a previous build. This is useful for:
1. Skipping stages you don't need to run
2. Avoiding unnecessary re-builds after test/publishing infrastructure fixes

Note: For simple retries of failed jobs, use the Azure Pipelines UI "Re-run failed jobs" feature instead.

**Scenario: Test failed, need to run publish anyway**

* Set `sourceBuildPipelineRunId` to the build which built the images
* Set `stages` to `publish`

**How it works:**

1. `sourceBuildPipelineRunId` tells the pipeline which previous run to pull artifacts from
2. The [`download-build-artifact.yml`](templates/steps/download-build-artifact.yml) step uses this ID to fetch `image-info.json` from that run
3. Specified stage(s) use the downloaded image info to know which images exist

**Common recovery patterns:**

| Scenario | stages | sourceBuildPipelineRunId |
|----------|--------|--------------------------|
| Normal full build | `"build,test,publish"` | `$(Build.BuildId)` (default) |
| Re-run publish after infra fix | `"publish"` | ID of the successful build run |
| Re-test after infra fix | `"test"` | ID of the build run to test |
| Build only (no publish) | `"build"` | `$(Build.BuildId)` (default) |
| Test + publish (skip build) | `"test,publish"` | ID of the build run |

**In the Azure DevOps UI:**

When you queue a new run, you can override these as runtime parameters:
1. Set `stages` to the stage(s) you want to run
2. Set `sourceBuildPipelineRunId` to the run ID containing the artifacts you need (find the build ID in the URL when viewing a pipeline run, e.g., `buildId=123456`)

This avoids the multi-hour rebuild cycle when you just need to retry a failed operation.
