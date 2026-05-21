using Robust.Shared.Serialization;

namespace Content.Shared._Misfits.Special;

[Serializable, NetSerializable]
public enum SpecialStat : byte
{
    Strength,
    Perception,
    Endurance,
    Charisma,
    Intelligence,
    Agility,
    Luck,
}

public static class SpecialStats
{
    public static readonly SpecialStat[] All =
    {
        SpecialStat.Strength,
        SpecialStat.Perception,
        SpecialStat.Endurance,
        SpecialStat.Charisma,
        SpecialStat.Intelligence,
        SpecialStat.Agility,
        SpecialStat.Luck,
    };
}
