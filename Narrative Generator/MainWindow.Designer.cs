namespace Narrative_Generator
{
    partial class MainWindow
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
            this.rbtnFantasy = new System.Windows.Forms.RadioButton();
            this.rbtnDetective = new System.Windows.Forms.RadioButton();
            this.grbxSetting = new System.Windows.Forms.GroupBox();
            this.rbtnDefaultDemo = new System.Windows.Forms.RadioButton();
            this.chBoxRandEnc = new System.Windows.Forms.CheckBox();
            this.grbxSettings = new System.Windows.Forms.GroupBox();
            this.txtBoxNodeCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chBoxHideNTDActions = new System.Windows.Forms.CheckBox();
            this.lblSeed = new System.Windows.Forms.Label();
            this.txtBoxSeed = new System.Windows.Forms.TextBox();
            this.chBoxRandDistOfInit = new System.Windows.Forms.CheckBox();
            this.chBoxRandFightRes = new System.Windows.Forms.CheckBox();
            this.chBoxRandConOfLoc = new System.Windows.Forms.CheckBox();
            this.grbxSubSettingFantasy = new System.Windows.Forms.GroupBox();
            this.rbtnGF = new System.Windows.Forms.RadioButton();
            this.rbtnDA = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chBoxGoalGetItem = new System.Windows.Forms.CheckBox();
            this.chBoxGoalKill = new System.Windows.Forms.CheckBox();
            this.chBoxGoalReachFinLoc = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBoxEvChance = new System.Windows.Forms.TextBox();
            this.lblEvidence = new System.Windows.Forms.Label();
            this.chBoxEachAgentsHasUG = new System.Windows.Forms.CheckBox();
            this.txtProtagonistSurvTime = new System.Windows.Forms.TextBox();
            this.chBoxStrOrdVicSec = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chBoxUniqKills = new System.Windows.Forms.CheckBox();
            this.txtAntagonistSurvTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chBoxAntognstWillSurv = new System.Windows.Forms.CheckBox();
            this.chBoxPrtgWillSurv = new System.Windows.Forms.CheckBox();
            this.chBoxCanFindEvidence = new System.Windows.Forms.CheckBox();
            this.chBoxTA = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnChoiseOutputPath = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chBoxCA = new System.Windows.Forms.CheckBox();
            this.chBoxCE = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chBoxCC = new System.Windows.Forms.CheckBox();
            this.chBoxCP = new System.Windows.Forms.CheckBox();
            this.chBoxAC = new System.Windows.Forms.CheckBox();
            this.chBoxAP = new System.Windows.Forms.CheckBox();
            this.chBoxSC = new System.Windows.Forms.CheckBox();
            this.chBoxSP = new System.Windows.Forms.CheckBox();
            this.chBoxTE = new System.Windows.Forms.CheckBox();
            this.chBoxPE = new System.Windows.Forms.CheckBox();
            this.chBoxPA = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtBoxNotesField = new System.Windows.Forms.TextBox();
            this.grbxSetting.SuspendLayout();
            this.grbxSettings.SuspendLayout();
            this.grbxSubSettingFantasy.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(738, 468);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // rbtnFantasy
            // 
            this.rbtnFantasy.AutoSize = true;
            this.rbtnFantasy.Location = new System.Drawing.Point(6, 30);
            this.rbtnFantasy.Name = "rbtnFantasy";
            this.rbtnFantasy.Size = new System.Drawing.Size(79, 21);
            this.rbtnFantasy.TabIndex = 2;
            this.rbtnFantasy.Text = "Fantasy";
            this.rbtnFantasy.UseVisualStyleBackColor = true;
            this.rbtnFantasy.CheckedChanged += new System.EventHandler(this.RbtnFantasy_CheckedChanged);
            // 
            // rbtnDetective
            // 
            this.rbtnDetective.AutoSize = true;
            this.rbtnDetective.Location = new System.Drawing.Point(6, 57);
            this.rbtnDetective.Name = "rbtnDetective";
            this.rbtnDetective.Size = new System.Drawing.Size(88, 21);
            this.rbtnDetective.TabIndex = 3;
            this.rbtnDetective.Text = "Detective";
            this.rbtnDetective.UseVisualStyleBackColor = true;
            this.rbtnDetective.CheckedChanged += new System.EventHandler(this.RbtnDetective_CheckedChanged);
            // 
            // grbxSetting
            // 
            this.grbxSetting.Controls.Add(this.rbtnDefaultDemo);
            this.grbxSetting.Controls.Add(this.rbtnFantasy);
            this.grbxSetting.Controls.Add(this.rbtnDetective);
            this.grbxSetting.Location = new System.Drawing.Point(12, 12);
            this.grbxSetting.Name = "grbxSetting";
            this.grbxSetting.Size = new System.Drawing.Size(239, 110);
            this.grbxSetting.TabIndex = 4;
            this.grbxSetting.TabStop = false;
            this.grbxSetting.Text = "Setting:";
            // 
            // rbtnDefaultDemo
            // 
            this.rbtnDefaultDemo.AutoSize = true;
            this.rbtnDefaultDemo.Enabled = false;
            this.rbtnDefaultDemo.Location = new System.Drawing.Point(6, 84);
            this.rbtnDefaultDemo.Name = "rbtnDefaultDemo";
            this.rbtnDefaultDemo.Size = new System.Drawing.Size(115, 21);
            this.rbtnDefaultDemo.TabIndex = 8;
            this.rbtnDefaultDemo.TabStop = true;
            this.rbtnDefaultDemo.Text = "Default Demo";
            this.rbtnDefaultDemo.UseVisualStyleBackColor = true;
            this.rbtnDefaultDemo.Visible = false;
            this.rbtnDefaultDemo.CheckedChanged += new System.EventHandler(this.RbtnDefaultDemo_CheckedChanged);
            // 
            // chBoxRandEnc
            // 
            this.chBoxRandEnc.AutoSize = true;
            this.chBoxRandEnc.Enabled = false;
            this.chBoxRandEnc.Location = new System.Drawing.Point(18, 135);
            this.chBoxRandEnc.Name = "chBoxRandEnc";
            this.chBoxRandEnc.Size = new System.Drawing.Size(158, 21);
            this.chBoxRandEnc.TabIndex = 5;
            this.chBoxRandEnc.Text = "Random encounters";
            this.chBoxRandEnc.UseVisualStyleBackColor = true;
            this.chBoxRandEnc.CheckedChanged += new System.EventHandler(this.ChBoxRandEnc_CheckedChanged);
            // 
            // grbxSettings
            // 
            this.grbxSettings.Controls.Add(this.txtBoxNodeCount);
            this.grbxSettings.Controls.Add(this.label3);
            this.grbxSettings.Controls.Add(this.chBoxHideNTDActions);
            this.grbxSettings.Controls.Add(this.lblSeed);
            this.grbxSettings.Controls.Add(this.txtBoxSeed);
            this.grbxSettings.Controls.Add(this.chBoxRandDistOfInit);
            this.grbxSettings.Controls.Add(this.chBoxRandFightRes);
            this.grbxSettings.Controls.Add(this.chBoxRandConOfLoc);
            this.grbxSettings.Controls.Add(this.chBoxRandEnc);
            this.grbxSettings.Location = new System.Drawing.Point(257, 12);
            this.grbxSettings.Name = "grbxSettings";
            this.grbxSettings.Size = new System.Drawing.Size(262, 330);
            this.grbxSettings.TabIndex = 6;
            this.grbxSettings.TabStop = false;
            this.grbxSettings.Text = "System settings:";
            // 
            // txtBoxNodeCount
            // 
            this.txtBoxNodeCount.Location = new System.Drawing.Point(144, 62);
            this.txtBoxNodeCount.Name = "txtBoxNodeCount";
            this.txtBoxNodeCount.Size = new System.Drawing.Size(49, 22);
            this.txtBoxNodeCount.TabIndex = 11;
            this.txtBoxNodeCount.Text = "350";
            this.txtBoxNodeCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxNodeCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBoxNodeCounter_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Node count (max):";
            // 
            // chBoxHideNTDActions
            // 
            this.chBoxHideNTDActions.AutoSize = true;
            this.chBoxHideNTDActions.Location = new System.Drawing.Point(18, 216);
            this.chBoxHideNTDActions.Name = "chBoxHideNTDActions";
            this.chBoxHideNTDActions.Size = new System.Drawing.Size(214, 21);
            this.chBoxHideNTDActions.TabIndex = 9;
            this.chBoxHideNTDActions.Text = "Hide \"Nothing To Do\" actions";
            this.chBoxHideNTDActions.UseVisualStyleBackColor = true;
            this.chBoxHideNTDActions.CheckedChanged += new System.EventHandler(this.ChBoxHideNTDActions_CheckedChanged);
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(15, 32);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(45, 17);
            this.lblSeed.TabIndex = 8;
            this.lblSeed.Text = "Seed:";
            // 
            // txtBoxSeed
            // 
            this.txtBoxSeed.Enabled = false;
            this.txtBoxSeed.Location = new System.Drawing.Point(144, 32);
            this.txtBoxSeed.MaxLength = 4;
            this.txtBoxSeed.Name = "txtBoxSeed";
            this.txtBoxSeed.Size = new System.Drawing.Size(49, 22);
            this.txtBoxSeed.TabIndex = 7;
            this.txtBoxSeed.Text = "0";
            this.txtBoxSeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxSeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBoxSeed_KeyPress);
            // 
            // chBoxRandDistOfInit
            // 
            this.chBoxRandDistOfInit.AutoSize = true;
            this.chBoxRandDistOfInit.Location = new System.Drawing.Point(18, 189);
            this.chBoxRandDistOfInit.Name = "chBoxRandDistOfInit";
            this.chBoxRandDistOfInit.Size = new System.Drawing.Size(227, 21);
            this.chBoxRandDistOfInit.TabIndex = 7;
            this.chBoxRandDistOfInit.Text = "Random distribution of initiative";
            this.chBoxRandDistOfInit.UseVisualStyleBackColor = true;
            this.chBoxRandDistOfInit.CheckedChanged += new System.EventHandler(this.ChBoxRandDistOfInit_CheckedChanged);
            // 
            // chBoxRandFightRes
            // 
            this.chBoxRandFightRes.AutoSize = true;
            this.chBoxRandFightRes.Location = new System.Drawing.Point(18, 162);
            this.chBoxRandFightRes.Name = "chBoxRandFightRes";
            this.chBoxRandFightRes.Size = new System.Drawing.Size(175, 21);
            this.chBoxRandFightRes.TabIndex = 7;
            this.chBoxRandFightRes.Text = "Random battles results";
            this.chBoxRandFightRes.UseVisualStyleBackColor = true;
            this.chBoxRandFightRes.CheckedChanged += new System.EventHandler(this.ChBoxRandFightRes_CheckedChanged);
            // 
            // chBoxRandConOfLoc
            // 
            this.chBoxRandConOfLoc.AutoSize = true;
            this.chBoxRandConOfLoc.Location = new System.Drawing.Point(18, 107);
            this.chBoxRandConOfLoc.Name = "chBoxRandConOfLoc";
            this.chBoxRandConOfLoc.Size = new System.Drawing.Size(232, 21);
            this.chBoxRandConOfLoc.TabIndex = 7;
            this.chBoxRandConOfLoc.Text = "Random connection of locations";
            this.chBoxRandConOfLoc.UseVisualStyleBackColor = true;
            this.chBoxRandConOfLoc.CheckedChanged += new System.EventHandler(this.ChBoxRandConOfLoc_CheckedChanged);
            // 
            // grbxSubSettingFantasy
            // 
            this.grbxSubSettingFantasy.Controls.Add(this.rbtnGF);
            this.grbxSubSettingFantasy.Controls.Add(this.rbtnDA);
            this.grbxSubSettingFantasy.Enabled = false;
            this.grbxSubSettingFantasy.Location = new System.Drawing.Point(12, 128);
            this.grbxSubSettingFantasy.Name = "grbxSubSettingFantasy";
            this.grbxSubSettingFantasy.Size = new System.Drawing.Size(239, 90);
            this.grbxSubSettingFantasy.TabIndex = 7;
            this.grbxSubSettingFantasy.TabStop = false;
            this.grbxSubSettingFantasy.Text = "Sub-Setting:";
            this.grbxSubSettingFantasy.Visible = false;
            // 
            // rbtnGF
            // 
            this.rbtnGF.AutoSize = true;
            this.rbtnGF.Location = new System.Drawing.Point(6, 59);
            this.rbtnGF.Name = "rbtnGF";
            this.rbtnGF.Size = new System.Drawing.Size(133, 21);
            this.rbtnGF.TabIndex = 9;
            this.rbtnGF.TabStop = true;
            this.rbtnGF.Text = "Generic Fantasy";
            this.rbtnGF.UseVisualStyleBackColor = true;
            this.rbtnGF.CheckedChanged += new System.EventHandler(this.RbtnGF_CheckedChanged);
            // 
            // rbtnDA
            // 
            this.rbtnDA.AutoSize = true;
            this.rbtnDA.Location = new System.Drawing.Point(6, 32);
            this.rbtnDA.Name = "rbtnDA";
            this.rbtnDA.Size = new System.Drawing.Size(105, 21);
            this.rbtnDA.TabIndex = 8;
            this.rbtnDA.TabStop = true;
            this.rbtnDA.Text = "Dragon Age";
            this.rbtnDA.UseVisualStyleBackColor = true;
            this.rbtnDA.CheckedChanged += new System.EventHandler(this.RbtnDA_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chBoxGoalGetItem);
            this.groupBox1.Controls.Add(this.chBoxGoalKill);
            this.groupBox1.Controls.Add(this.chBoxGoalReachFinLoc);
            this.groupBox1.Location = new System.Drawing.Point(12, 224);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 118);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Goal-type:";
            // 
            // chBoxGoalGetItem
            // 
            this.chBoxGoalGetItem.AutoSize = true;
            this.chBoxGoalGetItem.Enabled = false;
            this.chBoxGoalGetItem.Location = new System.Drawing.Point(6, 86);
            this.chBoxGoalGetItem.Name = "chBoxGoalGetItem";
            this.chBoxGoalGetItem.Size = new System.Drawing.Size(166, 21);
            this.chBoxGoalGetItem.TabIndex = 11;
            this.chBoxGoalGetItem.Text = "Get an important item";
            this.chBoxGoalGetItem.UseVisualStyleBackColor = true;
            this.chBoxGoalGetItem.Visible = false;
            this.chBoxGoalGetItem.CheckedChanged += new System.EventHandler(this.ChBoxGoalGetItem_CheckedChanged);
            // 
            // chBoxGoalKill
            // 
            this.chBoxGoalKill.AutoSize = true;
            this.chBoxGoalKill.Location = new System.Drawing.Point(6, 32);
            this.chBoxGoalKill.Name = "chBoxGoalKill";
            this.chBoxGoalKill.Size = new System.Drawing.Size(217, 21);
            this.chBoxGoalKill.TabIndex = 9;
            this.chBoxGoalKill.Text = "Kill the antagonist/all enemies";
            this.chBoxGoalKill.UseVisualStyleBackColor = true;
            this.chBoxGoalKill.CheckedChanged += new System.EventHandler(this.ChBoxGoalKill_CheckedChanged);
            // 
            // chBoxGoalReachFinLoc
            // 
            this.chBoxGoalReachFinLoc.AutoSize = true;
            this.chBoxGoalReachFinLoc.Enabled = false;
            this.chBoxGoalReachFinLoc.Location = new System.Drawing.Point(6, 59);
            this.chBoxGoalReachFinLoc.Name = "chBoxGoalReachFinLoc";
            this.chBoxGoalReachFinLoc.Size = new System.Drawing.Size(179, 21);
            this.chBoxGoalReachFinLoc.TabIndex = 10;
            this.chBoxGoalReachFinLoc.Text = "Reach the goal location";
            this.chBoxGoalReachFinLoc.UseVisualStyleBackColor = true;
            this.chBoxGoalReachFinLoc.Visible = false;
            this.chBoxGoalReachFinLoc.CheckedChanged += new System.EventHandler(this.ChBoxGoalReachFinLoc_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtBoxEvChance);
            this.groupBox2.Controls.Add(this.lblEvidence);
            this.groupBox2.Controls.Add(this.chBoxEachAgentsHasUG);
            this.groupBox2.Controls.Add(this.txtProtagonistSurvTime);
            this.groupBox2.Controls.Add(this.chBoxStrOrdVicSec);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chBoxUniqKills);
            this.groupBox2.Controls.Add(this.txtAntagonistSurvTime);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chBoxAntognstWillSurv);
            this.groupBox2.Controls.Add(this.chBoxPrtgWillSurv);
            this.groupBox2.Controls.Add(this.chBoxCanFindEvidence);
            this.groupBox2.Location = new System.Drawing.Point(525, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 330);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Story settings:";
            // 
            // txtBoxEvChance
            // 
            this.txtBoxEvChance.Enabled = false;
            this.txtBoxEvChance.Location = new System.Drawing.Point(251, 37);
            this.txtBoxEvChance.Name = "txtBoxEvChance";
            this.txtBoxEvChance.Size = new System.Drawing.Size(31, 22);
            this.txtBoxEvChance.TabIndex = 16;
            this.txtBoxEvChance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxEvChance_KeyPress);
            // 
            // lblEvidence
            // 
            this.lblEvidence.AutoSize = true;
            this.lblEvidence.Location = new System.Drawing.Point(159, 40);
            this.lblEvidence.Name = "lblEvidence";
            this.lblEvidence.Size = new System.Drawing.Size(86, 17);
            this.lblEvidence.TabIndex = 15;
            this.lblEvidence.Text = "Chance (%):";
            // 
            // chBoxEachAgentsHasUG
            // 
            this.chBoxEachAgentsHasUG.AutoSize = true;
            this.chBoxEachAgentsHasUG.Enabled = false;
            this.chBoxEachAgentsHasUG.Location = new System.Drawing.Point(10, 239);
            this.chBoxEachAgentsHasUG.Name = "chBoxEachAgentsHasUG";
            this.chBoxEachAgentsHasUG.Size = new System.Drawing.Size(261, 21);
            this.chBoxEachAgentsHasUG.TabIndex = 9;
            this.chBoxEachAgentsHasUG.Text = "Each agent has its own unique goals";
            this.chBoxEachAgentsHasUG.UseVisualStyleBackColor = true;
            this.chBoxEachAgentsHasUG.Visible = false;
            this.chBoxEachAgentsHasUG.CheckedChanged += new System.EventHandler(this.chBoxEachAgentsHasUG_CheckedChanged);
            // 
            // txtProtagonistSurvTime
            // 
            this.txtProtagonistSurvTime.Enabled = false;
            this.txtProtagonistSurvTime.Location = new System.Drawing.Point(126, 88);
            this.txtProtagonistSurvTime.Name = "txtProtagonistSurvTime";
            this.txtProtagonistSurvTime.Size = new System.Drawing.Size(27, 22);
            this.txtProtagonistSurvTime.TabIndex = 14;
            this.txtProtagonistSurvTime.Text = "0";
            this.txtProtagonistSurvTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtProtagonistSurvTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtProtagonistSurvTime_KeyPress);
            // 
            // chBoxStrOrdVicSec
            // 
            this.chBoxStrOrdVicSec.AutoSize = true;
            this.chBoxStrOrdVicSec.Location = new System.Drawing.Point(10, 212);
            this.chBoxStrOrdVicSec.Name = "chBoxStrOrdVicSec";
            this.chBoxStrOrdVicSec.Size = new System.Drawing.Size(215, 21);
            this.chBoxStrOrdVicSec.TabIndex = 8;
            this.chBoxStrOrdVicSec.Text = "Strict order of victim selection";
            this.chBoxStrOrdVicSec.UseVisualStyleBackColor = true;
            this.chBoxStrOrdVicSec.CheckedChanged += new System.EventHandler(this.chBoxStrOrdVicSec_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Term of survival:";
            // 
            // chBoxUniqKills
            // 
            this.chBoxUniqKills.AutoSize = true;
            this.chBoxUniqKills.Location = new System.Drawing.Point(10, 185);
            this.chBoxUniqKills.Name = "chBoxUniqKills";
            this.chBoxUniqKills.Size = new System.Drawing.Size(146, 21);
            this.chBoxUniqKills.TabIndex = 7;
            this.chBoxUniqKills.Text = "Unique ways to kill";
            this.chBoxUniqKills.UseVisualStyleBackColor = true;
            this.chBoxUniqKills.CheckedChanged += new System.EventHandler(this.chBoxUniqKills_CheckedChanged);
            // 
            // txtAntagonistSurvTime
            // 
            this.txtAntagonistSurvTime.Enabled = false;
            this.txtAntagonistSurvTime.Location = new System.Drawing.Point(125, 148);
            this.txtAntagonistSurvTime.Name = "txtAntagonistSurvTime";
            this.txtAntagonistSurvTime.Size = new System.Drawing.Size(27, 22);
            this.txtAntagonistSurvTime.TabIndex = 5;
            this.txtAntagonistSurvTime.Text = "0";
            this.txtAntagonistSurvTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAntagonistSurvTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtAntagonistSurvTime_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Term of survival:";
            // 
            // chBoxAntognstWillSurv
            // 
            this.chBoxAntognstWillSurv.AutoSize = true;
            this.chBoxAntognstWillSurv.Location = new System.Drawing.Point(6, 122);
            this.chBoxAntognstWillSurv.Name = "chBoxAntognstWillSurv";
            this.chBoxAntognstWillSurv.Size = new System.Drawing.Size(168, 21);
            this.chBoxAntognstWillSurv.TabIndex = 3;
            this.chBoxAntognstWillSurv.Text = "Antagonist will survive";
            this.chBoxAntognstWillSurv.UseVisualStyleBackColor = true;
            this.chBoxAntognstWillSurv.CheckedChanged += new System.EventHandler(this.chBoxAntognstWillSurv_CheckedChanged);
            // 
            // chBoxPrtgWillSurv
            // 
            this.chBoxPrtgWillSurv.AutoSize = true;
            this.chBoxPrtgWillSurv.Location = new System.Drawing.Point(10, 65);
            this.chBoxPrtgWillSurv.Name = "chBoxPrtgWillSurv";
            this.chBoxPrtgWillSurv.Size = new System.Drawing.Size(173, 21);
            this.chBoxPrtgWillSurv.TabIndex = 2;
            this.chBoxPrtgWillSurv.Text = "Protagonist will survive";
            this.chBoxPrtgWillSurv.UseVisualStyleBackColor = true;
            this.chBoxPrtgWillSurv.CheckedChanged += new System.EventHandler(this.chboxPrtgWillSurv_CheckedChanged);
            // 
            // chBoxCanFindEvidence
            // 
            this.chBoxCanFindEvidence.AutoSize = true;
            this.chBoxCanFindEvidence.Location = new System.Drawing.Point(10, 32);
            this.chBoxCanFindEvidence.Name = "chBoxCanFindEvidence";
            this.chBoxCanFindEvidence.Size = new System.Drawing.Size(143, 21);
            this.chBoxCanFindEvidence.TabIndex = 1;
            this.chBoxCanFindEvidence.Text = "Can find evidence";
            this.chBoxCanFindEvidence.UseVisualStyleBackColor = true;
            this.chBoxCanFindEvidence.CheckedChanged += new System.EventHandler(this.chBoxCanFindEvidence_CheckedChanged);
            // 
            // chBoxTA
            // 
            this.chBoxTA.AutoSize = true;
            this.chBoxTA.Location = new System.Drawing.Point(13, 32);
            this.chBoxTA.Name = "chBoxTA";
            this.chBoxTA.Size = new System.Drawing.Size(158, 21);
            this.chBoxTA.TabIndex = 0;
            this.chBoxTA.Text = "Talkative Antagonist";
            this.chBoxTA.UseVisualStyleBackColor = true;
            this.chBoxTA.CheckedChanged += new System.EventHandler(this.chBoxTA_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnChoiseOutputPath);
            this.groupBox3.Controls.Add(this.txtOutputPath);
            this.groupBox3.Controls.Add(this.lblOutputPath);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(525, 348);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(660, 111);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Files settings:";
            // 
            // btnChoiseOutputPath
            // 
            this.btnChoiseOutputPath.Location = new System.Drawing.Point(425, 72);
            this.btnChoiseOutputPath.Name = "btnChoiseOutputPath";
            this.btnChoiseOutputPath.Size = new System.Drawing.Size(125, 23);
            this.btnChoiseOutputPath.TabIndex = 7;
            this.btnChoiseOutputPath.Text = "Select path";
            this.btnChoiseOutputPath.UseVisualStyleBackColor = true;
            this.btnChoiseOutputPath.Click += new System.EventHandler(this.btnChoiseOutputPath_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(236, 72);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(182, 22);
            this.txtOutputPath.TabIndex = 6;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(143, 72);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(87, 17);
            this.lblOutputPath.TabIndex = 5;
            this.lblOutputPath.Text = "Output path:";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(425, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 22);
            this.button1.TabIndex = 2;
            this.button1.Text = "Select file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(237, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(182, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(102, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scenario-file path:";
            this.label1.Visible = false;
            // 
            // chBoxCA
            // 
            this.chBoxCA.AutoSize = true;
            this.chBoxCA.Checked = true;
            this.chBoxCA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBoxCA.Location = new System.Drawing.Point(13, 59);
            this.chBoxCA.Name = "chBoxCA";
            this.chBoxCA.Size = new System.Drawing.Size(153, 21);
            this.chBoxCA.TabIndex = 2;
            this.chBoxCA.Text = "Cunning Antagonist";
            this.chBoxCA.UseVisualStyleBackColor = true;
            this.chBoxCA.CheckedChanged += new System.EventHandler(this.chBoxCA_CheckedChanged);
            // 
            // chBoxCE
            // 
            this.chBoxCE.AutoSize = true;
            this.chBoxCE.Checked = true;
            this.chBoxCE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBoxCE.Location = new System.Drawing.Point(195, 59);
            this.chBoxCE.Name = "chBoxCE";
            this.chBoxCE.Size = new System.Drawing.Size(140, 21);
            this.chBoxCE.TabIndex = 3;
            this.chBoxCE.Text = "Cunning Enemies";
            this.chBoxCE.UseVisualStyleBackColor = true;
            this.chBoxCE.CheckedChanged += new System.EventHandler(this.chBoxCE_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chBoxCC);
            this.groupBox4.Controls.Add(this.chBoxCP);
            this.groupBox4.Controls.Add(this.chBoxAC);
            this.groupBox4.Controls.Add(this.chBoxAP);
            this.groupBox4.Controls.Add(this.chBoxSC);
            this.groupBox4.Controls.Add(this.chBoxSP);
            this.groupBox4.Controls.Add(this.chBoxTE);
            this.groupBox4.Controls.Add(this.chBoxPE);
            this.groupBox4.Controls.Add(this.chBoxPA);
            this.groupBox4.Controls.Add(this.chBoxCE);
            this.groupBox4.Controls.Add(this.chBoxTA);
            this.groupBox4.Controls.Add(this.chBoxCA);
            this.groupBox4.Location = new System.Drawing.Point(819, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(375, 330);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Character behavior:";
            // 
            // chBoxCC
            // 
            this.chBoxCC.AutoSize = true;
            this.chBoxCC.Location = new System.Drawing.Point(195, 303);
            this.chBoxCC.Name = "chBoxCC";
            this.chBoxCC.Size = new System.Drawing.Size(158, 21);
            this.chBoxCC.TabIndex = 12;
            this.chBoxCC.Text = "Cowardly characters";
            this.chBoxCC.UseVisualStyleBackColor = true;
            this.chBoxCC.CheckedChanged += new System.EventHandler(this.chBoxCC_CheckedChanged);
            // 
            // chBoxCP
            // 
            this.chBoxCP.AutoSize = true;
            this.chBoxCP.Location = new System.Drawing.Point(13, 300);
            this.chBoxCP.Name = "chBoxCP";
            this.chBoxCP.Size = new System.Drawing.Size(163, 21);
            this.chBoxCP.TabIndex = 11;
            this.chBoxCP.Text = "Cowardly Protagonist";
            this.chBoxCP.UseVisualStyleBackColor = true;
            this.chBoxCP.CheckedChanged += new System.EventHandler(this.chBoxCP_CheckedChanged);
            // 
            // chBoxAC
            // 
            this.chBoxAC.AutoSize = true;
            this.chBoxAC.Checked = true;
            this.chBoxAC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBoxAC.Location = new System.Drawing.Point(195, 273);
            this.chBoxAC.Name = "chBoxAC";
            this.chBoxAC.Size = new System.Drawing.Size(171, 21);
            this.chBoxAC.TabIndex = 10;
            this.chBoxAC.Text = "Aggressive characters";
            this.chBoxAC.UseVisualStyleBackColor = true;
            this.chBoxAC.CheckedChanged += new System.EventHandler(this.chBoxAC_CheckedChanged);
            // 
            // chBoxAP
            // 
            this.chBoxAP.AutoSize = true;
            this.chBoxAP.Checked = true;
            this.chBoxAP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBoxAP.Location = new System.Drawing.Point(13, 273);
            this.chBoxAP.Name = "chBoxAP";
            this.chBoxAP.Size = new System.Drawing.Size(176, 21);
            this.chBoxAP.TabIndex = 9;
            this.chBoxAP.Text = "Aggressive Protagonist";
            this.chBoxAP.UseVisualStyleBackColor = true;
            this.chBoxAP.CheckedChanged += new System.EventHandler(this.chBoxAP_CheckedChanged);
            // 
            // chBoxSC
            // 
            this.chBoxSC.AutoSize = true;
            this.chBoxSC.Location = new System.Drawing.Point(195, 244);
            this.chBoxSC.Name = "chBoxSC";
            this.chBoxSC.Size = new System.Drawing.Size(136, 21);
            this.chBoxSC.TabIndex = 8;
            this.chBoxSC.Text = "Silent characters";
            this.chBoxSC.UseVisualStyleBackColor = true;
            this.chBoxSC.CheckedChanged += new System.EventHandler(this.chBoxSC_CheckedChanged);
            // 
            // chBoxSP
            // 
            this.chBoxSP.AutoSize = true;
            this.chBoxSP.Location = new System.Drawing.Point(13, 244);
            this.chBoxSP.Name = "chBoxSP";
            this.chBoxSP.Size = new System.Drawing.Size(141, 21);
            this.chBoxSP.TabIndex = 7;
            this.chBoxSP.Text = "Silent Protagonist";
            this.chBoxSP.UseVisualStyleBackColor = true;
            this.chBoxSP.CheckedChanged += new System.EventHandler(this.chBoxSP_CheckedChanged);
            // 
            // chBoxTE
            // 
            this.chBoxTE.AutoSize = true;
            this.chBoxTE.Location = new System.Drawing.Point(195, 32);
            this.chBoxTE.Name = "chBoxTE";
            this.chBoxTE.Size = new System.Drawing.Size(145, 21);
            this.chBoxTE.TabIndex = 6;
            this.chBoxTE.Text = "Talkative Enemies";
            this.chBoxTE.UseVisualStyleBackColor = true;
            this.chBoxTE.CheckedChanged += new System.EventHandler(this.chBoxTE_CheckedChanged);
            // 
            // chBoxPE
            // 
            this.chBoxPE.AutoSize = true;
            this.chBoxPE.Location = new System.Drawing.Point(195, 86);
            this.chBoxPE.Name = "chBoxPE";
            this.chBoxPE.Size = new System.Drawing.Size(143, 21);
            this.chBoxPE.TabIndex = 5;
            this.chBoxPE.Text = "Peaceful Enemies";
            this.chBoxPE.UseVisualStyleBackColor = true;
            this.chBoxPE.CheckedChanged += new System.EventHandler(this.chBoxPE_CheckedChanged);
            // 
            // chBoxPA
            // 
            this.chBoxPA.AutoSize = true;
            this.chBoxPA.Location = new System.Drawing.Point(13, 86);
            this.chBoxPA.Name = "chBoxPA";
            this.chBoxPA.Size = new System.Drawing.Size(156, 21);
            this.chBoxPA.TabIndex = 4;
            this.chBoxPA.Text = "Peaceful Antagonist";
            this.chBoxPA.UseVisualStyleBackColor = true;
            this.chBoxPA.CheckedChanged += new System.EventHandler(this.chBoxPA_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtBoxNotesField);
            this.groupBox5.Location = new System.Drawing.Point(12, 348);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(507, 150);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Notes:";
            // 
            // txtBoxNotesField
            // 
            this.txtBoxNotesField.Location = new System.Drawing.Point(6, 31);
            this.txtBoxNotesField.Multiline = true;
            this.txtBoxNotesField.Name = "txtBoxNotesField";
            this.txtBoxNotesField.ReadOnly = true;
            this.txtBoxNotesField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxNotesField.Size = new System.Drawing.Size(495, 113);
            this.txtBoxNotesField.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 510);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbxSubSettingFantasy);
            this.Controls.Add(this.grbxSettings);
            this.Controls.Add(this.grbxSetting);
            this.Controls.Add(this.btnStart);
            this.Name = "MainWindow";
            this.Text = "Main Window";
            this.grbxSetting.ResumeLayout(false);
            this.grbxSetting.PerformLayout();
            this.grbxSettings.ResumeLayout(false);
            this.grbxSettings.PerformLayout();
            this.grbxSubSettingFantasy.ResumeLayout(false);
            this.grbxSubSettingFantasy.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RadioButton rbtnFantasy;
        private System.Windows.Forms.RadioButton rbtnDetective;
        private System.Windows.Forms.GroupBox grbxSetting;
        private System.Windows.Forms.CheckBox chBoxRandEnc;
        private System.Windows.Forms.GroupBox grbxSettings;
        private System.Windows.Forms.CheckBox chBoxRandConOfLoc;
        private System.Windows.Forms.CheckBox chBoxRandFightRes;
        private System.Windows.Forms.CheckBox chBoxRandDistOfInit;
        private System.Windows.Forms.TextBox txtBoxSeed;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.CheckBox chBoxHideNTDActions;
        private System.Windows.Forms.GroupBox grbxSubSettingFantasy;
        private System.Windows.Forms.RadioButton rbtnGF;
        private System.Windows.Forms.RadioButton rbtnDA;
        private System.Windows.Forms.RadioButton rbtnDefaultDemo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chBoxGoalGetItem;
        private System.Windows.Forms.CheckBox chBoxGoalKill;
        private System.Windows.Forms.CheckBox chBoxGoalReachFinLoc;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chBoxCanFindEvidence;
        private System.Windows.Forms.CheckBox chBoxTA;
        private System.Windows.Forms.CheckBox chBoxAntognstWillSurv;
        private System.Windows.Forms.CheckBox chBoxPrtgWillSurv;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chBoxCA;
        private System.Windows.Forms.CheckBox chBoxCE;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtAntagonistSurvTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chBoxCC;
        private System.Windows.Forms.CheckBox chBoxCP;
        private System.Windows.Forms.CheckBox chBoxAC;
        private System.Windows.Forms.CheckBox chBoxAP;
        private System.Windows.Forms.CheckBox chBoxSC;
        private System.Windows.Forms.CheckBox chBoxSP;
        private System.Windows.Forms.CheckBox chBoxTE;
        private System.Windows.Forms.CheckBox chBoxPE;
        private System.Windows.Forms.CheckBox chBoxPA;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtBoxNotesField;
        private System.Windows.Forms.CheckBox chBoxUniqKills;
        private System.Windows.Forms.CheckBox chBoxEachAgentsHasUG;
        private System.Windows.Forms.CheckBox chBoxStrOrdVicSec;
        private System.Windows.Forms.Button btnChoiseOutputPath;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.TextBox txtBoxNodeCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProtagonistSurvTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxEvChance;
        private System.Windows.Forms.Label lblEvidence;
    }
}

