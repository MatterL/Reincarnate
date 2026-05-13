using Content.Shared.RI.Prototypes;
using Content.Shared.RI.Stats;
using NUnit.Framework;
using Robust.Shared.Prototypes;

namespace Content.Tests.RI.Stats;

[TestFixture]
public sealed class RiStatSystemTests
{
    [Test]
    public void HumanMediumUsesTemplateValues()
    {
        var template = HumanTemplate();
        var body = MediumBody();

        var block = RiStatSystem.BuildInitialCharacterStats(template, body);

        Assert.That(block.GetFinalValue(RiStatType.EnergyCapacity), Is.EqualTo(1.5f));
        Assert.That(block.GetFinalValue(RiStatType.Strength), Is.EqualTo(1f));
        Assert.That(block.GetFinalValue(RiStatType.Regeneration), Is.EqualTo(0.5f));
        Assert.That(block.GetFinalValue(RiStatType.Recovery), Is.EqualTo(3f));
    }

    [Test]
    public void HumanSmallAppliesExplicitBodyMultipliers()
    {
        var template = HumanTemplate();
        var body = SmallBody();

        var block = RiStatSystem.BuildInitialCharacterStats(template, body);

        Assert.That(block.GetFinalValue(RiStatType.Strength), Is.EqualTo(0.5f));
        Assert.That(block.GetFinalValue(RiStatType.Endurance), Is.EqualTo(0.5f));
        Assert.That(block.GetFinalValue(RiStatType.Speed), Is.EqualTo(1.25f));
        Assert.That(block.GetFinalValue(RiStatType.Force), Is.EqualTo(1.25f));
        Assert.That(block.GetFinalValue(RiStatType.Offense), Is.EqualTo(1.5f));
        Assert.That(block.GetFinalValue(RiStatType.Defense), Is.EqualTo(1.5f));
    }

    [Test]
    public void HumanLargeAppliesExplicitBodyMultipliers()
    {
        var template = HumanTemplate();
        var body = LargeBody();

        var block = RiStatSystem.BuildInitialCharacterStats(template, body);

        Assert.That(block.GetFinalValue(RiStatType.Strength), Is.EqualTo(1.5f));
        Assert.That(block.GetFinalValue(RiStatType.Endurance), Is.EqualTo(1.5f));
        Assert.That(block.GetFinalValue(RiStatType.Speed), Is.EqualTo(0.7f));
        Assert.That(block.GetFinalValue(RiStatType.Force), Is.EqualTo(0.7f));
        Assert.That(block.GetFinalValue(RiStatType.Offense), Is.EqualTo(0.7f));
        Assert.That(block.GetFinalValue(RiStatType.Defense), Is.EqualTo(0.7f));
    }

    [Test]
    public void ChangelingLikeRaceRejectsSmallAndLargeBody()
    {
        var race = Race(
            id: "Changeling",
            allowedClasses: ["Changeling.Fighter"],
            tags: ["Humanoid", "ChangelingLike"]);

        var cls = Class(
            id: "Changeling.Fighter",
            allowedRaceIds: ["Changeling"],
            statTemplate: "Changeling.Fighter");

        var small = SmallBody();
        var large = LargeBody();

        var smallResult = RiStatSystem.ValidateChoices(race, cls, small);
        var largeResult = RiStatSystem.ValidateChoices(race, cls, large);

        Assert.That(smallResult.Valid, Is.False);
        Assert.That(smallResult.Error, Does.Contain("ChangelingLike"));

        Assert.That(largeResult.Valid, Is.False);
        Assert.That(largeResult.Error, Does.Contain("ChangelingLike"));
    }

    [Test]
    public void AlienClassVariantUsesReplacementTemplate()
    {
        var fighterTemplate = AlienFighterTemplate();
        var technologistTemplate = AlienTechnologistTemplate();
        var medium = MediumBody();

        var fighter = RiStatSystem.BuildInitialCharacterStats(fighterTemplate, medium);
        var technologist = RiStatSystem.BuildInitialCharacterStats(technologistTemplate, medium);

        Assert.That(fighter.GetFinalValue(RiStatType.Endurance), Is.EqualTo(0.5f));
        Assert.That(fighter.GetFinalValue(RiStatType.Resistance), Is.EqualTo(0.5f));
        Assert.That(fighter.GetFinalValue(RiStatType.Defense), Is.EqualTo(0.5f));

        Assert.That(technologist.GetFinalValue(RiStatType.Endurance), Is.EqualTo(1.5f));
        Assert.That(technologist.GetFinalValue(RiStatType.Resistance), Is.EqualTo(1.5f));
        Assert.That(technologist.GetFinalValue(RiStatType.Defense), Is.EqualTo(1.5f));
    }

