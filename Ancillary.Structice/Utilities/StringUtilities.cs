// <copyright file="StringUtilities.cs" company="Gareth Jones">
// © Gareth Jones. All rights reserved.
// </copyright>

namespace Ancillary.Structice.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Simple extra utilities for working with strings.
/// </summary>
public static class StringUtilities
{
    /// <summary>
    /// Split a set of strings into groups split on the given character.
    /// </summary>
    /// <param name="stringSet">Set of strings.</param>
    /// <param name="separator">Character to split on.</param>
    /// <returns>The groups.</returns>
    public static IEnumerable<IGrouping<string, string>> ThenSplit(this IEnumerable<string> stringSet, char separator)
    {
        _ = stringSet ?? throw new ArgumentNullException(nameof(stringSet));
        return stringSet.Select(entry => new Grouping<string, string>(entry, entry.Split(separator)));
    }

    /// <summary>
    /// Simple class to wrap a group of elements with a key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of the group.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the group.</typeparam>
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grouping{TKey, TElement}"/> class.
        /// </summary>
        /// <param name="key">The key of the group.</param>
        /// <param name="values">The elements to wrap in a group.</param>
        internal Grouping(TKey key, IEnumerable<TElement> values)
        {
            this.Key = key;
            this.Values = values;
        }

        /// <summary>
        /// Gets the key of the group.
        /// </summary>
        public TKey Key { get; }

        private IEnumerable<TElement> Values { get; }

        /// <summary>
        /// Get the enumerator of the elements.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<TElement> GetEnumerator() => this.Values.GetEnumerator();

        /// <summary>
        /// Get the enumerator of the elements.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.Values.GetEnumerator();
    }
}
