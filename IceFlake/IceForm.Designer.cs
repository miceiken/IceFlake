namespace IceFlake
{
    partial class IceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rbLogBox = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabStatus = new System.Windows.Forms.TabPage();
            this.gbPlayer = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPowerText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblHealth = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblZone = new System.Windows.Forms.Label();
            this.tabScripts = new System.Windows.Forms.TabPage();
            this.lstScripts = new System.Windows.Forms.CheckedListBox();
            this.btnScriptCompile = new System.Windows.Forms.Button();
            this.btnScriptStop = new System.Windows.Forms.Button();
            this.btnScriptStart = new System.Windows.Forms.Button();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.btnLoSTest = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tbLUA = new System.Windows.Forms.TextBox();
            this.btnSpellCast = new System.Windows.Forms.Button();
            this.tabPath = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPos1 = new System.Windows.Forms.Label();
            this.lblPos2 = new System.Windows.Forms.Label();
            this.btnGenPath = new System.Windows.Forms.Button();
            this.GUITimer = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.gbPlayer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabScripts.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.tabPath.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbLogBox
            // 
            this.rbLogBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbLogBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbLogBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLogBox.Location = new System.Drawing.Point(0, 213);
            this.rbLogBox.Name = "rbLogBox";
            this.rbLogBox.ReadOnly = true;
            this.rbLogBox.Size = new System.Drawing.Size(361, 298);
            this.rbLogBox.TabIndex = 0;
            this.rbLogBox.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStatus);
            this.tabControl1.Controls.Add(this.tabScripts);
            this.tabControl1.Controls.Add(this.tabDebug);
            this.tabControl1.Controls.Add(this.tabPath);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 213);
            this.tabControl1.TabIndex = 3;
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.gbPlayer);
            this.tabStatus.Location = new System.Drawing.Point(4, 22);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatus.Size = new System.Drawing.Size(353, 187);
            this.tabStatus.TabIndex = 2;
            this.tabStatus.Text = "Status";
            this.tabStatus.UseVisualStyleBackColor = true;
            // 
            // gbPlayer
            // 
            this.gbPlayer.Controls.Add(this.tableLayoutPanel1);
            this.gbPlayer.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbPlayer.Location = new System.Drawing.Point(3, 3);
            this.gbPlayer.Name = "gbPlayer";
            this.gbPlayer.Size = new System.Drawing.Size(184, 181);
            this.gbPlayer.TabIndex = 0;
            this.gbPlayer.TabStop = false;
            this.gbPlayer.Text = "Player";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPowerText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblHealth, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPower, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLevel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblZone, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(178, 162);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Health:";
            // 
            // lblPowerText
            // 
            this.lblPowerText.AutoSize = true;
            this.lblPowerText.Location = new System.Drawing.Point(3, 13);
            this.lblPowerText.Name = "lblPowerText";
            this.lblPowerText.Size = new System.Drawing.Size(40, 13);
            this.lblPowerText.TabIndex = 1;
            this.lblPowerText.Text = "Power:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Level:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Zone:";
            // 
            // lblHealth
            // 
            this.lblHealth.AutoSize = true;
            this.lblHealth.Location = new System.Drawing.Point(50, 0);
            this.lblHealth.Name = "lblHealth";
            this.lblHealth.Size = new System.Drawing.Size(63, 13);
            this.lblHealth.TabIndex = 4;
            this.lblHealth.Text = "<unknown>";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(50, 13);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(63, 13);
            this.lblPower.TabIndex = 5;
            this.lblPower.Text = "<unknown>";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(50, 26);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(63, 13);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "<unknown>";
            // 
            // lblZone
            // 
            this.lblZone.AutoSize = true;
            this.lblZone.Location = new System.Drawing.Point(50, 39);
            this.lblZone.Name = "lblZone";
            this.lblZone.Size = new System.Drawing.Size(63, 13);
            this.lblZone.TabIndex = 7;
            this.lblZone.Text = "<unknown>";
            // 
            // tabScripts
            // 
            this.tabScripts.Controls.Add(this.lstScripts);
            this.tabScripts.Controls.Add(this.btnScriptCompile);
            this.tabScripts.Controls.Add(this.btnScriptStop);
            this.tabScripts.Controls.Add(this.btnScriptStart);
            this.tabScripts.Location = new System.Drawing.Point(4, 22);
            this.tabScripts.Name = "tabScripts";
            this.tabScripts.Padding = new System.Windows.Forms.Padding(3);
            this.tabScripts.Size = new System.Drawing.Size(353, 187);
            this.tabScripts.TabIndex = 0;
            this.tabScripts.Text = "Scripts";
            this.tabScripts.UseVisualStyleBackColor = true;
            // 
            // lstScripts
            // 
            this.lstScripts.FormattingEnabled = true;
            this.lstScripts.Location = new System.Drawing.Point(6, 7);
            this.lstScripts.Name = "lstScripts";
            this.lstScripts.Size = new System.Drawing.Size(339, 139);
            this.lstScripts.TabIndex = 4;
            this.lstScripts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstScripts_ItemCheck);
            this.lstScripts.SelectedIndexChanged += new System.EventHandler(this.lstScripts_SelectedIndexChanged);
            // 
            // btnScriptCompile
            // 
            this.btnScriptCompile.Image = global::IceFlake.Properties.Resources.brick_go;
            this.btnScriptCompile.Location = new System.Drawing.Point(168, 156);
            this.btnScriptCompile.Name = "btnScriptCompile";
            this.btnScriptCompile.Size = new System.Drawing.Size(75, 23);
            this.btnScriptCompile.TabIndex = 3;
            this.btnScriptCompile.Text = "Compile";
            this.btnScriptCompile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScriptCompile.UseVisualStyleBackColor = true;
            this.btnScriptCompile.Click += new System.EventHandler(this.btnScriptCompile_Click);
            // 
            // btnScriptStop
            // 
            this.btnScriptStop.Image = global::IceFlake.Properties.Resources.control_stop_blue;
            this.btnScriptStop.Location = new System.Drawing.Point(87, 156);
            this.btnScriptStop.Name = "btnScriptStop";
            this.btnScriptStop.Size = new System.Drawing.Size(75, 23);
            this.btnScriptStop.TabIndex = 2;
            this.btnScriptStop.Text = "Stop";
            this.btnScriptStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScriptStop.UseVisualStyleBackColor = true;
            this.btnScriptStop.Click += new System.EventHandler(this.btnScriptStop_Click);
            // 
            // btnScriptStart
            // 
            this.btnScriptStart.Image = global::IceFlake.Properties.Resources.control_play_blue;
            this.btnScriptStart.Location = new System.Drawing.Point(6, 156);
            this.btnScriptStart.Name = "btnScriptStart";
            this.btnScriptStart.Size = new System.Drawing.Size(75, 23);
            this.btnScriptStart.TabIndex = 1;
            this.btnScriptStart.Text = "Start";
            this.btnScriptStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScriptStart.UseVisualStyleBackColor = true;
            this.btnScriptStart.Click += new System.EventHandler(this.btnScriptStart_Click);
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.button1);
            this.tabDebug.Controls.Add(this.btnLoSTest);
            this.tabDebug.Controls.Add(this.btnExecute);
            this.tabDebug.Controls.Add(this.tbLUA);
            this.tabDebug.Controls.Add(this.btnSpellCast);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebug.Size = new System.Drawing.Size(353, 187);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // btnLoSTest
            // 
            this.btnLoSTest.Location = new System.Drawing.Point(8, 36);
            this.btnLoSTest.Name = "btnLoSTest";
            this.btnLoSTest.Size = new System.Drawing.Size(75, 23);
            this.btnLoSTest.TabIndex = 3;
            this.btnLoSTest.Text = "LoSTest";
            this.btnLoSTest.UseVisualStyleBackColor = true;
            this.btnLoSTest.Click += new System.EventHandler(this.btnLoSTest_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(181, 160);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tbLUA
            // 
            this.tbLUA.Location = new System.Drawing.Point(8, 162);
            this.tbLUA.Name = "tbLUA";
            this.tbLUA.Size = new System.Drawing.Size(167, 20);
            this.tbLUA.TabIndex = 1;
            // 
            // btnSpellCast
            // 
            this.btnSpellCast.Location = new System.Drawing.Point(8, 7);
            this.btnSpellCast.Name = "btnSpellCast";
            this.btnSpellCast.Size = new System.Drawing.Size(75, 23);
            this.btnSpellCast.TabIndex = 0;
            this.btnSpellCast.Text = "SpellCast";
            this.btnSpellCast.UseVisualStyleBackColor = true;
            this.btnSpellCast.Click += new System.EventHandler(this.btnSpellCast_Click);
            // 
            // tabPath
            // 
            this.tabPath.Controls.Add(this.groupBox1);
            this.tabPath.Location = new System.Drawing.Point(4, 22);
            this.tabPath.Name = "tabPath";
            this.tabPath.Padding = new System.Windows.Forms.Padding(3);
            this.tabPath.Size = new System.Drawing.Size(353, 187);
            this.tabPath.TabIndex = 3;
            this.tabPath.Text = "Pathing";
            this.tabPath.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Controls.Add(this.btnGenPath);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generate Path";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblPos1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPos2, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(194, 58);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Position 1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Position 2:";
            // 
            // lblPos1
            // 
            this.lblPos1.AutoSize = true;
            this.lblPos1.Location = new System.Drawing.Point(100, 0);
            this.lblPos1.Name = "lblPos1";
            this.lblPos1.Size = new System.Drawing.Size(58, 13);
            this.lblPos1.TabIndex = 2;
            this.lblPos1.Text = "<click me>";
            this.lblPos1.Click += new System.EventHandler(this.lblPos1_Click);
            // 
            // lblPos2
            // 
            this.lblPos2.AutoSize = true;
            this.lblPos2.Location = new System.Drawing.Point(100, 29);
            this.lblPos2.Name = "lblPos2";
            this.lblPos2.Size = new System.Drawing.Size(58, 13);
            this.lblPos2.TabIndex = 3;
            this.lblPos2.Text = "<click me>";
            this.lblPos2.Click += new System.EventHandler(this.lblPos2_Click);
            // 
            // btnGenPath
            // 
            this.btnGenPath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGenPath.Location = new System.Drawing.Point(3, 74);
            this.btnGenPath.Name = "btnGenPath";
            this.btnGenPath.Size = new System.Drawing.Size(194, 23);
            this.btnGenPath.TabIndex = 0;
            this.btnGenPath.Text = "Generate";
            this.btnGenPath.UseVisualStyleBackColor = true;
            this.btnGenPath.Click += new System.EventHandler(this.btnGenPath_Click);
            // 
            // GUITimer
            // 
            this.GUITimer.Enabled = true;
            this.GUITimer.Interval = 500;
            this.GUITimer.Tick += new System.EventHandler(this.GUITimer_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(262, 158);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "DebugWnd...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // IceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 511);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.rbLogBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IceForm";
            this.ShowIcon = false;
            this.Text = "IceFlake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IceForm_FormClosing);
            this.Load += new System.EventHandler(this.IceForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabStatus.ResumeLayout(false);
            this.gbPlayer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabScripts.ResumeLayout(false);
            this.tabDebug.ResumeLayout(false);
            this.tabDebug.PerformLayout();
            this.tabPath.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rbLogBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabScripts;
        private System.Windows.Forms.Button btnScriptCompile;
        private System.Windows.Forms.Button btnScriptStop;
        private System.Windows.Forms.Button btnScriptStart;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.Button btnSpellCast;
        private System.Windows.Forms.CheckedListBox lstScripts;
        private System.Windows.Forms.Timer GUITimer;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox tbLUA;
        private System.Windows.Forms.TabPage tabStatus;
        private System.Windows.Forms.GroupBox gbPlayer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPowerText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblHealth;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblZone;
        private System.Windows.Forms.TabPage tabPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPos1;
        private System.Windows.Forms.Label lblPos2;
        private System.Windows.Forms.Button btnGenPath;
        private System.Windows.Forms.Button btnLoSTest;
        private System.Windows.Forms.Button button1;
    }
}

