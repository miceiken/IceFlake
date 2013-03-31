namespace IceFlake.Client.Patchables
{
    internal static class Pointers
    {
        #region Nested type: CVar

        internal class CVar
        {
            internal static uint LookupRegistered = 0x6CA80; //@
            internal static uint Set = 0x6BF50; //@
        }

        #endregion

        #region Nested type: Container

        internal class Container
        {
            internal static uint EquippedBagGUID = 0xCDBF40; //@
            internal static uint GetBagAtIndex = 0x005D6F20;
            internal static uint LootWindowOffset = 0x00BFA8D8;
        }

        #endregion

        #region Nested type: Drawing

        internal class Drawing
        {
            internal static uint WorldFrame = 0x00B7436C;
            internal static uint ActiveCamera = 0x7E20;
            internal static uint AspectRatio = 0xAD743C;
            internal static uint GetFarClip = 0x6EBC50; //@
            internal static uint RenderBackground = 0x2532E0;
        }

        #endregion

        #region Nested type: Events

        internal class Events
        {
            internal static uint EventVictim = 0x563DD0; //@
        }

        #endregion

        #region Nested type: Item

        internal class Item
        {
            internal static uint UseItem = 0x47CD10; //@  
            internal static uint GetItemSparseRecPtr = 0x47B700; //@ 0x47CDC0;

            //New
            internal static uint GetClass = 0x47B250;
            internal static uint GetSubclass = 0x47B290;
            internal static uint InventoryType = 0x47B2D0;
        }

        #endregion

        #region Nested type: LocalPlayer

        internal class LocalPlayer
        {
            internal static uint ClickToMove = 0x00727400;
            internal static uint SetFacing = 0x004F42A0;
            internal static uint IsClickMoving = 0x00721F90;
            internal static uint StopCTM = 0x0072B3A0; 
            internal static uint GetRuneReadyBySlot = 0x629940; //@
            internal static uint CorpsePosition = 0x0051F430;
            internal static uint ComboPoints = 0x00BD084D;

            internal static uint CurrentSpeed = 0x850;
            internal static uint SwimSpeed = 0x810;
            internal static uint RunSpeed = 0x808;
            internal static uint FlySpeed = 0x818;

            internal static uint Height = 0x8A0;
            internal static uint Width = 0x84C;

            internal static uint Stepheight = 0x854;

            internal static uint RotationSpeed = 0x820;

            internal static uint IsSwimming = 0x38;

            internal static uint BackbackSlot1 = 0x4A30;
        }

        #endregion

        #region Nested type: LuaInterface

        // 3.3.5a: 12340
        internal class LuaInterface
        {
            internal static uint LuaState = 0x00D3F78C;
            internal static uint LuaLoadBuffer = 0x0084F860;
            internal static uint LuaPCall = 0x0084EC50;
            internal static uint LuaGetTop = 0x0084DBD0;
            internal static uint LuaSetTop = 0x0084DBF0;
            internal static uint LuaType = 0x11D170; //@
            internal static uint LuaToNumber = 0x0084E030;
            internal static uint LuaToLString = 0x0084E0E0;
            internal static uint LuaToBoolean = 0x0044E2C0;
        };

        #endregion

        #region Nested type: Object

        // 3.3.5a: 12340
        internal class Object
        {
            internal static uint GetObjectName = 54;
            internal static uint GetObjectLocation = 12;
            internal static uint GetObjectFacing = 14;
            internal static uint Interact = 44;
            internal static uint SelectObject = 0x00524BF0;
        }

        #endregion

        #region Nested type: DBC

        // 3.3.5a: 12340
        internal class DBC
        {
            internal static uint RegisterBase = 0x006337D0;
            internal static uint GetRow = 0x004BB1C0;
            internal static uint GetLocalizedRow = 0x004CFD20;
        }

        #endregion

        #region Nested type: ObjectManager

        // 3.3.5a: 12340
        internal class ObjectManager
        {
            internal static uint
                EnumVisibleObjects = 0x004D4B30,
                GetObjectByGuid = 0x004D4DB0,
                GetLocalPlayerGuid = 0x004D3790;
        }

        #endregion

        #region Nested type: Other

        internal class Other
        {
            internal static uint PerformanceCounter = 0x0086AE20; //?
            internal static uint LastHardwareAction = 0x00B499A4;
            internal static uint IsBobbing = 0xBC;
            internal static uint GameState = 0xC6BBDE; //@
            internal static uint WardenBase = 0x00BBD344; //@
            internal static uint IsLoading = 0xCC6FF0; //@
            internal static uint RealmName = 0xDC9766; //@
            // return ( InstanceDifficulty* )0x00C4EC2C;
            internal static uint InstanceDifficulty = 0x00C4EC2C;
        }

        #endregion

        #region Nested type: Party

        // 3.3.5a: 12340
        internal class Party
        {
            internal static uint PartyArray = 0x00BD1948; // 0x00C4FCC8?
        }

        #endregion

        #region Nested type: Raid

        // 3.3.5a: 12340
        internal class Raid
        {
            //return ( int* )0x00C543E0;
            internal static uint RaidCount = 0x00C543E0;
            // return ( ulong** )0x00C54340;
            internal static uint RaidArray = 0x00C54340;
        }

        #endregion

        #region Nested type: Spell

        // 3.3.5a: 12340
        internal class Spell
        {
            internal static uint SpellCount = 0x00BE8D9C;
            internal static uint SpellBook = 0x00BE5D88;
            internal static uint CastSpell = 0x0080DA40;
            internal static uint GetSpellCooldown = 0x00807980;
            internal static uint GetSpellEffectRec = 0x953B0; // @
        }

        #endregion

        #region Nested type: Unit

        // 3.3.5a: 12340
        internal class Unit
        {
            internal static uint FishChanneledCasting = 0xD70;
            internal static uint ChanneledCastingId = 0xC20;
            internal static uint CastingId = 0xc08;
            internal static uint UnitReaction = 0x007251C0;
            internal static uint HasAuraBySpellId = 0x007282A0;
            internal static uint GetAura = 0x00556E10;
            internal static uint GetAuraCount = 0x004F8850;
            internal static uint GetCreatureType = 0x0071F300;
            internal static uint ShapeshiftFormId = 0x0071AF70;
            internal static uint CalculateThreat = 0x007374C0;
        }

        #endregion

        #region Nested type: World

        internal class World
        {
            internal static uint Traceline = 0x007A3B70;
            internal static uint CurrentMapId = 0xC6BCFC; //@
            internal static uint ContinentID = 0xA724AC; //@
            internal static uint ZoneID = 0x00BD080C;
        }

        #endregion
    }
}