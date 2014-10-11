#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using IceFlake.DirectX;

namespace IceFlake.Client.Objects
{
    public class WoWLocalPlayer : WoWPlayer
    {
        #region Typedefs & Delegates

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int ClickToMoveDelegate(
            IntPtr thisPointer, int clickType, ref ulong interactGuid, ref Location clickLocation, float precision);

        public static ClickToMoveDelegate ClickToMoveFunction;

        private static IsClickMovingDelegate _isClickMoving;

        private static StopCTMDelegate _stopCTM;

        private static SetFacingDelegate _setFacing;

        private static CanUseItemDelegate _canUseItem;

        private static GetQuestInfoBlockByIdDelegate _getQuestInfoBlockById;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool CanUseItemDelegate(IntPtr thisObj, IntPtr itemSparseRec, out GameError error);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetQuestInfoBlockByIdDelegate(
            IntPtr instance, int a2, ref int a3, int a4 = 0, int a5 = 0, int a6 = 0);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool IsClickMovingDelegate(IntPtr thisObj);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SetFacingDelegate(IntPtr thisObj, uint time, float facing);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void StopCTMDelegate(IntPtr thisObj);

        #endregion

        public WoWLocalPlayer(IntPtr pointer)
            : base(pointer)
        {
#if DEBUG
            if (Direct3D.FrameCount == 0)
            {
                Log.WriteLine("LocalPlayer:");
                Log.WriteLine("\tLevel {0} {1} {2}", Level, Race, Class);
                Log.WriteLine("\tHealth: {0}/{1} ({2}%)", Health, MaxHealth, (int) HealthPercentage);
                Log.WriteLine("\t{0}: {1}/{2} ({3}%)", PowerType, Power, MaxPower, (int) PowerPercentage);
                Log.WriteLine("\tPosition: {0}", Location);
            }
#endif
        }

        public bool IsClickMoving
        {
            get
            {
                if (_isClickMoving == null)
                    _isClickMoving =
                        Manager.Memory.RegisterDelegate<IsClickMovingDelegate>(
                            (IntPtr) Pointers.LocalPlayer.IsClickMoving);

                return _isClickMoving(Pointer);
            }
        }

        public Location Corpse
        {
            get { return Manager.Memory.Read<Location>((IntPtr) Pointers.LocalPlayer.CorpsePosition); }
        }

        public int UnusedTalentPoints
        {
            get { return WoWScript.Execute<int>("UnitCharacterPoints(\"player\")", 0); }
        }

        public int Combopoints
        {
            get { return Manager.Memory.Read<int>((IntPtr) Pointers.LocalPlayer.ComboPoints); }
        }

        public IEnumerable<WoWUnit> Totems
        {
            get
            {
                return Manager.ObjectManager.Objects
                    .Where(obj => obj.IsValid && obj.IsUnit)
                    .OfType<WoWUnit>()
                    .Where(unit => unit.IsTotem && unit.CreatedBy == Guid);
                //.OfType<WoWTotem>();
            }
        }

        public void ClickToMove(Location target, ClickToMoveType type = ClickToMoveType.Move, ulong guid = 0ul)
        {
            if (ClickToMoveFunction == null)
                ClickToMoveFunction =
                    Manager.Memory.RegisterDelegate<ClickToMoveDelegate>((IntPtr) Pointers.LocalPlayer.ClickToMove);

            Helper.ResetHardwareAction();
            ClickToMoveFunction(Pointer, (int) type, ref guid, ref target, 0.1f);
        }

        public void StopCTM()
        {
            if (_stopCTM == null)
                _stopCTM = Manager.Memory.RegisterDelegate<StopCTMDelegate>((IntPtr) Pointers.LocalPlayer.StopCTM);

            _stopCTM(Pointer);
        }

        public void LookAt(Location loc)
        {
            Location local = Location;
            var diffVector = new Location(loc.X - local.X, loc.Y - local.Y, loc.Z - local.Z);
            SetFacing(diffVector.Angle);
        }

        public void SetFacing(float angle)
        {
            if (_setFacing == null)
                _setFacing = Manager.Memory.RegisterDelegate<SetFacingDelegate>((IntPtr) Pointers.LocalPlayer.SetFacing);

            const float pi2 = (float) (Math.PI*2);
            if (angle < 0.0f)
                angle += pi2;
            if (angle > pi2)
                angle -= pi2;
            _setFacing(Pointer, Helper.PerformanceCount, angle);
        }

        #region Class specific

        private uint RuneState
        {
            get { return Manager.Memory.Read<uint>((IntPtr) Pointers.LocalPlayer.RuneState); }
        }

        public Dictionary<RuneType, int> RunesReady
        {
            get
            {
                var ret = new Dictionary<RuneType, int>();
                for (int i = 0; i < 6; i++)
                {
                    var runeType = Manager.Memory.Read<RuneType>(new IntPtr(Pointers.LocalPlayer.RuneType + i*4));
                    bool runeReady = (RuneState & (1 << i)) > 0;
                    if (runeReady)
                    {
                        if (ret.ContainsKey(runeType))
                            ret[runeType]++;
                        else
                            ret.Add(runeType, 1);
                    }
                }
                return ret;
            }
        }

