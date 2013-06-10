using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using IceFlake.Client.Patchables;
using SlimDX;
using SlimDX.Direct3D9;
using System.Runtime.InteropServices;
using System.Drawing;
using IceFlake.DirectX;

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
            : base("Spellbook", "Dumper")
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
            Print("\tPosition: {0}", camera.Position);
            Print("\tFoV: {0}", camera.FieldOfView);
            Print("\tNearZ: {0}", camera.NearZ);
            Print("\tFarZ: {0}", camera.FarZ);
            Print("\tAspect: {0}", camera.Aspect);

            Stop();
        }
    }

    #endregion

    #region DrawUnitsScript


    public class DrawUnitsScript : Script
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PositionColored
        {
            public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
            public static readonly int Stride = Vector3.SizeInBytes + sizeof(int);

            public Vector3 Position;
            public int Color;

            public PositionColored(Vector3 pos, int col)
            {
                Position = pos;
                Color = col;
            }
        }

        public DrawUnitsScript()
            : base("Draw Units", "Test")
        {
            colorGreen = Color.FromArgb(0x8f, 0, 0xff, 0);
            colorRed = Color.FromArgb(0x8f, 0xff, 0, 0);
            colorBlue = Color.FromArgb(0x8f, 0, 0, 0xff);
        }

        private Color colorGreen;
        private Color colorRed;
        private Color colorBlue;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
            {
                Stop();
                return;
            }
        }

        public override void OnTick()
        {
            foreach (var u in Manager.ObjectManager.Objects.Where(x => x.IsValid && x.IsUnit).OfType<WoWUnit>())
            {
                if (u == null || !u.IsValid)
                    continue;

                var color = (!u.IsFriendly ? colorRed : colorGreen);
                DrawCircle(u.Location, 3f, color, color);
            }
        }

        private void DrawCircle(Location loc, float radius, Color innerColor, Color outerColor, int complexity = 24, bool isFilled = true)
        {
            var vertices = new List<PositionColored>();
            if (isFilled)
                vertices.Add(new PositionColored(Vector3.Zero, innerColor.ToArgb()));

            double stepAngle = (Math.PI * 2) / complexity;
            for (int i = 0; i <= complexity; i++)
            {
                double angle = (Math.PI * 2) - (i * stepAngle);
                float x = (float)(radius * Math.Cos(angle));
                float y = (float)(-radius * Math.Sin(angle));
                vertices.Add(new PositionColored(new Vector3(x, y, 0), outerColor.ToArgb()));
            }

            var buffer = vertices.ToArray();

            SetTarget(new Vector3(loc.X, loc.Y, loc.Z + 1));

            IceFlake.DirectX.Direct3D.Device.DrawUserPrimitives(PrimitiveType.TriangleFan, buffer.Length - 2, buffer);
        }

        private void SetTarget(Vector3 target, float yaw = 0, float pitch = 0, float roll = 0)
        {
            var worldMatrix = Matrix.Translation(target) * Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            IceFlake.DirectX.Direct3D.Device.SetTransform(TransformState.World, worldMatrix);
        }
    }

    #endregion
}
