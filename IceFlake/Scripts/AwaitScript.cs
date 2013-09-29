using IceFlake.Client.Scripts;
using IceFlake.Runtime;
using System.Threading.Tasks;

namespace IceFlake.Scripts
{
    public class AwaitScript : Script
    {

        private PulseSynchronizer synchronizer;

        public AwaitScript()
            : base("Test", "Behold the Awaiter")
        {
            synchronizer = new PulseSynchronizer();
        }

        public override void OnStart()
        {
            Task task = null;

            Print("---- Before execute");
            synchronizer.Execute(() => task = DoStuff());
            Print("---- After execute");

            for (var i = 0; i < 5; i++)
            {
                Print("---- Before step run");
                synchronizer.Run();
                Print("---- After step run");
            }
        }

        private async Task DoStuff()
        {
            Print("Before delay 1000");
            await Task.Delay(1000);
            Print("After delay 1000");

            Print("Before tasklet");
            var tasklet = new Tasklet("Tasklet");
            await tasklet;
            Print("After tasklet");

            Print("Before delay 5000");
            await Task.Delay(5000);
            Print("After delay 5000");

            Print("Before DoMoreStuff");
            await DoMoreStuff();
            Print("After DoMoreStuff");
        }

        private async Task DoMoreStuff()
        {
            Print("Before tasklet 1");
            var tasklet1 = new Tasklet("Tasklet 1");
            await tasklet1;
            Print("After tasklet 1");

            Print("Yield");
            await Task.Yield();

            Print("Yield");
            await Task.Yield();

            Print("Yield");
            await Task.Yield();

            Print("Yield");
            await Task.Yield();

            Print("Yield");
            await Task.Yield();

            Print("Before tasklet 2");
            var tasklet2 = new Tasklet("Tasklet 2");
            await tasklet2;
            Print("After tasklet 2");
        }
    }
}
