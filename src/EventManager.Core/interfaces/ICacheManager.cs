using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client.interfaces
{
    public interface ICacheManager
    {
        bool TryGetValue<TItem>(object key, out TItem value);
        TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
    }
}
