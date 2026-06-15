using Content.Shared.Projectiles;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Server._Misfits.EmbeddedProjectiles;

/// <summary>
///     #Misfits Add - Drops embedded projectiles before a mob/silicon is deleted, preventing
///     them from being silently destroyed as transform children of the slain entity.
///     This mirrors the butchering fix in SharpSystem but covers non-butchering death paths
///     (robots destroyed in combat, gibbing, admin deletion, etc.).
/// </summary>
public sealed class EmbeddedProjectileDropSystem : EntitySystem
{
    [Dependency] private readonly SharedProjectileSystem _projectile = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TransformComponent, EntityTerminatingEvent>(OnEntityTerminating);
    }

    private void OnEntityTerminating(Entity<TransformComponent> entity, ref EntityTerminatingEvent args)
    {
        // Snapshot children first because RemoveEmbed reparents the child,
        // which would modify the enumerator mid-iteration.
        var children = new List<EntityUid>();
        var childEnumerator = entity.Comp.ChildEnumerator;
        while (childEnumerator.MoveNext(out var child))
            children.Add(child);

        foreach (var child in children)
        {
            if (TryComp<EmbeddableProjectileComponent>(child, out var embed))
                _projectile.RemoveEmbed(child, embed, null);
        }
    }
}
