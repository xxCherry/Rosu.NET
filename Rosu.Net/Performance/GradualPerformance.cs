using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Rosu.Net.Performance;

public sealed class GradualPerformance : SafeHandle
{
    internal GradualPerformance(nint handle) : base(nint.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == nint.Zero;

    protected override bool ReleaseHandle()
    {
        Native.rosu_pp_gradual_performance_free(handle);
        return true;
    }

    public bool TryNext(ref RosuPpScoreState state, out RosuPpPerformanceAttributes attrs)
    {
        RosuPpError err = Native.rosu_pp_gradual_performance_next(handle, ref state, out attrs);

        return err switch
        {
            RosuPpError.Ok => true,
            RosuPpError.EndOfStream => false,
            _ => throw new RosuPpException(err),
        };
    }

    public bool TryLast(ref RosuPpScoreState state, out RosuPpPerformanceAttributes attrs)
    {
        RosuPpError err = Native.rosu_pp_gradual_performance_last(handle, ref state, out attrs);

        return err switch
        {
            RosuPpError.Ok => true,
            RosuPpError.EndOfStream => false,
            _ => throw new RosuPpException(err),
        };
    }
}