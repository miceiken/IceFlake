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
}
