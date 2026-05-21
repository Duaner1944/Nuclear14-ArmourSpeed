using Content.Shared._Misfits.Special;
using Content.Shared._Misfits.Special.Components;
using Content.Shared.Damage;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Random;

namespace Content.Server._Misfits.Special;

public sealed class SpecialCombatSystem : EntitySystem
{
    [Dependency] private readonly SharedSpecialSystem _special = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GetMeleeDamageEvent>(OnGetMeleeDamage);
        SubscribeLocalEvent<MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<ProjectileComponent, ProjectileHitEvent>(OnProjectileHit);
        SubscribeLocalEvent<SpecialComponent, SpecialModifyHitscanDamageEvent>(OnModifyHitscanDamage);
    }

    private void OnGetMeleeDamage(ref GetMeleeDamageEvent args)
    {
        if (!TryComp<SpecialComponent>(args.User, out var special))
            return;

        var tuning = _special.GetTuning();
        var delta = _special.GetEffectDelta(args.User, SpecialStat.Strength, special);
        var multiplier = 1f + delta * tuning.StrengthMeleeDamageMultiplierPerPoint;

        args.Damage *= MathF.Max(0.1f, multiplier);
    }

    private void OnMeleeHit(MeleeHitEvent args)
    {
        if (!args.IsHit || args.HitEntities.Count == 0)
            return;

        if (!TryComp<SpecialComponent>(args.User, out var special))
            return;

        var damage = args.BaseDamage;
        if (TryApplyLuckCritical(args.User, ref damage, special))
            args.BonusDamage += damage - args.BaseDamage;
    }

    private void OnProjectileHit(Entity<ProjectileComponent> ent, ref ProjectileHitEvent args)
    {
        if (args.Shooter == null ||
            !TryComp<SpecialComponent>(args.Shooter.Value, out var special))
            return;

        var damage = args.Damage;
        if (TryApplyLuckCritical(args.Shooter.Value, ref damage, special))
            args.Damage = damage;
    }

    private void OnModifyHitscanDamage(Entity<SpecialComponent> ent, ref SpecialModifyHitscanDamageEvent args)
    {
        var damage = args.Damage;
        if (TryApplyLuckCritical(ent.Owner, ref damage, ent.Comp))
            args.Damage = damage;
    }

    private bool TryApplyLuckCritical(EntityUid user, ref DamageSpecifier damage, SpecialComponent special)
    {
        var tuning = _special.GetTuning();
        var chance = _special.GetLuckRollChance(user, 0f, tuning.LuckCriticalChancePerPoint, special);

        if (chance <= 0f || !_random.Prob(chance))
            return false;

        damage *= tuning.LuckCriticalDamageMultiplier;
        return true;
    }
}
