namespace Beisen.UpaasPortal.ImportTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.logTB = new System.Windows.Forms.TextBox();
            this.tenantTB = new System.Windows.Forms.TextBox();
            this.fieldsTB = new System.Windows.Forms.TextBox();
            this.metaObjectTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.startRepairButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.initBT = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.logTB);
            this.tabPage1.Controls.Add(this.tenantTB);
            this.tabPage1.Controls.Add(this.fieldsTB);
            this.tabPage1.Controls.Add(this.metaObjectTB);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.startRepairButton);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(738, 443);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "数据修复工具";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(19, 245);
            this.logTB.Multiline = true;
            this.logTB.Name = "logTB";
            this.logTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTB.Size = new System.Drawing.Size(397, 166);
            this.logTB.TabIndex = 12;
            // 
            // tenantTB
            // 
            this.tenantTB.Location = new System.Drawing.Point(19, 165);
            this.tenantTB.Multiline = true;
            this.tenantTB.Name = "tenantTB";
            this.tenantTB.Size = new System.Drawing.Size(277, 74);
            this.tenantTB.TabIndex = 10;
            // 
            // fieldsTB
            // 
            this.fieldsTB.AllowDrop = true;
            this.fieldsTB.Location = new System.Drawing.Point(88, 88);
            this.fieldsTB.Name = "fieldsTB";
            this.fieldsTB.Size = new System.Drawing.Size(208, 21);
            this.fieldsTB.TabIndex = 3;
            this.fieldsTB.Text = "UserInfo";
            // 
            // metaObjectTB
            // 
            this.metaObjectTB.AllowDrop = true;
            this.metaObjectTB.Location = new System.Drawing.Point(88, 29);
            this.metaObjectTB.Name = "metaObjectTB";
            this.metaObjectTB.Size = new System.Drawing.Size(208, 21);
            this.metaObjectTB.TabIndex = 0;
            this.metaObjectTB.Text = "AppConnect.UserInfoMapping";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "租户ID（按逗号隔开）：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "选择字段：";
            // 
            // startRepairButton
            // 
            this.startRepairButton.Location = new System.Drawing.Point(642, 6);
            this.startRepairButton.Name = "startRepairButton";
            this.startRepairButton.Size = new System.Drawing.Size(75, 29);
            this.startRepairButton.TabIndex = 2;
            this.startRepairButton.Text = "开始修复";
            this.startRepairButton.UseVisualStyleBackColor = true;
            this.startRepairButton.Click += new System.EventHandler(this.startRepairButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "选择对象：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.initBT);
            this.tabPage3.Controls.Add(this.textBox3);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(738, 443);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Url转换";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // initBT
            // 
            this.initBT.Location = new System.Drawing.Point(540, 174);
            this.initBT.Name = "transBT";
            this.initBT.Size = new System.Drawing.Size(75, 23);
            this.initBT.TabIndex = 20;
            this.initBT.Text = "转换Url";
            this.initBT.UseVisualStyleBackColor = true;
            this.initBT.Click += new System.EventHandler(this.transBT_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(95, 23);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(397, 174);
            this.textBox3.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "原始Url：";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl.Location = new System.Drawing.Point(0, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(746, 469);
            this.tabControl.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(95, 241);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(397, 174);
            this.textBox1.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "转换后Url：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 481);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Appconnect工具";
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox logTB;
        private System.Windows.Forms.TextBox tenantTB;
        private System.Windows.Forms.TextBox fieldsTB;
        private System.Windows.Forms.TextBox metaObjectTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button startRepairButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button initBT;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}

