using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using IceFlake.DirectX;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace IceFlake
{
    public partial class IceForm : Form, ILog
    {
        public IceForm()
        {
            InitializeComponent();
        }

        #region ILog Members

        public void WriteLine(LogEntry entry)
        {
            Color logColor;
            switch (entry.Type)
            {
                case LogType.Error:
                    logColor = Color.Red;
                    break;
                case LogType.Warning:
                    logColor = Color.OrangeRed;
                    break;
                case LogType.Information:
                    logColor = Color.SteelBlue;
                    break;
                case LogType.Good:
                    logColor = Color.ForestGreen;
                    break;
                case LogType.Normal:
                default:
                    logColor = Color.Black;
                    break;
            }

            rbLogBox.Invoke((Action) (() =>
            {
                rbLogBox.SelectionColor = logColor;
                rbLogBox.AppendText(entry.FormattedMessage + Environment.NewLine);
                rbLogBox.ScrollToCaret();
            }));
        }

        #endregion

        private void IceForm_Load(object sender, EventArgs e)
        {
            Log.AddReader(this);
            Log.WriteLine(LogType.Good, "CoreForm loaded");
        }

        private void IceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.RemoveReader(this);

            foreach (Script s in Manager.Scripts.Scripts.Where(x => x.IsRunning))
                s.Stop();

            // Let's give us a chance to undo some damage.
            Direct3D.Shutdown();
        }

        private void GUITimer_Tick(object sender, EventArgs e)
        {
            if (lstScripts.Items.Count == 0)
                if (Manager.Scripts != null)
                    SetupScripts();

            if (Manager.Scripts.Scripts.Where(s => s.IsRunning).Contains(SelectedScript))
            {
                btnScriptStart.Enabled = false;
                btnScriptStop.Enabled = true;
            }
            else
            {
                btnScriptStart.Enabled = true;
                btnScriptStop.Enabled = false;
            }

            if (!Manager.ObjectManager.IsInGame)
                return;

            try
            {
                WoWLocalPlayer lp = Manager.LocalPlayer;
                lblHealth.Text = string.Format("{0}/{1} ({2:0}%)", lp.Health, lp.MaxHealth, lp.HealthPercentage);
                lblPowerText.Text = string.Format("{0}:", lp.PowerType);
                lblPower.Text = string.Format("{0}/{1} ({2:0}%)", lp.Power, lp.MaxPower, lp.PowerPercentage);
                lblLevel.Text = string.Format("{0}", lp.Level);
                lblZone.Text = string.Format("{0} ({1})", WoWWorld.CurrentZone ?? "<unknown>",
                    WoWWorld.CurrentSubZone ?? "<unknown>");
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var dbg = new IceDebug();
            //dbg.Show();
        }

        #region Debug tab

        private Location
            _pos1 = default(Location),
            _pos2 = default(Location);

        private void btnExecute_Click(object sender, EventArgs e)
        {
            string lua = tbLUA.Text;
            if (string.IsNullOrEmpty(lua))
                return;
            Manager.ExecutionQueue.AddExececution(() =>
            {
                Log.WriteLine(lua);
                List<string> ret = WoWScript.Execute(lua);
                for (int i = 0; i < ret.Count; i++)
                    Log.WriteLine("\t[{0}] = \"{1}\"", i, ret[i]);
            });
        }

        private void btnSpellCast_Click(object sender, EventArgs e)
        {
            try
            {
                if (Manager.LocalPlayer.Class != WoWClass.Shaman)
                    return;
                WoWSpell healingWave = Manager.Spellbook["Healing Wave"];
                if (healingWave == null || !healingWave.IsValid)
                    return;
                Manager.ExecutionQueue.AddExececution(() => { healingWave.Cast(); });
            }
            catch
            {
            }
        }

        private void lblPos1_Click(object sender, EventArgs e)
        {
            if (!Manager.ObjectManager.IsInGame)
                return;
            _pos1 = Manager.LocalPlayer.Location;
            lblPos1.Text = _pos1.ToString();
        }

        private void lblPos2_Click(object sender, EventArgs e)
        {
            if (!Manager.ObjectManager.IsInGame)
                return;
            _pos2 = Manager.LocalPlayer.Location;
            lblPos2.Text = _pos2.ToString();
        }

        private void btnGenPath_Click(object sender, EventArgs e)
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            if (_pos1 == default(Location) || _pos2 == default(Location))
                return;

            try
            {
                string map = WoWWorld.CurrentMap;
                Log.WriteLine("Generate path from {0} to {1} in {2}", _pos1, _pos2, map);
                var mesh = new Pather("Kalimdor");
                mesh.LoadAppropriateTiles(_pos1.ToVector3(), _pos2.ToVector3());
                List<Vector3> path = mesh.DetourMesh.FindPath(_pos1.ToFloatArray(), _pos2.ToFloatArray(), false);
                foreach (Vector3 point in path)
                    Log.WriteLine("[{0}]", point.ToString());
            }
            catch (Exception ex)
            {
                Log.WriteLine("NavMesh: {0}", ex.Message);
            }
        }

        private void btnLoSTest_Click(object sender, EventArgs e)
        {
            const uint flags = 0x120171;

            WoWLocalPlayer me = Manager.LocalPlayer;
            if (me == null || !me.IsValid)
                return;
            Location start = me.Location;

            WoWObject target = me.Target;
            if (target == null || !target.IsValid)
                return;
            Location end = target.Location;

            start.Z += 1.3f;
            end.Z += 1.3f;

            Manager.ExecutionQueue.AddExececution(() =>
            {
                Location result;
                bool los = (WoWWorld.Traceline(start, end, out result, flags) & 0xFF) == 0;
                Log.WriteLine("LoSTest: {0} -> {1} = {2} @ {3}", me.Location, target.Location, los, result);
            });
        }

        #endregion

        #region Scripts tab

        private Script SelectedScript;

        private void SetupScripts()
        {
            Manager.Scripts.ScriptRegistered += OnScriptRegisteredEvent;

            foreach (Script s in Manager.Scripts.Scripts)
            {
                s.OnStartedEvent += OnScriptStartedEvent;
                s.OnStoppedEvent += OnScriptStoppedEvent;
            }

            lstScripts.DataSource = Manager.Scripts.Scripts.OrderBy(x => x.Category).ToList();
            Log.WriteLine(LogType.Information, "Loaded {0} scripts.", Manager.Scripts.Scripts.Count);
        }

        private void btnScriptStart_Click(object sender, EventArgs e)
        {
            if (SelectedScript == null)
                return;

            SelectedScript.Start();
        }

        private void btnScriptStop_Click(object sender, EventArgs e)
        {
            if (SelectedScript == null)
                return;

            SelectedScript.Stop();
        }

        private void btnScriptCompile_Click(object sender, EventArgs e)
        {
            Manager.Scripts.CompileAsync();
        }

        private void lstScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var script = lstScripts.SelectedItem as Script;
            if (script == null)
                return;

            SelectedScript = script;
        }

        private void lstScripts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var script = lstScripts.Items[e.Index] as Script;
            if (e.NewValue == CheckState.Checked)
                script.Start();
            else if (e.NewValue == CheckState.Unchecked)
                script.Stop();
        }

        private void OnScriptRegisteredEvent(object sender, EventArgs e)
        {
            var script = sender as Script;
            script.OnStartedEvent += OnScriptStartedEvent;
            script.OnStoppedEvent += OnScriptStoppedEvent;
            lstScripts.Invoke((Action) (() =>
            {
                lstScripts.DataSource = Manager.Scripts.Scripts.OrderBy(x => x.Category).ToList();
                lstScripts.Invalidate();
            }));
        }

        private void OnScriptStartedEvent(object sender, EventArgs e)
        {
            int idx = lstScripts.Items.IndexOf(sender);
            lstScripts.Invoke((Action) (() => lstScripts.SetItemCheckState(idx, CheckState.Checked)));
        }

        private void OnScriptStoppedEvent(object sender, EventArgs e)
        {
            int idx = lstScripts.Items.IndexOf(sender);
            lstScripts.Invoke((Action) (() => lstScripts.SetItemCheckState(idx, CheckState.Unchecked)));
        }

        #endregion
    }
}