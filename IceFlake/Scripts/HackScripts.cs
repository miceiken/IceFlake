using System;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System.Linq;

namespace IceFlake.Scripts
{
    #region SpeedHackScript

    public class SpeedHackScript : Script
    {
        public SpeedHackScript()
            : base("Speed Hack", "Hack")
        { }

        private readonly IntPtr POINTER = new IntPtr(0x6F14A8);

        private const uint START_SPEED = 0x9000E6C1;
        private const uint SPEED_MODIFIER = 4;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(START_SPEED));
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(START_SPEED + ((0x10000 * SPEED_MODIFIER) + 0x1000)));
            Print("Applying speed hack");
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(START_SPEED));
            Print("Removing speed hack");
        }
    }

    #endregion

    #region MorphScaleScript

    public class MorphScaleScript : Script
    {
        public MorphScaleScript()
            : base("Morph & Scale", "Hack")
        { }

        private const bool CHANGE_DISPLAYID = false;
        private const uint MORPH_DISPLAYID = 69;
        private uint DefaultDisplayID = 0;

        private const bool CHANGE_SCALE = false;
        private const float MORPH_SCALE = 1.0f;
        private float DefaultScale = 0f;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            var lp = Manager.LocalPlayer;

            if (CHANGE_DISPLAYID)
            {
                DefaultDisplayID = lp.GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_NATIVEDISPLAYID);
                lp.SetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_DISPLAYID, MORPH_DISPLAYID);
                Print("Changing display ID from {0} to {1}", DefaultDisplayID, MORPH_DISPLAYID);
            }

            if (CHANGE_SCALE)
            {
                DefaultScale = lp.GetDescriptor<float>(WoWObjectFields.OBJECT_FIELD_SCALE_X);
                lp.SetDescriptor<float>(WoWObjectFields.OBJECT_FIELD_SCALE_X, MORPH_SCALE);
                Print("Changing scale from {0:1} to {1:1}", DefaultScale, MORPH_SCALE);
            }

            // TODO: Implement CGUnit_C__UpdateModel VMT
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            var lp = Manager.LocalPlayer;

            if (CHANGE_DISPLAYID)
                lp.SetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_DISPLAYID, DefaultDisplayID);
            if (CHANGE_SCALE)
                lp.SetDescriptor<float>(WoWObjectFields.OBJECT_FIELD_SCALE_X, DefaultScale);

            // TODO: Implement CGUnit_C__UpdateModel VMT
        }
    }

    #endregion
}
