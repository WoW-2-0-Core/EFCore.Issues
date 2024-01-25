using System.Collections;

namespace IncludeIssue.Api.Domain.Extensions;

/// <summary>
/// Provides type reflection extensions
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Generic collection types
    /// </summary>
    public static IList<Type> GenericCollections { get; } = new List<Type>
    {
        typeof(IEnumerable<>),
        typeof(ICollection<>),
        typeof(IList<>),
        typeof(IAsyncEnumerable<>),
        typeof(IEnumerable)
    };

    /// <summary>
    /// Non generic collection types
    /// </summary>
    public static IList<Type> NonGenericCollections { get; } = new List<Type>
    {
        typeof(IEnumerable),
        typeof(ICollection),
        typeof(IList)
    };

    /// <summary>
    /// Checks if type is a collection
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns>True if type is collection, otherwise false</returns>
    public static bool IsCollection(this Type type)
    {
        var interfaceTypes = type.IsInterface ? [type] : type.GetInterfaces();

        return interfaceTypes.Any(
            interfaceType => interfaceType.IsGenericType
                ? GenericCollections.Contains(interfaceType.GetGenericTypeDefinition())
                : NonGenericCollections.Contains(interfaceType)
        );
    }

    /// <summary>
    /// Checks if type is a task
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsTask(this Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Task<>) || type.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type? GetGenericArgument(this Type type)
    {
        return type.GetGenericArguments().FirstOrDefault();
    }
}