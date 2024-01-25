## Overview

Issues related to database update commands

### Domain

- `GuestFeedback` entity - represents a feedback from user, related many-to-one with `Listing` 
- `Rating` entity - represents abstract listing rating, related as owned type with `Listing`


### The problem

#### Summary

Delete operation of any `GuestFeedback` entry causing an exception on database side, for non-nullability reason.

#### Infrastructure and flow

- remove operations is called from `RatingsController`
- deletion is intercepted by `UpdateSoftDeletionInterceptor` in order to avoid hard deletion

#### Details 

- the problem is owned types considered separate entity and they have their separate entry state

### Specifications

- C# - 12
- .NET - 8
- .EF Core - 8

## Solutions

### Use target type selection in interceptor to avoid them from soft deletion

```csharp
          var ownedTypes = entry.References.Where(entity => entity.Metadata.TargetEntityType.IsOwned()).ToList();
          ownedTypes.ForEach(entity => entity.TargetEntry.State = EntityState.Modified);
```

