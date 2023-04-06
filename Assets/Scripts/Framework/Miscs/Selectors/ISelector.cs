using System;

/// <summary>
/// Selects an element.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISelector<T>
{
    abstract T Select(Predicate<T> predicate = null);

    abstract bool TryToSelect(out T selectable, Predicate<T> predicate = null);
}
