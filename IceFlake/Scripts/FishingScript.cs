using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;
using IceFlake.Client.Objects;
using IceFlake.Client.API;
using IceFlake.Client.Patchables;

namespace IceFlake.Scripts
{
    public class FishingBot : Script
    {
        public FishingBot()
            : base("Fishing", "Script")
        { }

        private int Fishes;
        private WoWUnit Trainer;
        private WoWSpell Fishing;
        private DateTime LootTimer;
        private DateTime CombatTimer;
        private DateTime SleepTimer = DateTime.MinValue;

        private List<string> EventSubscription = new List<string>
        {
            "LOOT_OPENED",
            "LOOT_SLOT_CLEARED",
            "LOOT_CLOSED",
            "SKILL_LINES_CHANGED",
            "PLAYER_REGEN_DISABLED",
            "PLAYER_REGEN_ENABLED"
        };

        private FishingState CurrentState
        {
            get;
            set;
        }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Manager.Spellbook.Update = true;
            CurrentState = FishingState.Lure;
            Fishes = 0;
            Fishing = Manager.Spellbook["Fishing"];
            if (!Fishing.IsValid)
            {
                Print("You don't know fishing!");
                Stop();
            }

            foreach (var ev in EventSubscription)
                Manager.Events.Register(ev, HandleFishingEvents);
        }

        public override void OnTerminate()
        {
            foreach (var ev in EventSubscription)
                Manager.Events.Remove(ev, HandleFishingEvents);

            CurrentState = FishingState.Lure;
            Fishing = null;
            Print("Fishing season is over! You did however catch {0} fishes.", Fishes);
        }

        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if ((DateTime.Now - SleepTimer).TotalMilliseconds < 200)
                return;
            SleepTimer = DateTime.Now;

            switch (CurrentState)
            {
                case FishingState.Lure:
                    //if (!HasBait)
                    //{
                    //    // Implement applying lure
                    //}
                    var fish = API.Profession.Fishing;
                    if (fish != null)
                    {
                        if (fish.Level >= (fish.MaxLevel - 25) && fish.MaxLevel < 450)
                        {
                            CurrentState = FishingState.Training;
                            return;
                        }
                    }
                    CurrentState = FishingState.Cast;
                    break;

                case FishingState.Cast:
                    Print("Casting Fishing Pole");
                    Fishing.Cast();
                    CurrentState = FishingState.Fishing;
                    break;

                case FishingState.Fishing:
                    if (IsFishing)
                    {
                        if (IsBobbing)
                            CurrentState = FishingState.Loot;
                    }
                    else
                        CurrentState = FishingState.Lure;
                    break;

                case FishingState.Loot:
                    Print("Getting Fishing Bobber");
                    Bobber.Interact();
                    LootTimer = DateTime.Now;
                    CurrentState = FishingState.Looting;
                    break;

                case FishingState.Looting:
                    var span = DateTime.Now - LootTimer;
                    if (span.TotalSeconds > 3)
                    {
                        Print("No loot? Back to fishing then!");
                        CurrentState = FishingState.Lure;
                    }
                    break;

                // TODO: Clean up, add dynamic fishing trainer locator.
                case FishingState.Training:
                    if (!Trainer.IsValid)
                    { // Marcia Chase, Dalaran - Neutral.
                        Trainer = Manager.ObjectManager.Objects.Where(x => x.IsValid && x.IsUnit).OfType<WoWUnit>().FirstOrDefault(u => u.Entry == 28742);
                        return;
                    }

                    if (Trainer.Distance > 6f)
                    {
                        if (Manager.Movement.Destination == Trainer.Location)
                            Sleep(100);
                        Manager.Movement.PathTo(Trainer.Location);
                        return;
                    }

                    if (!API.Gossip.IsShown && !API.Trainer.IsShown)
                    {
                        Trainer.Interact();
                        return;
                    }

                    if (API.Gossip.IsShown)
                    {
                        var opt = API.Gossip.Options.FirstOrDefault(g => g.Gossip == GossipType.Trainer);
                        if (opt != null)
                            opt.Select();
                        return;
                    }

                    if (API.Trainer.IsShown)
                    {
                        API.Trainer.BuyAllAvailable();
                        Fishing = null;
                        Manager.Spellbook.Update = true;
                        CurrentState = FishingState.Lure;
                        return;
                    }

                    CurrentState = FishingState.Lure;

                    break;

            }
        }

        private void HandleFishingEvents(string ev, List<string> args)
        {
            //Print("DEBUG - EVENT: {0} ({1})", ev, args);
            TimeSpan span;
            switch (ev)
            {
                case "LOOT_OPENED":
                    Print("We have {0} catches to loot", args[0]);
                    break;

                case "LOOT_SLOT_CLEARED":
                    //var loot = WoWScript.Execute("GetLootSlotInfo(" + args[0] + ")");
                    //Print("Looted [{0}]x{1}", loot[1], loot[2]);
                    break;

                case "LOOT_CLOSED":
                    span = DateTime.Now - LootTimer;
                    Print("Looting took {0} seconds.", Math.Round(span.TotalSeconds, 1));
                    Fishes++;
                    CurrentState = FishingState.Lure;
                    break;

                case "SKILL_LINES_CHANGED":
                    Print("Seems like we leveled up our fishing!");
                    break;

                case "PLAYER_REGEN_DISABLED":
                    Print("Seems like we entered combat!");
                    CombatTimer = DateTime.Now;
                    //if (CanHandleCombat)
                    //    Combat.Pulse();
                    //else
                    //    Print("... too bad you didn't select a combat brain, now we're going to die");
                    CurrentState = FishingState.Combat;
                    break;

                case "PLAYER_REGEN_ENABLED":
                    span = DateTime.Now - CombatTimer;
                    Print("Combat done after {0} seconds", Math.Round(span.TotalSeconds, 1));
                    CurrentState = FishingState.Lure;
                    break;
            }
        }

        #region Properties

        private WoWGameObject Bobber
        {
            get
            {
                return Manager.ObjectManager.Objects.Where(b => b.IsValid && b.IsGameObject)
                    .Select(b => b as WoWGameObject)
                    .Where(b => b.CreatedByMe && b.Name == "Fishing Bobber")
                    .FirstOrDefault();
            }
        }

        private bool IsBobbing
        {
            get { return (Bobber.IsValid ? Manager.Memory.Read<byte>(new IntPtr(Bobber.Pointer.ToInt64() + Pointers.Other.IsBobbing)) == 1 : false); }
        }

        private bool IsFishing
        {
            get { return Manager.LocalPlayer.ChanneledCastingId == Fishing.Id; }
        }

        private bool HasBait
        {
            get { return WoWScript.Execute<int>("GetWeaponEnchantInfo()", 0) == 1; }
        }

        #endregion

        #region FishingState enum

        enum FishingState
        {
            Lure,
            Cast,
            Fishing,
            Loot,
            Looting,
            Combat,
            Training,
        }

        #endregion
    }
}
