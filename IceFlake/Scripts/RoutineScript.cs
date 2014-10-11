using IceFlake.Client.Patchables;
using IceFlake.Client.Routines;
using IceFlake.Client.Scripts;
using IceFlake.Routines;

namespace IceFlake.Scripts
{
    public class StarterMageRoutineScript : Script
    {
        private readonly RoutineBrain routine;

        public StarterMageRoutineScript()
            : base("Starter Mage", "Routine")
        {
            routine = new StarterMageRoutine();
        }

        public override void OnStart()
        {
            if (Manager.LocalPlayer.Class != WoWClass.Mage)
                Stop();
            routine.Start();
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            routine.Stop();
        }
    }

    public class ElementalShamanRoutineScript : Script
    {
        private readonly RoutineBrain routine;

        public ElementalShamanRoutineScript()
            : base("Elemental Shaman", "Routine")
        {
            routine = new ElementalShamanRoutine();
        }

        public override void OnStart()
        {
            if (Manager.LocalPlayer.Class != WoWClass.Mage)
                Stop();
            routine.Start();
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            routine.Stop();
        }
    }
}