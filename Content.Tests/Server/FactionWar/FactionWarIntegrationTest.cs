using System;
using Content.Server._Misfits.FactionWar;
using Content.Server._Misfits.RaidRequest;
using Content.Shared._Misfits.FactionWar;
using NUnit.Framework;
using Robust.Server.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.UnitTesting;

namespace Content.Tests.Server.FactionWar;

[TestFixture]
[TestOf(typeof(FactionWarSystem))]
public sealed class FactionWarIntegrationTest : ContentUnitTest
{
    [Test]
    public void TestFactionWarSystemExists()
    {
        var entManager = IoCManager.Resolve<IEntityManager>();
        var warSystem = entManager.System<FactionWarSystem>();

        // Just verify the system exists and can be resolved.
        Assert.That(warSystem, Is.Not.Null);
    }

    [Test]
    public void TestRaidRequestSystemIntegration()
    {
        // ContentUnitTest intentionally loads a minimal system graph.
        // Validate that the raid system type and its public contract exist.
        Assert.That(typeof(RaidRequestSystem).IsSubclassOf(typeof(EntitySystem)), Is.True);
    }

    [Test]
    public void TestHasWarQuery()
    {
        var entManager = IoCManager.Resolve<IEntityManager>();
        var warSystem = entManager.System<FactionWarSystem>();

        // Test the public HasWar API (should return false for non-existent war keys).
        var nonExistentWarKey = "test-war-nonexistent-" + Guid.NewGuid();
        Assert.That(warSystem.HasWar(nonExistentWarKey), Is.False);
    }

    [Test]
    public void TestTryGetActiveWarForOriginalParticipant()
    {
        var entManager = IoCManager.Resolve<IEntityManager>();
        var warSystem = entManager.System<FactionWarSystem>();

        // Test the public TryGetActiveWarForOriginalParticipant API (should fail for non-existent players).
        var nonExistentPlayer = new NetUserId(Guid.NewGuid());
        var hasWar = warSystem.TryGetActiveWarForOriginalParticipant(nonExistentPlayer, out var war);
        Assert.That(hasWar, Is.False);
        Assert.That(war, Is.Null);
    }
}
