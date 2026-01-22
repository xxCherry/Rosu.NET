using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Rosu.Net.Attributes;

public sealed class DifficultyAttributes : SafeHandle
{
    internal DifficultyAttributes(nint handle) : base(nint.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == nint.Zero;

    protected override bool ReleaseHandle()
    {
        Native.rosu_pp_difficulty_attrs_free(handle);
        return true;
    }

    public RosuPpDifficultyAttributes Values
    {
        get
        {
            Native.ThrowIfError(Native.rosu_pp_difficulty_attrs_values(handle, out RosuPpDifficultyAttributes values));
            return values;
        }
    }

    public PerformanceAttributes CalculatePerformance(uint mods, double accuracy, uint combo, uint misses)
    {
        Native.ThrowIfError(
            Native.rosu_pp_performance_calculate(handle, mods, accuracy, combo, misses, out nint perf)
        );
        return new PerformanceAttributes(perf);
    }
}
