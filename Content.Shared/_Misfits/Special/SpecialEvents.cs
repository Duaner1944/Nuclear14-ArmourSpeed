using Content.Shared.Damage;

namespace Content.Shared._Misfits.Special;

public sealed class SpecialChangedEvent : EntityEventArgs;

[ByRefEvent]
public record struct SpecialModifyHitscanDamageEvent(EntityUid Weapon, DamageSpecifier Damage);
