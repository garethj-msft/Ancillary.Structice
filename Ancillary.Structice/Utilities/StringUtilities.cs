// <copyright file="StringUtilities.cs" >
// © Gareth Jones. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ancillary.Structice.Utilities
{
    using System;

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
        /// <returns></returns>
        public static IEnumerable<IGrouping<string, string>> ThenSplit(this IEnumerable<string> stringSet, char separator)
        {
            _ = stringSet ?? throw new ArgumentNullException(nameof(stringSet));
            return stringSet.Select(entry => new Grouping<string, string>(entry, entry.Split(separator)));
        }

        /// <summary>
        /// Simple class to wrap a group of elements with a key.
        /// </summary>
        internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
        {
            public TKey Key { get; }

            private IEnumerable<TElement> Values { get; }

            internal Grouping(TKey key, IEnumerable<TElement> values)
            {
                this.Key = key;
                this.Values =  values;
            }

            public IEnumerator<TElement> GetEnumerator() => this.Values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.Values.GetEnumerator();
        }
    }
}
