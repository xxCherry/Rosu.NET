using System.Runtime.InteropServices;

namespace Rosu.Net;

public enum RosuPpError : int
{
    Ok = 0,
    NullPointer = 1,
    InvalidUtf8 = 2,
    IoError = 3,
    Panic = 4,
    TooSuspicious = 5,
    EndOfStream = 6,
}

public enum RosuPpGameMode : int
{
    Osu = 0,
    Taiko = 1,
    Catch = 2,
    Mania = 3,
}

public enum RosuPpSuspicion : int
{
    None = 0,
    Density = 1,
    Length = 2,
    ObjectCount = 3,
    RedFlag = 4,
    SliderPositions = 5,
    SliderRepeats = 6,
    Unknown = 255,
}

[StructLayout(LayoutKind.Sequential)]
public struct RosuPpScoreState
{
    public uint max_combo;
    public uint osu_large_tick_hits;
    public uint osu_small_tick_hits;
    public uint slider_end_hits;
    public uint n_geki;
    public uint n_katu;
    public uint n300;
    public uint n100;
    public uint n50;
    public uint misses;
}

[StructLayout(LayoutKind.Sequential)]
public struct RosuPpDifficultyAttributes
{
    public double stars;
    public uint max_combo;
    public RosuPpGameMode mode;
}

[StructLayout(LayoutKind.Sequential)]
public struct RosuPpPerformanceAttributes
{
    public double pp;
    public double stars;
    public uint max_combo;
    public RosuPpGameMode mode;
}

internal static partial class Native
{
    private const string LibName = "rosu_pp";

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial nint rosu_pp_error_str(int err);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpScoreState rosu_pp_score_state_new();

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_beatmap_from_path([MarshalAs(UnmanagedType.LPStr)] string path, out nint map);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_beatmap_from_bytes(
        byte[] bytes,
        nuint len,
        out nint map
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void rosu_pp_beatmap_free(nint map);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_beatmap_mode(nint map, out RosuPpGameMode mode);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_beatmap_check_suspicion(
        nint map,
        out RosuPpSuspicion suspicion
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_difficulty_calculate(
        nint map,
        uint mods,
        out nint difficulty
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void rosu_pp_difficulty_attrs_free(nint difficulty);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_difficulty_attrs_values(
        nint difficulty,
        out RosuPpDifficultyAttributes values
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_performance_calculate(
        nint difficulty,
        uint mods,
        double accuracy,
        uint combo,
        uint misses,
        out nint performance
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void rosu_pp_performance_attrs_free(nint performance);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_performance_attrs_values(
        nint performance,
        out RosuPpPerformanceAttributes values
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_performance_attrs_max_pp(
        nint performance,
        uint mods,
        out double maxPp
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_gradual_performance_new(
        nint map,
        uint mods,
        double clock_rate,
        out nint gradual
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void rosu_pp_gradual_performance_free(nint gradual);

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_gradual_performance_next(
        nint gradual,
        ref RosuPpScoreState state,
        out RosuPpPerformanceAttributes attrs
    );

    [LibraryImport(LibName)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial RosuPpError rosu_pp_gradual_performance_last(
        nint gradual,
        ref RosuPpScoreState state,
        out RosuPpPerformanceAttributes attrs
    );

    internal static string ErrorString(RosuPpError err)
    {
        var ptr = rosu_pp_error_str((int)err);
        return Marshal.PtrToStringAnsi(ptr) ?? "Unknown";
    }

    internal static void ThrowIfError(RosuPpError err)
    {
        if (err == RosuPpError.Ok)
        {
            return;
        }

        throw new RosuPpException(err);
    }
}

