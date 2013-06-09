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
            this.rbLogBox = new System.Windows.Forms.RichTextBox();
            this.btnDump = new System.Windows.Forms.Button();
            this.btnGeneratePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbLogBox
            // 
            this.rbLogBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.rbLogBox.Location = new System.Drawing.Point(0, 0);
            this.rbLogBox.Name = "rbLogBox";
            this.rbLogBox.ReadOnly = true;
            this.rbLogBox.Size = new System.Drawing.Size(306, 304);
            this.rbLogBox.TabIndex = 0;
            this.rbLogBox.Text = "";
            // 
            // btnDump
            // 
            this.btnDump.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDump.Location = new System.Drawing.Point(0, 307);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(306, 23);
            this.btnDump.TabIndex = 1;
            this.btnDump.Text = "Dump";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.btnDump_Click);
            // 
            // btnGeneratePath
            // 
            this.btnGeneratePath.Location = new System.Drawing.Point(0, 0);
            this.btnGeneratePath.Name = "btnGeneratePath";
            this.btnGeneratePath.Size = new System.Drawing.Size(75, 23);
            this.btnGeneratePath.TabIndex = 2;
            this.btnGeneratePath.Text = "Path";
            this.btnGeneratePath.UseVisualStyleBackColor = true;
            this.btnGeneratePath.Click += new System.EventHandler(this.btnGeneratePath_Click);
            // 
            // IceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 330);
            this.Controls.Add(this.btnGeneratePath);
            this.Controls.Add(this.btnDump);
            this.Controls.Add(this.rbLogBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IceForm";
            this.ShowIcon = false;
            this.Text = "IceFlake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IceForm_FormClosing);
            this.Load += new System.EventHandler(this.IceForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rbLogBox;
        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.Button btnGeneratePath;
    }
}

