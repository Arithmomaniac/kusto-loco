# KustoLoco FileFormats Packages

The KustoLoco FileFormats functionality has been split into separate packages to reduce dependencies and allow users to install only the file formats they need.

## Available Packages

### Core Packages

- **KustoLoco.FileFormats.Abstractions**: Shared interfaces and types used by all format-specific packages
  - Contains `ITableSerializer`, `TableLoadResult`, `TableSaveResult`
  - Required by all format-specific packages

### Format-Specific Packages

- **KustoLoco.FileFormats.Parquet**: Parquet file format support
  - Dependencies: `Parquet.Net`
  - Provides `ParquetSerializer`

- **KustoLoco.FileFormats.Csv**: CSV and TSV file format support
  - Dependencies: `CsvHelper`
  - Provides `CsvSerializer`

- **KustoLoco.FileFormats.Excel**: Excel file format support (XLS, XLSX)
  - Dependencies: `ClosedXML`, `ExcelDataReader.DataSet`
  - Provides `ExcelSerializer`

- **KustoLoco.FileFormats.Json**: JSON file format support
  - No additional dependencies (uses System.Text.Json)
  - Provides `JsonObjectArraySerializer`, `JsonLSerializer`

### Metapackage

- **KustoLoco.FileFormats**: Metapackage that includes all format-specific packages
  - Use this if you need support for all file formats
  - Includes all of the above packages

## Usage

### Install Specific Formats

If you only need specific file formats, install just those packages:

```bash
# For Parquet support only
dotnet add package KustoLoco.FileFormats.Parquet

# For CSV support only
dotnet add package KustoLoco.FileFormats.Csv

# For Excel support only
dotnet add package KustoLoco.FileFormats.Excel

# For JSON support only
dotnet add package KustoLoco.FileFormats.Json
```

### Install All Formats (Recommended for Compatibility)

To get all file format support (backward compatible with previous versions):

```bash
dotnet add package KustoLoco.FileFormats
```

## Migration Guide

### From Previous Versions

If you were previously using `KustoLoco.FileFormats`, no changes are required. The metapackage provides the same functionality and includes all serializers.

### Optimizing Dependencies

If you want to reduce dependencies, you can replace the metapackage with specific format packages:

```xml
<!-- Before -->
<PackageReference Include="KustoLoco.FileFormats" Version="x.y.z" />

<!-- After (example: only CSV and Parquet) -->
<PackageReference Include="KustoLoco.FileFormats.Csv" Version="x.y.z" />
<PackageReference Include="KustoLoco.FileFormats.Parquet" Version="x.y.z" />
```

## Benefits

1. **Reduced Dependencies**: Install only the format libraries you need
2. **Smaller Package Size**: Each format-specific package has fewer dependencies
3. **Flexibility**: Mix and match format support as needed
4. **No Breaking Changes**: The metapackage ensures backward compatibility
