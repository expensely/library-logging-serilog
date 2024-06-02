# Publish Website to S3

This GitHub Action pulls an artifact that's a `.tar`, unpacks it, and publishes it to S3. Additionally, it checks for outdated packages in a .NET project.

## Inputs

| Name                                  | Description                                                                                                                     | Required | Default |
|---------------------------------------|---------------------------------------------------------------------------------------------------------------------------------|----------|---------|
| `ignore-failed-sources`               | Treat package source failures as warnings                                                                                       | false    | `false` |
| `include-auto-references`             | Specifies whether to include auto-referenced packages                                                                           | false    | `false` |
| `include-package-versions-older-than` | Include all dependencies in the report even the ones not outdated                                                               | false    |         |
| `include-up-to-date`                  | Only include package versions that are older than the specified number of days                                                  | false    |         |
| `pre-release-mode`                    | Specifies whether to look for pre-release versions of packages. Possible values are Auto, Always, or Never                      | false    |         |
| `include-transitive-dependencies`     | Specifies whether it should detect transitive dependencies                                                                      | false    |         |
| `restore-packages`                    | Add the reference with performing restore preview and compatibility check                                                       | false    | `false` |
| `recursive`                           | Recursively search for all projects within the provided directory                                                               | false    | `false` |
| `transitive-depth`                    | Defines how many levels deep transitive dependencies should be analyzed. Integer value (default = 1)                            | false    |         |
| `upgrade-packages`                    | Specifies whether outdated packages should be upgraded                                                                          | false    | `false` |
| `version-lock-mode`                   | Specifies whether the package should be locked to the current Major or Minor version. Possible values are None, Major, or Minor | false    | `None`  |

## Usage

```yaml
name: Publish website to S3
on:
  push:
    branches:
      - main

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v2

      - name: Set up .NET
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '5.0.x'

      - name: Pull artifact, unpack, and publish to S3
        uses: ./.github/actions/publish-website-to-s3
        with:
          ignore-failed-sources: 'true'
          include-auto-references: 'true'
          include-package-versions-older-than: '30'
          include-up-to-date: 'true'
          pre-release-mode: 'Always'
          include-transitive-dependencies: 'true'
          restore-packages: 'true'
          recursive: 'true'
          transitive-depth: '2'
          upgrade-packages: 'true'
          version-lock-mode: 'Major'
```
