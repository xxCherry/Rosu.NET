using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Rosu.Net.Attributes;

public sealed class PerformanceAttributes : SafeHandle
{
    internal PerformanceAttributes(nint handle) : base(nint.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == nint.Zero;

    protected override bool ReleaseHandle()
    {
        Native.rosu_pp_performance_attrs_free(handle);
        return true;
    }

    public RosuPpPerformanceAttributes Values
    {
        get
        {
            Native.ThrowIfError(
                Native.rosu_pp_performance_attrs_values(handle, out RosuPpPerformanceAttributes values)
            );
            return values;
        }
    }

    public double MaxPp(uint mods)
    {
        Native.ThrowIfError(Native.rosu_pp_performance_attrs_max_pp(handle, mods, out double pp));
        return pp;
    }
}

