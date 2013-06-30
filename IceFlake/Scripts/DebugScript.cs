using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Routines;
using IceFlake.Client.Scripts;
using IceFlake.Routines;
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using D3D = IceFlake.DirectX.Direct3D;

namespace IceFlake.Scripts
{
    public class DebugScript : Script
    {
        public DebugScript()
            : base("Debug", "Script")
        {
        }

        private Location loc1 = default(Location);
        private Location loc2 = default(Location);

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (loc1 == default(Location))
            {
                loc1 = Manager.LocalPlayer.Location;
                Print("loc1: {0}", loc1);
                Stop();
                return;
            }

            if (loc2 == default(Location))
            {
                loc2 = Manager.LocalPlayer.Location;
                Print("loc2: {0}", loc2);
            }

            //Print("Death Knight Runes:");
            //var runeStates = Manager.Memory.Read<uint>((IntPtr)Pointers.LocalPlayer.RuneState);
            //for (var i = 0; i < 6; i++)
            //{
            //    var runeType = Manager.Memory.Read<int>(new IntPtr(Pointers.LocalPlayer.RuneType + i * 4));
            //    var runeReady = (runeStates & (1 << i)) > 0;
            //    var runeCooldown = Manager.Memory.Read<int>(new IntPtr(Pointers.LocalPlayer.RuneCooldown + i * 4));
            //    Print("\t {0}: {1} {2}", (RuneType)runeType, runeReady ? "Ready" : "Not ready", runeCooldown);
            //}
        }

        private IEnumerable<Vector3> _path = null;
        public override void OnTick()
        {
            if (Manager.Movement.PatherInstance == null)
                Manager.Movement.PatherInstance = new Pather(WoWWorld.CurrentMap);

            if (_path == null || _path.Count() == 0)
                _path = Manager.Movement.PatherInstance.FindPath(loc1, loc2, false);

            var lastPt = _path.First() + new Vector3(0, 0, 1f);
            foreach (var pt in _path.Skip(1))
            {
                var currPt = pt + new Vector3(0,0,1f);
                DrawLine(lastPt, currPt, Color.Blue);
                lastPt = currPt;
            }
        }

        public override void OnTerminate()
        {
        }

        private void DrawLine(Vector3 from, Vector3 to, Color color)
        {
            var vertices = new List<PositionColored>();

            vertices.Add(new PositionColored(from, color.ToArgb()));
            vertices.Add(new PositionColored(to, color.ToArgb()));

            var buffer = vertices.ToArray();

            SetTarget(Vector3.Zero);

            D3D.Device.DrawUserPrimitives(PrimitiveType.LineStrip, vertices.Count - 1, buffer);
        }

        private void SetTarget(Vector3 target, float yaw = 0, float pitch = 0, float roll = 0)
        {
            var worldMatrix = Matrix.Translation(target) * Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            D3D.Device.SetTransform(TransformState.World, worldMatrix);
        }
    }
}