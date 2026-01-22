using Rosu.Net.Attributes;
using Rosu.Net.Performance;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Rosu.Net;

public sealed class Beatmap : SafeHandle
{
    private Beatmap() : base(nint.Zero, ownsHandle: true) { }

    private Beatmap(nint handle) : base(nint.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == nint.Zero;

    protected override bool ReleaseHandle()
    {
        Native.rosu_pp_beatmap_free(handle);
        return true;
    }

    public static Beatmap FromPath(string path)
    {
        Native.ThrowIfError(Native.rosu_pp_beatmap_from_path(path, out nint map));
        return new Beatmap(map);
    }

    public static Beatmap FromBytes(byte[] bytes)
    {
        Native.ThrowIfError(Native.rosu_pp_beatmap_from_bytes(bytes, (nuint)bytes.Length, out nint map));
        return new Beatmap(map);
    }

    public RosuPpGameMode Mode
    {
        get
        {
            Native.ThrowIfError(Native.rosu_pp_beatmap_mode(handle, out RosuPpGameMode mode));
            return mode;
        }
    }

    public bool IsSuspicious(out RosuPpSuspicion suspicion)
    {
        RosuPpError err = Native.rosu_pp_beatmap_check_suspicion(handle, out suspicion);

        return err switch
        {
            RosuPpError.Ok => false,
            RosuPpError.TooSuspicious => true,
            _ => throw new RosuPpException(err),
        };
    }

    public DifficultyAttributes CalculateDifficulty(uint mods)
    {
        Native.ThrowIfError(Native.rosu_pp_difficulty_calculate(handle, mods, out nint diff));
        return new DifficultyAttributes(diff);
    }

    public GradualPerformance CreateGradualPerformance(uint mods, double clockRate = 0.0)
    {
        Native.ThrowIfError(Native.rosu_pp_gradual_performance_new(handle, mods, clockRate, out nint gradual));
        return new GradualPerformance(gradual);
    }
}

