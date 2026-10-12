# .NET Container Image Signatures

.NET container images are signed using [Notary Project](https://notaryproject.dev/) signatures.

## Inspect signature artifacts

Signatures are stored as [OCI referrer artifacts](https://github.com/opencontainers/distribution-spec/blob/main/spec.md#listing-referrers)
attached to the signed image in the registry. This structure is visible when
using the [ORAS CLI] to list referrer artifacts associated with an image:

```console
> oras discover mcr.microsoft.com/dotnet/sdk:10.0
mcr.microsoft.com/dotnet/sdk@sha256:<digest>
└── application/vnd.cncf.notary.signature
    └── sha256:<signature-digest>
        └── [annotations]
            ├── org.opencontainers.image.created: "2026-04-10T20:33:39.8412151Z"
            └── io.cncf.notary.x509chain.thumbprint#S256: '["84813e9c...","9b1894f2...","23ffe2b8..."]'
```

In this example, `mcr.microsoft.com/dotnet/sdk:10.0` has a single referrer
artifact with the `application/vnd.cncf.notary.signature` media type. The
[Notation CLI] reveals more detailed information about the attached signature
artifact:

```console
> notation inspect mcr.microsoft.com/dotnet/sdk@sha256:<digest>
mcr.microsoft.com/dotnet/sdk@sha256:<digest>
└── application/vnd.cncf.notary.signature
    └── sha256:<signature-digest>
        ├── media type: application/cose
        ├── signature algorithm: RSASSA-PSS-SHA-384
        ├── signed attributes
        │   ├── signingScheme: notary.x509
        │   └── signingTime: Fri Apr 10 13:31:25 2026
        ├── unsigned attributes
        │   └── timestamp signature
        │       └── timestamp: [Fri Apr 10 20:31:25 2026, Fri Apr 10 20:31:26 2026]
        ├── certificates
        │   ├── SHA256 fingerprint: 84813e9c82fe8abd08e0e6fa21b71b25b256d2c9fec38e8216e8bf48543008c9
        │   │   ├── issued to: CN=Microsoft SCD Products RSA Signing,O=Microsoft Corporation,L=Redmond,ST=Washington,C=US
        │   │   ├── issued by: CN=Microsoft SCD Products RSA CA,O=Microsoft Corporation,C=US
        │   │   └── expiry: Tue Nov 10 19:27:20 2026
        │   ├── SHA256 fingerprint: 9b1894f223d934cbd6575af3c6e1f6096b9221a7da132185f5a5cdc92235b5dc
        │   │   ├── issued to: CN=Microsoft SCD Products RSA CA,O=Microsoft Corporation,C=US
        │   │   ├── issued by: CN=Microsoft Supply Chain RSA Root CA 2022,O=Microsoft Corporation,C=US
        │   │   └── expiry: Mon Feb 17 00:55:23 2042
        │   └── SHA256 fingerprint: 23ffe2b8bdb9a1711515d4cffda04bc7f793d513c76c243f1020507d8669b7db
        │       ├── issued to: CN=Microsoft Supply Chain RSA Root CA 2022,O=Microsoft Corporation,C=US
        │       ├── issued by: CN=Microsoft Supply Chain RSA Root CA 2022,O=Microsoft Corporation,C=US
        │       └── expiry: Sun Feb 17 00:21:09 2047
        └── signed artifact
            ├── media type: application/vnd.docker.distribution.manifest.list.v2+json
            ├── digest: sha256:<digest>
            └── size: 1079
```

## Verify signatures using Notation CLI

### Prerequisites

- [Notation CLI]. This guide was written using version `1.3.2`.
- (Optional) [ORAS CLI] or [Docker CLI], for resolving image digests.

### Download the root CA certificates

.NET container image signatures use the following certificates:

- Signature Root Certificate: [Microsoft Supply Chain RSA Root CA 2022](https://www.microsoft.com/pkiops/certs/Microsoft%20Supply%20Chain%20RSA%20Root%20CA%202022.crt)
- Timestamping Authority (TSA) Root Certificate: [Microsoft Root Certificate Authority 2010](http://www.microsoft.com/pki/certs/MicRooCerAut_2010-06-23.crt)

### Set up Trust Store and Trust Policy

See the Notary Project's [Trust Store and Trust Policy Specification](https://github.com/notaryproject/specifications/blob/d4b62b949228911bd5e62da4f5f8dd38682d293a/specs/trust-store-trust-policy.md).

First, add both signatures listed above to a new `notation` trust store:

```console
notation certificate add --type ca --store msft-supplychain "Microsoft Supply Chain RSA Root CA 2022.crt"
notation certificate add --type tsa --store msft-supplychain "MicRooCerAut_2010-06-23.crt"
```

Next, set up a trust policy according to the [Trust Policy](https://github.com/notaryproject/specifications/blob/d4b62b949228911bd5e62da4f5f8dd38682d293a/specs/trust-store-trust-policy.md#trust-policy)
section of the Notation spec. Here is an **example** trust policy configuration
for verifying .NET images:

```json
{
  "version": "1.0",
  "trustPolicies": [
    {
      "name": "supplychain",
      "registryScopes": [
          "mcr.microsoft.com/dotnet/aspire-dashboard",
          "mcr.microsoft.com/dotnet/aspnet",
          "mcr.microsoft.com/dotnet/monitor",
          "mcr.microsoft.com/dotnet/monitor/base",
          "mcr.microsoft.com/dotnet/runtime",
          "mcr.microsoft.com/dotnet/runtime-deps",
          "mcr.microsoft.com/dotnet/sdk"
      ],
      "signatureVerification": { "level": "strict" },
      "trustStores": ["ca:msft-supplychain", "tsa:msft-supplychain"],
      "trustedIdentities": [
        "x509.subject: CN=Microsoft SCD Products RSA Signing,O=Microsoft Corporation,L=Redmond,S=Washington,C=US"
      ]
    }
  ]
}
```

> [!CAUTION]
> A misconfigured trust policy can cause signature verification to silently
> pass without logging errors for invalid or missing signatures. Read the trust
> policy specification carefully.

Import the trust policy configuration using `notation`:

```console
notation policy import trustpolicy.json
```

### Resolve multi-platform image digests

For multi-platform tags, it is important to verify signatures of both the
multi-platform and the platform-specific manifests. See [supported tags](supported-tags.md)
for more details on supported single-platform and multi-platform tags.

Use `oras manifest fetch $image` or `docker buildx imagetools inspect $image`
to determine whether an image is multi-platform or single-platform.
Multi-platform images have one of the following values for `mediaType`:

- `application/vnd.docker.distribution.manifest.list.v2+json`
- `application/vnd.oci.image.index.v1+json`

For multi-platform images, all of the platform-specific image digests will be
listed under the `manifests` property. Each multi-platform image and each of
its constituent single-platform images has its own unique signature artifact.

### Verify image signature

> [!IMPORTANT]
> Always use full image digests when verifying signatures, since image tags are
> mutable.

To verify the signature of an image digest, run:

```console
> notation verify mcr.microsoft.com/dotnet/sdk@sha256:<digest>
Successfully verified signature for mcr.microsoft.com/dotnet/sdk@sha256:<digest>
```

## Useful links

- [Notary Project Overview](https://notaryproject.dev/docs/notary-project-overview/)
- [Notary Project Specifications](https://github.com/notaryproject/specifications)

[Docker CLI]: https://docs.docker.com/reference/cli/docker/
[Notation CLI]: https://github.com/notaryproject/notation
[ORAS CLI]: https://oras.land/docs/installation
