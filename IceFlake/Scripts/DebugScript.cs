using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Routines;
using IceFlake.Client.Scripts;
using IceFlake.Runtime;
using IceFlake.Routines;
#if SLIMDX
using SlimDX;
using SlimDX.Direct3D9;
#else
using IceFlake.DirectX;
#endif
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

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;
            foreach (var q in Manager.Quests.QuestLog)
            {
                var qcr = Manager.LocalPlayer.GetQuestRecord2FromId(q.ID);
                qcr.DumpProperties();
            }
            Stop();
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
        }

#if SLIMDX
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
#endif
    }
}