using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Business.Core.Domain.Events.Retry;

public readonly struct Result<T>
{
    public readonly T Payload { get; }

    public Result(T payload)
    {
        Payload = payload;
    }
}