    [Test]
    public void FormulaOrderIsDeterministic()
    {
        var block = RiStatMath.BuildBlock(
            baseValues: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 10f,
            },
            earnedBonuses: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 5f,
            },
            creationMultipliers: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 2f,
            },
            permanentAddBonuses: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 3f,
            },
            temporaryAddBonuses: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 4f,
            },
            temporaryMultipliers: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 1.5f,
            });

        // ((10 + 5 + 3) * 2 + 4) * 1.5 = 60
        Assert.That(block.GetFinalValue(RiStatType.Strength), Is.EqualTo(60f));
    }

    [Test]
    public void CreationAllocationIsPermanentAddNotMultiplier()
    {
        var template = HumanTemplate();
        var body = MediumBody();

        var block = RiStatSystem.BuildInitialCharacterStats(
            template,
            body,
            creationAllocations: new Dictionary<RiStatType, float>
            {
                [RiStatType.Strength] = 4f,
            });

        // Human strength affinity is 1. The allocation adds 4 to the permanent base side.
        // (1 + 4) * 1 = 5
        Assert.That(block.GetFinalValue(RiStatType.Strength), Is.EqualTo(5f));
    }

    private static RiRacePrototype Race(string id, string[] allowedClasses, string[] tags)
    {
        var race = new RiRacePrototype();

        SetPrototypeId(race, id);
        race.DisplayName = id;
        race.DefaultClass = allowedClasses[0];
        race.AllowedClasses = allowedClasses.Select(x => (ProtoId<RiClassPrototype>) x).ToList();
        race.StatTemplate = id;
        race.Tags = tags.ToHashSet();

        return race;
    }

    private static RiClassPrototype Class(string id, string[] allowedRaceIds, string statTemplate)
    {
        var cls = new RiClassPrototype();

        SetPrototypeId(cls, id);
        cls.DisplayName = id;
        cls.SourceClassName = id.Split('.').Last();
        cls.AllowedRaceIds = allowedRaceIds.Select(x => (ProtoId<RiRacePrototype>) x).ToList();
        cls.StatTemplate = statTemplate;

        return cls;
    }

    private static RiBodyTypePrototype MediumBody()
    {
        var body = new RiBodyTypePrototype();
        SetPrototypeId(body, "Medium");
        body.DisplayName = "Medium";
        return body;
    }

    private static RiBodyTypePrototype SmallBody()
    {
        var body = new RiBodyTypePrototype();
        SetPrototypeId(body, "Small");
        body.DisplayName = "Small";
        body.StatAffinityMultipliers = new Dictionary<RiStatType, float>
        {
            [RiStatType.Strength] = 0.5f,
            [RiStatType.Endurance] = 0.5f,
            [RiStatType.Speed] = 1.25f,
            [RiStatType.Resistance] = 0.5f,
            [RiStatType.Force] = 1.25f,
            [RiStatType.Offense] = 1.5f,
            [RiStatType.Defense] = 1.5f,
        };
        body.ExcludedRaceTags = ["ChangelingLike"];
        return body;
    }

    private static RiBodyTypePrototype LargeBody()
    {
        var body = new RiBodyTypePrototype();
        SetPrototypeId(body, "Large");
        body.DisplayName = "Large";
        body.StatAffinityMultipliers = new Dictionary<RiStatType, float>
        {
            [RiStatType.Strength] = 1.5f,
            [RiStatType.Endurance] = 1.5f,
            [RiStatType.Speed] = 0.7f,
            [RiStatType.Resistance] = 1.5f,
            [RiStatType.Force] = 0.7f,
            [RiStatType.Offense] = 0.7f,
            [RiStatType.Defense] = 0.7f,
        };
        body.ExcludedRaceTags = ["ChangelingLike"];
        return body;
    }

    private static RiStatTemplatePrototype HumanTemplate()
    {
        return Template("Human", new Dictionary<RiStatType, float>
        {
            [RiStatType.EnergyCapacity] = 1.5f,
            [RiStatType.Strength] = 1f,
            [RiStatType.Endurance] = 1f,
            [RiStatType.Speed] = 1f,
            [RiStatType.Force] = 1f,
            [RiStatType.Resistance] = 1f,
            [RiStatType.Offense] = 1f,
            [RiStatType.Defense] = 1f,
            [RiStatType.Regeneration] = 0.5f,
            [RiStatType.Recovery] = 3f,
            [RiStatType.Anger] = 0.5f,
        });
    }

    private static RiStatTemplatePrototype AlienFighterTemplate()
    {
        return Template("Alien.Fighter", new Dictionary<RiStatType, float>
        {
            [RiStatType.EnergyCapacity] = 0.5f,
            [RiStatType.Strength] = 0.5f,
            [RiStatType.Endurance] = 0.5f,
            [RiStatType.Speed] = 0.5f,
            [RiStatType.Force] = 0.5f,
            [RiStatType.Resistance] = 0.5f,
            [RiStatType.Offense] = 0.5f,
            [RiStatType.Defense] = 0.5f,
            [RiStatType.Regeneration] = 0.5f,
            [RiStatType.Recovery] = 2f,
            [RiStatType.Anger] = 0.4f,
        });
    }

    private static RiStatTemplatePrototype AlienTechnologistTemplate()
    {
        return Template("Alien.Technologist", new Dictionary<RiStatType, float>
        {
            [RiStatType.EnergyCapacity] = 0.5f,
            [RiStatType.Strength] = 0.5f,
            [RiStatType.Endurance] = 1.5f,
            [RiStatType.Speed] = 0.5f,
            [RiStatType.Force] = 0.5f,
            [RiStatType.Resistance] = 1.5f,
            [RiStatType.Offense] = 0.5f,
            [RiStatType.Defense] = 1.5f,
            [RiStatType.Regeneration] = 0.5f,
            [RiStatType.Recovery] = 0.3f,
            [RiStatType.Anger] = 0.4f,
        });
    }

    private static RiStatTemplatePrototype Template(string id, Dictionary<RiStatType, float> affinities)
    {
        var template = new RiStatTemplatePrototype();
        SetPrototypeId(template, id);
        template.BaseAffinities = affinities;
        return template;
    }

    private static void SetPrototypeId<T>(T prototype, string id)
        where T : class, IPrototype
    {
        var property = typeof(T).GetProperty(nameof(IPrototype.ID))!;
        property.SetValue(prototype, id);
    }
}