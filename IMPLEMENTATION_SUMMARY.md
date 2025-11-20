# FileFormats Package Split - Implementation Summary

## Overview
This document describes the implementation of issue #511: splitting the KustoLoco.FileFormats library into separate packages.

## Problem Statement
The original `KustoLoco.FileFormats` package included support for all file formats (Parquet, CSV, Excel, JSON) with all their dependencies bundled together. This meant users had to download dependencies for all formats even if they only needed one or two.

## Solution
Split the monolithic package into focused, format-specific packages with a metapackage for backward compatibility.

## New Package Structure

### Core Package
- **KustoLoco.FileFormats.Abstractions** (new)
  - Contains shared interfaces: `ITableSerializer`
  - Contains shared types: `TableLoadResult`, `TableSaveResult`
  - Zero format-specific dependencies
  - Referenced by all format-specific packages

### Format-Specific Packages (all new)
- **KustoLoco.FileFormats.Parquet**
  - Dependencies: Parquet.Net
  - Contains: `ParquetSerializer`

- **KustoLoco.FileFormats.Csv**
  - Dependencies: CsvHelper
  - Contains: `CsvSerializer`

- **KustoLoco.FileFormats.Excel**
  - Dependencies: ClosedXML, ExcelDataReader.DataSet
  - Contains: `ExcelSerializer`, `DataTableReader`

- **KustoLoco.FileFormats.Json**
  - Dependencies: System.Text.Json (built-in)
  - Contains: `JsonObjectArraySerializer`, `JsonLSerializer`

### Metapackage
- **KustoLoco.FileFormats** (updated)
  - References all format-specific packages
  - Maintains backward compatibility
  - Contains: `KustoResultSerializer` (uses Parquet internally)

## File Moves
```
Before:
libraries/FileFormats/
  ├── ITableSerializer.cs
  ├── TableLoadResult.cs
  ├── TableSaveResult.cs
  ├── ParquetSerializer.cs
  ├── CsvSerializer.cs
  ├── ExcelSerializer.cs
  ├── DataTableReader.cs
  ├── JsonObjectArraySerializer.cs
  └── KustoResultSerializer.cs

After:
libraries/FileFormats.Abstractions/
  ├── ITableSerializer.cs
  ├── TableLoadResult.cs
  └── TableSaveResult.cs

libraries/FileFormats.Parquet/
  └── ParquetSerializer.cs

libraries/FileFormats.Csv/
  └── CsvSerializer.cs

libraries/FileFormats.Excel/
  ├── ExcelSerializer.cs
  └── DataTableReader.cs

libraries/FileFormats.Json/
  └── JsonObjectArraySerializer.cs

libraries/FileFormats/
  └── KustoResultSerializer.cs
```

## CI/CD Changes

### Build Script (build.ps1)
Updated the `$packages` array to include all new packages:
```powershell
$packages = @(
    ("KustoLoco.Core", "KustoLoco.Core"), 
    ("FileFormats.Abstractions", "KustoLoco.FileFormats.Abstractions"),
    ("FileFormats.Parquet", "KustoLoco.FileFormats.Parquet"),
    ("FileFormats.Csv", "KustoLoco.FileFormats.Csv"),
    ("FileFormats.Excel", "KustoLoco.FileFormats.Excel"),
    ("FileFormats.Json", "KustoLoco.FileFormats.Json"),
    ("FileFormats", "KustoLoco.FileFormats"),
    ...
)
```

### GitHub Actions Workflow
Created `.github/workflows/publish-nuget.yml` with:
- Triggered on releases or manual workflow dispatch
- Builds and tests all packages
- Creates NuGet packages with version from release tag or input
- Publishes to NuGet.org when NUGET_API_KEY secret is configured
- Uploads artifacts for manual verification

## Testing
All existing tests pass without modification:
- SerializationTests: 10/10 passed
- BasicTests: 317/320 passed (3 skipped - unrelated)
- CoreTests: 233/238 passed (5 skipped - unrelated)
- All other test suites: passing

## Backward Compatibility
✅ Complete backward compatibility maintained:
- Existing projects referencing `KustoLoco.FileFormats` continue to work
- All serializers available in the same namespace
- No API changes
- All functionality preserved

## Migration Path for Users

### Option 1: No Changes (Recommended for Most Users)
Continue using `KustoLoco.FileFormats` metapackage - no changes required.

### Option 2: Optimize Dependencies
Replace metapackage with specific format packages:
```xml
<!-- Only need CSV and Parquet? -->
<PackageReference Include="KustoLoco.FileFormats.Csv" Version="x.y.z" />
<PackageReference Include="KustoLoco.FileFormats.Parquet" Version="x.y.z" />
```

## Benefits
1. **Reduced Dependencies**: Users can install only what they need
2. **Smaller Packages**: Format-specific packages have fewer dependencies
3. **Better Separation**: Each format is independently maintainable
4. **Flexibility**: Easy to add new formats in the future
5. **No Breaking Changes**: Metapackage ensures compatibility

## Package Publishing Workflow
1. Create a new release on GitHub with semantic version tag (e.g., `v1.2.3`)
2. GitHub Actions workflow automatically:
   - Builds all projects
   - Runs tests
   - Creates NuGet packages
   - Publishes to NuGet.org (if NUGET_API_KEY is set)
   - Uploads artifacts

Alternative: Manual dispatch for testing/preview releases

## Documentation
- Created `libraries/FileFormats/PACKAGES.md` with detailed usage guide
- Includes migration instructions
- Lists all packages and their dependencies

## Next Steps for Maintainers
1. Set up `NUGET_API_KEY` secret in GitHub repository settings
2. Create a release to trigger automatic NuGet publishing
3. Consider updating main README.md to reference the new package structure
4. Update any documentation that specifically mentions FileFormats package
