using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KonoeStudio.Libs.Hid
{
    public abstract class BaseHidDeviceInfoCollection : IReadOnlyList<IHidDeviceInfo>
    {
        private readonly List<IHidDeviceInfo> _innerList;
        protected INativeHelper Helper { get; }

        public int Count => _innerList.Count;

        public IHidDeviceInfo this[int index] => _innerList[index];

        protected BaseHidDeviceInfoCollection(INativeHelper helper)
        {
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");
            _innerList = helper.EnumerateDeviceInfo().ToList();
        }

        public IEnumerator<IHidDeviceInfo> GetEnumerator()
        {
            return ((IReadOnlyList<IHidDeviceInfo>)_innerList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<IHidDeviceInfo>)_innerList).GetEnumerator();
        }
    }
}
