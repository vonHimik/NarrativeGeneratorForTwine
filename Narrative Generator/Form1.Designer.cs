namespace Narrative_Generator
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.rbtn1 = new System.Windows.Forms.RadioButton();
            this.rbtn2 = new System.Windows.Forms.RadioButton();
            this.grbx1 = new System.Windows.Forms.GroupBox();
            this.chBoxRandEnc = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chBoxRandFightRes = new System.Windows.Forms.CheckBox();
            this.chBoxRandConOfLoc = new System.Windows.Forms.CheckBox();
            this.chBoxRandDistOfInit = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chBoxHideNTDActions = new System.Windows.Forms.CheckBox();
            this.grbx1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(306, 360);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // rbtn1
            // 
            this.rbtn1.AutoSize = true;
            this.rbtn1.Location = new System.Drawing.Point(6, 30);
            this.rbtn1.Name = "rbtn1";
            this.rbtn1.Size = new System.Drawing.Size(79, 21);
            this.rbtn1.TabIndex = 2;
            this.rbtn1.TabStop = true;
            this.rbtn1.Text = "Fantasy";
            this.rbtn1.UseVisualStyleBackColor = true;
            this.rbtn1.CheckedChanged += new System.EventHandler(this.rbtn1_CheckedChanged);
            // 
            // rbtn2
            // 
            this.rbtn2.AutoSize = true;
            this.rbtn2.Location = new System.Drawing.Point(6, 57);
            this.rbtn2.Name = "rbtn2";
            this.rbtn2.Size = new System.Drawing.Size(88, 21);
            this.rbtn2.TabIndex = 3;
            this.rbtn2.TabStop = true;
            this.rbtn2.Text = "Detective";
            this.rbtn2.UseVisualStyleBackColor = true;
            this.rbtn2.CheckedChanged += new System.EventHandler(this.rbtn2_CheckedChanged);
            // 
            // grbx1
            // 
            this.grbx1.Controls.Add(this.rbtn1);
            this.grbx1.Controls.Add(this.rbtn2);
            this.grbx1.Location = new System.Drawing.Point(12, 12);
            this.grbx1.Name = "grbx1";
            this.grbx1.Size = new System.Drawing.Size(200, 100);
            this.grbx1.TabIndex = 4;
            this.grbx1.TabStop = false;
            this.grbx1.Text = "Setting";
            // 
            // chBoxRandEnc
            // 
            this.chBoxRandEnc.AutoSize = true;
            this.chBoxRandEnc.Enabled = false;
            this.chBoxRandEnc.Location = new System.Drawing.Point(20, 125);
            this.chBoxRandEnc.Name = "chBoxRandEnc";
            this.chBoxRandEnc.Size = new System.Drawing.Size(159, 21);
            this.chBoxRandEnc.TabIndex = 5;
            this.chBoxRandEnc.Text = "Random Encounters";
            this.chBoxRandEnc.UseVisualStyleBackColor = true;
            this.chBoxRandEnc.CheckedChanged += new System.EventHandler(this.chBoxRandEnc_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chBoxHideNTDActions);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.chBoxRandDistOfInit);
            this.groupBox1.Controls.Add(this.chBoxRandFightRes);
            this.groupBox1.Controls.Add(this.chBoxRandConOfLoc);
            this.groupBox1.Controls.Add(this.chBoxRandEnc);
            this.groupBox1.Location = new System.Drawing.Point(218, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 235);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // chBoxRandFightRes
            // 
            this.chBoxRandFightRes.AutoSize = true;
            this.chBoxRandFightRes.Location = new System.Drawing.Point(20, 152);
            this.chBoxRandFightRes.Name = "chBoxRandFightRes";
            this.chBoxRandFightRes.Size = new System.Drawing.Size(175, 21);
            this.chBoxRandFightRes.TabIndex = 7;
            this.chBoxRandFightRes.Text = "Random battles results";
            this.chBoxRandFightRes.UseVisualStyleBackColor = true;
            this.chBoxRandFightRes.CheckedChanged += new System.EventHandler(this.chBoxRandFightRes_CheckedChanged);
            // 
            // chBoxRandConOfLoc
            // 
            this.chBoxRandConOfLoc.AutoSize = true;
            this.chBoxRandConOfLoc.Location = new System.Drawing.Point(20, 30);
            this.chBoxRandConOfLoc.Name = "chBoxRandConOfLoc";
            this.chBoxRandConOfLoc.Size = new System.Drawing.Size(232, 21);
            this.chBoxRandConOfLoc.TabIndex = 7;
            this.chBoxRandConOfLoc.Text = "Random connection of locations";
            this.chBoxRandConOfLoc.UseVisualStyleBackColor = true;
            this.chBoxRandConOfLoc.CheckedChanged += new System.EventHandler(this.chBoxRandConOfLoc_CheckedChanged);
            // 
            // chBoxRandDistOfInit
            // 
            this.chBoxRandDistOfInit.AutoSize = true;
            this.chBoxRandDistOfInit.Location = new System.Drawing.Point(20, 179);
            this.chBoxRandDistOfInit.Name = "chBoxRandDistOfInit";
            this.chBoxRandDistOfInit.Size = new System.Drawing.Size(227, 21);
            this.chBoxRandDistOfInit.TabIndex = 7;
            this.chBoxRandDistOfInit.Text = "Random distribution of initiative";
            this.chBoxRandDistOfInit.UseVisualStyleBackColor = true;
            this.chBoxRandDistOfInit.CheckedChanged += new System.EventHandler(this.chBoxRandDistOfInit_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(20, 86);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Seed:";
            // 
            // chBoxHideNTDActions
            // 
            this.chBoxHideNTDActions.AutoSize = true;
            this.chBoxHideNTDActions.Location = new System.Drawing.Point(20, 206);
            this.chBoxHideNTDActions.Name = "chBoxHideNTDActions";
            this.chBoxHideNTDActions.Size = new System.Drawing.Size(214, 21);
            this.chBoxHideNTDActions.TabIndex = 9;
            this.chBoxHideNTDActions.Text = "Hide \"Nothing To Do\" actions";
            this.chBoxHideNTDActions.UseVisualStyleBackColor = true;
            this.chBoxHideNTDActions.CheckedChanged += new System.EventHandler(this.chBoxHideNTDActions_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbx1);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Main Window";
            this.grbx1.ResumeLayout(false);
            this.grbx1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RadioButton rbtn1;
        private System.Windows.Forms.RadioButton rbtn2;
        private System.Windows.Forms.GroupBox grbx1;
        private System.Windows.Forms.CheckBox chBoxRandEnc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chBoxRandConOfLoc;
        private System.Windows.Forms.CheckBox chBoxRandFightRes;
        private System.Windows.Forms.CheckBox chBoxRandDistOfInit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chBoxHideNTDActions;
    }
}

