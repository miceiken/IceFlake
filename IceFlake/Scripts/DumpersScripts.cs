using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System.Linq;

namespace IceFlake.Scripts
{
    #region UnitDumperScript

    public class UnitDumperScript : Script
    {
        public UnitDumperScript()
            : base("Units", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            foreach (var u in Manager.ObjectManager.Objects.Where(x => x.IsUnit).Cast<WoWUnit>())
            {
                Print("-- {0}", u.Name);
                Print("\tGUID: 0x{0}", u.Guid.ToString("X8"));
                Print("\tHealth: {0}/{1} ({2}%)", u.Health, u.MaxHealth, (int)u.HealthPercentage);
                Print("\tReaction: {0}", u.Reaction);
                Print("\tPosition: {0}", u.Location);
            }

            Stop();
        }
    }

    #endregion

    #region PlayerDumperScript

    public class PlayerDumperScript : Script
    {
        public PlayerDumperScript()
            : base("Players", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            foreach (var p in Manager.ObjectManager.Objects.Where(x => x.IsPlayer).Cast<WoWPlayer>())
            {
                Print("-- {0}", p.Name);
                Print("\tGUID: 0x{0}", p.Guid.ToString("X8"));
                Print("\tLevel {0} {1} {2}", p.Level, p.Race, p.Class);
                Print("\tHealth: {0}/{1} ({2}%)", p.Health, p.MaxHealth, (int)p.HealthPercentage);
                Print("\tPosition: {0}", p.Location);
            }

            Stop();
        }
    }

    #endregion

    #region PartyDumperScript

    public class PartyDumperScript : Script
    {
        public PartyDumperScript()
            : base("Party", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            foreach (var p in Party.Members)
            {
                Print("-- {0}", p.Name);
                Print("\tGUID: 0x{0}", p.Guid.ToString("X8"));
                Print("\tLevel {0} {1} {2}", p.Level, p.Race, p.Class);
                Print("\tHealth: {0}/{1} ({2}%)", p.Health, p.MaxHealth, (int)p.HealthPercentage);
                Print("\tLocation: {0} ({1} yards)", p.Location, p.Distance);
                Print("\tLoS: {0}", p.InLoS);
            }

            Stop();
        }
    }

    #endregion

    #region RaidDumperScript

    public class RaidDumperScript : Script
    {
        public RaidDumperScript()
            : base("Raid", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Print("--- [ RAID ] ---");
            Print("\tInstance Difficulty: {0}", Raid.Difficulty);
            Print("\tRaid Members: {0}", Raid.NumRaidMembers);
            Print("----------------");

            foreach (var p in Raid.Members)
            {
                Print("-- {0}", p.Name);
                Print("\tGUID: 0x{0}", p.Guid.ToString("X8"));
                Print("\tLevel {0} {1} {2}", p.Level, p.Race, p.Class);
                Print("\tHealth: {0}/{1} ({2}%)", p.Health, p.MaxHealth, (int)p.HealthPercentage);
                Print("\tLocation: {0} ({1} yards)", p.Location, p.Distance);
                Print("\tLoS: {0}", p.InLoS);
            }

            Stop();
        }
    }

    #endregion

    #region InventoryItemsDumperScript

    public class InventoryItemsDumperScript : Script
    {
        public InventoryItemsDumperScript()
            : base("Inventory Items", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Print("Inventory Items");
            foreach (var item in Manager.LocalPlayer.InventoryItems)
            {
                if (item == null || !item.IsValid) continue;

                int x, y;
                if (item.GetSlotIndexes(out x, out y))
                    Print("\t({0},{1}) [{2}]x{3}", x, y, item.Name, item.StackCount);
                else
                    Print("\t[{0}]x{1}", item.Name, item.StackCount);
            }

            Stop();
        }
    }

    #endregion

    #region EquippedItemsDumperScript

    public class EquippedItemsDumperScript : Script
    {
        public EquippedItemsDumperScript()
            : base("Equipped Items", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Print("Equipped Items:");
            for (var i = (int)EquipSlot.Head; i < (int)EquipSlot.Tabard + 1; i++)
            {
                var item = Manager.LocalPlayer.GetEquippedItem(i);
                if (item == null || !item.IsValid) continue;
                Print("[{0}] {1} ({2})", (EquipSlot)i, item.Name, item.Entry);
                var itemInfo = item.ItemInfo;
                Print("\tQuality: {0}", itemInfo.Quality);
                Print("\tBonding: {0}", itemInfo.Bonding);
                Print("\tClass: {0}", itemInfo.Class);
                switch (itemInfo.Class)
                {
                    case ItemClass.Armor:
                        Print("\tArmor Class: {0}", (ItemArmorClass)itemInfo.SubClassId);
                        break;
                    case ItemClass.Weapon:
                        Print("\tWeapon Class: {0}", (ItemWeaponClass)itemInfo.SubClassId);
                        break;
                }
                Print("\tStats:");
                foreach (var pair in itemInfo.Stats)
                    Print("\t\t{0}: {1}", pair.Key, pair.Value);
                Print("\tEnchants:");
                foreach (var e in item.Enchants)
                    Print("\t\t{0} {1} {2}", e.Id, e.Charges, e.Duration);
                Print("\tFits in:");
                foreach (var s in WoWItem.GetInventorySlotsByEquipSlot(itemInfo.InventoryType))
                    Print("\t\t{0}", s);
            }

            Stop();
        }
    }

    #endregion

    #region SpellDumperScript

    public class SpellDumperScript : Script
    {
        public SpellDumperScript()
            : base("Spells", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Print("Spellbook:");
            foreach (var spell in Manager.Spellbook)
                Print("#{0}: {1}", spell.Id, spell.Name);

            Stop();
        }
    }

    #endregion

    #region QuestDumperScript

    public class QuestDumperScript : Script
    {
        public QuestDumperScript()
            : base("Quests", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            var quests = Manager.LocalPlayer.Quests;
            Print("Quests:");
            foreach (var q in quests)
            {
                Print("-- Quest #{0}", q.ID);
                Print("\tState: {0}", q.State);
                Print("\tObjectives:");
                foreach (var o in q.Objectives)
                    Print("\t\t{0}", o);
                Print("\tTime: {0}", q.Time);
            }

            Stop();
        }
    }

    #endregion

    #region CameraDumperScript

    public class CameraDumperScript : Script
    {
        public CameraDumperScript()
            : base("Camera", "Dumper")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            var camera = Manager.Camera.GetCamera();
            Print("Camera:");
            Print("\tPosition: [{0}]", camera.Position);
            Print("\tNearZ: {0}", camera.NearZ);
            Print("\tFarZ: {0}", camera.FarZ);
            Print("\tField of View: {0}", camera.FieldOfView);
            Print("\tAspect Ratio: {0}", camera.Aspect);

            Stop();
        }
    }

    #endregion
}