        public int BloodRunesReady
        {
            get { return RunesReady[RuneType.Blood]; }
        }

        public int UnholyRunesReady
        {
            get { return RunesReady[RuneType.Unholy]; }
        }

        public int FrostRunesReady
        {
            get { return RunesReady[RuneType.Frost]; }
        }

        public int DeathRunesReady
        {
            get { return RunesReady[RuneType.Death]; }
        }

        public bool IsRuneReady(RuneSlot slot)
        {
            return (RuneState & (1 << (int) slot)) > 0;
        }

        public int GetRuneCooldown(RuneSlot slot)
        {
            return Manager.Memory.Read<int>(new IntPtr(Pointers.LocalPlayer.RuneCooldown + (int) slot*4));
        }

        #endregion

        #region Item helpers

        public WoWItem GetBackpackItem(int slot)
        {
            var guid = GetAbsoluteDescriptor<ulong>((int) WoWPlayerFields.PLAYER_FIELD_PACK_SLOT_1*4 + (slot*8));
            return Manager.ObjectManager.GetObjectByGuid(guid) as WoWItem;
        }

        public WoWItem GetBankedItem(int slot)
        {
            var guid = GetAbsoluteDescriptor<ulong>((int) WoWPlayerFields.PLAYER_FIELD_BANK_SLOT_1*4 + (slot*8));
            return Manager.ObjectManager.GetObjectByGuid(guid) as WoWItem;
        }

        public WoWItem GetEquippedItem(EquipSlot slot)
        {
            return GetEquippedItem((int) slot);
        }

        public WoWItem GetEquippedItem(int slot)
        {
            var guid = GetAbsoluteDescriptor<ulong>((int) WoWPlayerFields.PLAYER_FIELD_INV_SLOT_HEAD*4 + (slot*8));
            return Manager.ObjectManager.GetObjectByGuid(guid) as WoWItem;
        }

        public bool CanUseItem(WoWItem item, out GameError error)
        {
            return CanUseItem(WoWItem.GetItemRecordPointerFromId(item.Entry), out error);
        }

        public bool CanUseItem(IntPtr pointer, out GameError error)
        {
            if (_canUseItem == null)
                _canUseItem = Manager.Memory.RegisterDelegate<CanUseItemDelegate>((IntPtr) Pointers.Item.CanUseItem);
            return _canUseItem(Pointer, pointer, out error);
        }

        #endregion

        #region Quest helpers

        public QuestCacheRecord GetQuestRecordFromId(int id)
        {
            if (_getQuestInfoBlockById == null)
                _getQuestInfoBlockById =
                    Manager.Memory.RegisterDelegate<GetQuestInfoBlockByIdDelegate>(
                        (IntPtr) Pointers.WDB.DbQuestCache_GetInfoBlockByID);

            IntPtr recPtr = _getQuestInfoBlockById((IntPtr) Pointers.WDB.WdbQuestCache, id, ref id);
            return Manager.Memory.Read<QuestCacheRecord>(recPtr);
        }

        public QuestCache GetQuestRecord2FromId(int id)
        {
            if (_getQuestInfoBlockById == null)
                _getQuestInfoBlockById =
                    Manager.Memory.RegisterDelegate<GetQuestInfoBlockByIdDelegate>(
                        (IntPtr) Pointers.WDB.DbQuestCache_GetInfoBlockByID);

            IntPtr recPtr = _getQuestInfoBlockById((IntPtr) Pointers.WDB.WdbQuestCache, id, ref id);
            return Manager.Memory.Read<QuestCache>(recPtr);
        }

        #endregion

        #region LUA Helpers

        public void StartAttack()
        {
            WoWScript.Execute("StartAttack()");
        }

        #region Movement

        public void Ascend()
        {
            WoWScript.ExecuteNoResults("JumpOrAscendStart()");
        }

        public void Jump()
        {
            WoWScript.ExecuteNoResults("JumpOrAscendStart()");
        }

        public void Descend()
        {
            WoWScript.ExecuteNoResults("SitStandOrDescendStart()");
        }

        public void MoveBackward()
        {
            WoWScript.ExecuteNoResults("MoveBackwardStart()");
        }

        public void MoveForward()
        {
            WoWScript.ExecuteNoResults("MoveForwardStart()");
        }

        public void StopMoving()
        {
            WoWScript.ExecuteNoResults(
                "AscendStop() DescendStop() MoveBackwardStop() MoveForwardStop() StrafeLeftStop() StrafeRightStop()");
        }

        public void StrafeLeft()
        {
            WoWScript.ExecuteNoResults("StrafeLeftStart()");
        }

        public void StrafeRight()
        {
            WoWScript.ExecuteNoResults("StrafeRightStart()");
        }

        public void Dismount()
        {
            WoWScript.ExecuteNoResults("Dismount()");
        }

        #endregion

        #endregion
    }
}