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
            this.grbx2 = new System.Windows.Forms.GroupBox();
            this.chBoxRandFightRes = new System.Windows.Forms.CheckBox();
            this.chBoxRandConOfLoc = new System.Windows.Forms.CheckBox();
            this.chBoxRandDistOfInit = new System.Windows.Forms.CheckBox();
            this.txtBoxSeed = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chBoxHideNTDActions = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grbx3 = new System.Windows.Forms.GroupBox();
            this.rbtn3 = new System.Windows.Forms.RadioButton();
            this.rbtn4 = new System.Windows.Forms.RadioButton();
            this.grbx1.SuspendLayout();
            this.grbx2.SuspendLayout();
            this.grbx3.SuspendLayout();
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
            this.grbx1.Text = "Setting:";
            // 
            // chBoxRandEnc
            // 
            this.chBoxRandEnc.AutoSize = true;
            this.chBoxRandEnc.Enabled = false;
            this.chBoxRandEnc.Location = new System.Drawing.Point(20, 75);
            this.chBoxRandEnc.Name = "chBoxRandEnc";
            this.chBoxRandEnc.Size = new System.Drawing.Size(159, 21);
            this.chBoxRandEnc.TabIndex = 5;
            this.chBoxRandEnc.Text = "Random Encounters";
            this.chBoxRandEnc.UseVisualStyleBackColor = true;
            this.chBoxRandEnc.CheckedChanged += new System.EventHandler(this.chBoxRandEnc_CheckedChanged);
            // 
            // grbx2
            // 
            this.grbx2.Controls.Add(this.label3);
            this.grbx2.Controls.Add(this.label2);
            this.grbx2.Controls.Add(this.chBoxHideNTDActions);
            this.grbx2.Controls.Add(this.label1);
            this.grbx2.Controls.Add(this.txtBoxSeed);
            this.grbx2.Controls.Add(this.chBoxRandDistOfInit);
            this.grbx2.Controls.Add(this.chBoxRandFightRes);
            this.grbx2.Controls.Add(this.chBoxRandConOfLoc);
            this.grbx2.Controls.Add(this.chBoxRandEnc);
            this.grbx2.Location = new System.Drawing.Point(218, 12);
            this.grbx2.Name = "grbx2";
            this.grbx2.Size = new System.Drawing.Size(424, 206);
            this.grbx2.TabIndex = 6;
            this.grbx2.TabStop = false;
            this.grbx2.Text = "Settings:";
            // 
            // chBoxRandFightRes
            // 
            this.chBoxRandFightRes.AutoSize = true;
            this.chBoxRandFightRes.Location = new System.Drawing.Point(20, 102);
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
            this.chBoxRandConOfLoc.Location = new System.Drawing.Point(20, 47);
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
            this.chBoxRandDistOfInit.Location = new System.Drawing.Point(20, 129);
            this.chBoxRandDistOfInit.Name = "chBoxRandDistOfInit";
            this.chBoxRandDistOfInit.Size = new System.Drawing.Size(227, 21);
            this.chBoxRandDistOfInit.TabIndex = 7;
            this.chBoxRandDistOfInit.Text = "Random distribution of initiative";
            this.chBoxRandDistOfInit.UseVisualStyleBackColor = true;
            this.chBoxRandDistOfInit.CheckedChanged += new System.EventHandler(this.chBoxRandDistOfInit_CheckedChanged);
            // 
            // txtBoxSeed
            // 
            this.txtBoxSeed.Location = new System.Drawing.Point(282, 44);
            this.txtBoxSeed.Name = "txtBoxSeed";
            this.txtBoxSeed.Size = new System.Drawing.Size(49, 22);
            this.txtBoxSeed.TabIndex = 7;
            this.txtBoxSeed.Text = "0";
            this.txtBoxSeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Seed:";
            // 
            // chBoxHideNTDActions
            // 
            this.chBoxHideNTDActions.AutoSize = true;
            this.chBoxHideNTDActions.Location = new System.Drawing.Point(20, 156);
            this.chBoxHideNTDActions.Name = "chBoxHideNTDActions";
            this.chBoxHideNTDActions.Size = new System.Drawing.Size(214, 21);
            this.chBoxHideNTDActions.TabIndex = 9;
            this.chBoxHideNTDActions.Text = "Hide \"Nothing To Do\" actions";
            this.chBoxHideNTDActions.UseVisualStyleBackColor = true;
            this.chBoxHideNTDActions.CheckedChanged += new System.EventHandler(this.chBoxHideNTDActions_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "(";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(337, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = ")";
            // 
            // grbx3
            // 
            this.grbx3.Controls.Add(this.rbtn4);
            this.grbx3.Controls.Add(this.rbtn3);
            this.grbx3.Location = new System.Drawing.Point(12, 118);
            this.grbx3.Name = "grbx3";
            this.grbx3.Size = new System.Drawing.Size(200, 100);
            this.grbx3.TabIndex = 7;
            this.grbx3.TabStop = false;
            this.grbx3.Text = "Sub-Setting:";
            // 
            // rbtn3
            // 
            this.rbtn3.AutoSize = true;
            this.rbtn3.Location = new System.Drawing.Point(6, 32);
            this.rbtn3.Name = "rbtn3";
            this.rbtn3.Size = new System.Drawing.Size(105, 21);
            this.rbtn3.TabIndex = 8;
            this.rbtn3.TabStop = true;
            this.rbtn3.Text = "Dragon Age";
            this.rbtn3.UseVisualStyleBackColor = true;
            // 
            // rbtn4
            // 
            this.rbtn4.AutoSize = true;
            this.rbtn4.Location = new System.Drawing.Point(6, 59);
            this.rbtn4.Name = "rbtn4";
            this.rbtn4.Size = new System.Drawing.Size(133, 21);
            this.rbtn4.TabIndex = 9;
            this.rbtn4.TabStop = true;
            this.rbtn4.Text = "Generic Fantasy";
            this.rbtn4.UseVisualStyleBackColor = true;
            this.rbtn4.CheckedChanged += new System.EventHandler(this.rbtn4_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grbx3);
            this.Controls.Add(this.grbx2);
            this.Controls.Add(this.grbx1);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Main Window";
            this.grbx1.ResumeLayout(false);
            this.grbx1.PerformLayout();
            this.grbx2.ResumeLayout(false);
            this.grbx2.PerformLayout();
            this.grbx3.ResumeLayout(false);
            this.grbx3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RadioButton rbtn1;
        private System.Windows.Forms.RadioButton rbtn2;
        private System.Windows.Forms.GroupBox grbx1;
        private System.Windows.Forms.CheckBox chBoxRandEnc;
        private System.Windows.Forms.GroupBox grbx2;
        private System.Windows.Forms.CheckBox chBoxRandConOfLoc;
        private System.Windows.Forms.CheckBox chBoxRandFightRes;
        private System.Windows.Forms.CheckBox chBoxRandDistOfInit;
        private System.Windows.Forms.TextBox txtBoxSeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chBoxHideNTDActions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grbx3;
        private System.Windows.Forms.RadioButton rbtn4;
        private System.Windows.Forms.RadioButton rbtn3;
    }
}

