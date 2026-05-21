# S.P.E.C.I.A.L. tuning

Base values live on `HumanoidCharacterProfile.Special` and are copied to `SpecialComponent` when the character spawns.
Runtime systems should query `SharedSpecialSystem` instead of reading fields directly:

- `GetBase(entity, stat)` for character-creation values.
- `GetModifier(entity, stat)` for temporary modifier totals.
- `GetEffective(entity, stat)` for gameplay-safe values clamped to 1-10.
- `HasRequirement(entity, stat, minimum)` for perks, weapons, or future skill gates.
- `TryModifyTemporary(entity, stat, modifier, duration, source)` for drugs, chems, injuries, perks, or equipment.

Balance values are in `Resources/Prototypes/_Misfits/Special/special_tuning.yml`.
Initial effects are deliberately small because SS14 combat is real-time:

- Strength changes melee damage by `strengthMeleeDamageMultiplierPerPoint` per point away from 5.
- Perception changes ranged spread/recoil by `perceptionSpreadReductionPerPoint` per point away from 5.
- Endurance changes stamina crit threshold by `enduranceStaminaCritThresholdPerPoint` per point away from 5.
- Agility changes movement speed by `agilityMovementSpeedMultiplierPerPoint` per point away from 5.
- Luck adds critical-hit and lucky-scavenge chance per point above 5.

Charisma and Intelligence are intentionally API-only until barter, reputation, companion, crafting, medical, or skill systems have concrete hooks.
