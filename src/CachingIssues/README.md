## Overview

Issues related to caching operations

- [Hash code issue](#hash-code-issue)
- [Custom queryable join issue](#custom-queryable-join-issue)

## Hash code issue

### The problem

#### Summary

#### Infrastructure and flow

#### Details 

### Specifications

- C# - 12
- .NET - 8
- .EF Core - 8

### Solutions


# Custom queryable join issue

### The problem

Using custom queryable wrapping queryable provided by EF Core causes ignoring Include operation when executing the query operation with expression

#### Infrastructure and flow

- `CachedQueryable` - custom queryable collection that wraps EF Core queryable and cache source
- `CachedQueryProvider` - custom provider that uses EF Core query provider under the hood
- `IAsyncQueryProviderResolver` - custom query provider resolver to retrieve `_entityQueryable` field from `InternalDbSet` type
- `IExpressionCacheKeyResolver` - cache key resolver to generate correct cache key for single and multiple entries
- `IQueryCacheBroker` - cache broker that tries to retrieve data from cache source, if not found uses value factory
- `ICacheBroker` - general cache broker abstracting caching source

To wrap queryable of EF Core with custom queryable we create `CachedQueryable`

#### Details 

- no expression tree visitor is used
- `CachedQueryProvider` uses `CachedQueryable` when creating query for given expression

### Specifications

- C# - 12
- .NET - 8
- .EF Core - 8