namespace GithubConnect
{
    partial class Main_Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Window));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最终用户协议EULAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.github仓库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于程序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_CnDisplay = new System.Windows.Forms.Label();
            this.button_Fix = new System.Windows.Forms.Button();
            this.button_CheckCn = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(414, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.最终用户协议EULAToolStripMenuItem,
            this.github仓库ToolStripMenuItem,
            this.关于程序ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 最终用户协议EULAToolStripMenuItem
            // 
            this.最终用户协议EULAToolStripMenuItem.Name = "最终用户协议EULAToolStripMenuItem";
            this.最终用户协议EULAToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.最终用户协议EULAToolStripMenuItem.Text = "最终用户协议(EULA)";
            this.最终用户协议EULAToolStripMenuItem.Click += new System.EventHandler(this.最终用户协议EULAToolStripMenuItem_Click);
            // 
            // github仓库ToolStripMenuItem
            // 
            this.github仓库ToolStripMenuItem.Name = "github仓库ToolStripMenuItem";
            this.github仓库ToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.github仓库ToolStripMenuItem.Text = "Github仓库*";
            this.github仓库ToolStripMenuItem.Click += new System.EventHandler(this.github仓库ToolStripMenuItem_Click);
            // 
            // 关于程序ToolStripMenuItem
            // 
            this.关于程序ToolStripMenuItem.Name = "关于程序ToolStripMenuItem";
            this.关于程序ToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.关于程序ToolStripMenuItem.Text = "关于程序";
            this.关于程序ToolStripMenuItem.Click += new System.EventHandler(this.关于程序ToolStripMenuItem_Click);
            // 
            // label_CnDisplay
            // 
            this.label_CnDisplay.AutoSize = true;
            this.label_CnDisplay.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CnDisplay.ForeColor = System.Drawing.Color.Red;
            this.label_CnDisplay.Location = new System.Drawing.Point(100, 25);
            this.label_CnDisplay.Name = "label_CnDisplay";
            this.label_CnDisplay.Size = new System.Drawing.Size(227, 35);
            this.label_CnDisplay.TabIndex = 1;
            this.label_CnDisplay.Text = "连通性:Unknown";
            // 
            // button_Fix
            // 
            this.button_Fix.Location = new System.Drawing.Point(12, 76);
            this.button_Fix.Name = "button_Fix";
            this.button_Fix.Size = new System.Drawing.Size(184, 26);
            this.button_Fix.TabIndex = 2;
            this.button_Fix.Text = "修复连接";
            this.button_Fix.UseVisualStyleBackColor = true;
            this.button_Fix.Click += new System.EventHandler(this.button_Fix_Click);
            // 
            // button_CheckCn
            // 
            this.button_CheckCn.Location = new System.Drawing.Point(218, 76);
            this.button_CheckCn.Name = "button_CheckCn";
            this.button_CheckCn.Size = new System.Drawing.Size(184, 26);
            this.button_CheckCn.TabIndex = 3;
            this.button_CheckCn.Text = "连通性测试";
            this.button_CheckCn.UseVisualStyleBackColor = true;
            this.button_CheckCn.Click += new System.EventHandler(this.button_CheckCn_Click);
            // 
            // Main_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 114);
            this.Controls.Add(this.button_CheckCn);
            this.Controls.Add(this.button_Fix);
            this.Controls.Add(this.label_CnDisplay);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(430, 153);
            this.MinimumSize = new System.Drawing.Size(430, 153);
            this.Name = "Main_Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main_Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Window_FormClosing);
            this.Load += new System.EventHandler(this.Main_Window_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最终用户协议EULAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem github仓库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于程序ToolStripMenuItem;
        private System.Windows.Forms.Label label_CnDisplay;
        private System.Windows.Forms.Button button_Fix;
        private System.Windows.Forms.Button button_CheckCn;
    }
}