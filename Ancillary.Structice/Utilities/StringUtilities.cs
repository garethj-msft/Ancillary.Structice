using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancillary.Structice.Utilities
{
    public static class StringUtilities
    {

        public static IEnumerable<IGrouping<string, string>> ThenSplit(this string[] theList, char separator)
        {
            foreach (string entry in theList)
            {
                yield return new GroupedEnumerable(entry, entry.Split(separator));
            }
        }

        internal class GroupedEnumerable : IGrouping<string, string>
        {
            public string Key { get; private set; }

            private string[] Values { get; set; }

            internal GroupedEnumerable(string key, string[] values)
            {
                this.Key = key;
                this.Values =  values;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return ((IEnumerable<string>)this.Values).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.Values.GetEnumerator();
            }
        }
    }
}
