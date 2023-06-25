using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the behavior of the graphical interface for interacting with the program.
    /// </summary>
    public partial class MainWindow : Form
    {
        private StoryAlgorithm system = new StoryAlgorithm();
        private bool settingSelected = false;
        private bool goalTypeSelected = false;
        Setting selectedSetting;
        private bool goalTypeStatusSelected = false;
        private int additionalAgentsInEncounters = 2;
        private int additionalLocationsInEncounters = 3;
        private string outputPath = "";
        private bool outputPathSelected = false;

        /// <summary>
        /// Unconditional constructor with initialization of components.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            PrintNote("Steps before starting:" + Environment.NewLine +
                      "1) Select a setting." + Environment.NewLine +
                      "2) Select a sub-setting (if available)." + Environment.NewLine +
                      "3) Select the type of agents goals." + Environment.NewLine +
                      "4) Set the system settings (if necessary)." + Environment.NewLine +
                      "5) Set the story settings (if necessary)." + Environment.NewLine +
                      "6) Set the agent behavior settings (if necessary)." + Environment.NewLine +
                      "7) Specify the address for outputting the resulting files.");
        }

        /// <summary>
        /// The method that handles pressing the "Start" button.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void BtnStart_Click (object sender, EventArgs e)
        {
            if (settingSelected && goalTypeSelected && outputPathSelected)
            {
                btnStart.Enabled = false;
                SeedControl();
                NodeCountControl();
                ProtagonistProtectionControl();
                AntagonistProtectionControl();
                EvidenceFindChanceControl();
                if (chBoxCA.Checked) { system.cunningAntagonist = true; }
                if (chBoxCE.Checked) { system.cunningEnemies = true; }
                if (chBoxAP.Checked) { system.aggresiveProtagonist = true; }
                if (chBoxAC.Checked) { system.aggresiveCharacters = true; }

                foreach (var selectedItem in listBoxItemsSelected.SelectedItems)
                {
                    system.TargetItemsNames.Add(selectedItem.ToString());
                }

                system.Start(ref txtBoxNotesField, txtOutputPath.Text.ToString());
                btnStart.Enabled = true;
            }
        }

        /// <summary>
        /// The method that handles pressing the "Select path" button.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void btnChoiseOutputPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                outputPath = FBD.SelectedPath;
                txtOutputPath.Text = FBD.SelectedPath.ToString();
                outputPathSelected = true;
            }
        }

        /// <summary>
        /// The method that handles pressing the "Exit" button.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        /// <summary>
        /// Text input handler for the "Seed" field, allowing entering only numbers.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void TxtBoxSeed_KeyPress (object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) { e.Handled = true;}
        }

        /// <summary>
        /// Text input handler for the "Node count" field, allowing entering only numbers.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void TxtBoxNodeCounter_KeyPress (object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) { e.Handled = true; }
        }

        /// <summary>
        /// Text input handler for the "Term of protagonist survival" field, allowing entering only numbers.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void TxtProtagonistSurvTime_KeyPress (object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) { e.Handled = true; }
        }

        /// <summary>
        /// Text input handler for the "Term of antagonist survival" field, allowing entering only numbers.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void TxtAntagonistSurvTime_KeyPress (object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) { e.Handled = true; }
        }

        /// <summary>
        /// Text input handler for the "Find evidence chance" field, allowing entering only numbers.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void txtBoxEvChance_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) { e.Handled = true; }
        }

        /// <summary>
        /// Handles seed value change control for random generation.
        /// </summary>
        public void SeedControl()
        {
            if (txtBoxSeed.Text == null) { txtBoxSeed.Text = "0"; system.Seed = 0; }
            else { system.Seed = Int32.Parse(txtBoxSeed.Text); }
        }

        /// <summary>
        /// Method controlling the assignment of the maximum number of nodes.
        /// </summary>
        public void NodeCountControl()
        {
            if (txtBoxNodeCount.Text == null) { txtBoxNodeCount.Text = "350"; system.MaxNodes = 350; }
            else { system.MaxNodes = Int32.Parse(txtBoxNodeCount.Text); }
        }

        /// <summary>
        /// A method that controls the assignment of the protagonist's protection time.
        /// </summary>
        public void ProtagonistProtectionControl()
        {
            if (txtProtagonistSurvTime.Text == null) { txtProtagonistSurvTime.Text = "0"; system.ProtagonistProtectionTime = 0; }
            else { system.ProtagonistProtectionTime = Int32.Parse(txtProtagonistSurvTime.Text); }
        }

        /// <summary>
        /// A method that controls the assignment of the antagonist's protection time.
        /// </summary>
        public void AntagonistProtectionControl()
        {
            if (txtAntagonistSurvTime.Text == null) { txtAntagonistSurvTime.Text = "0"; system.AntagonistProtectionTime = 0; }
            else { system.AntagonistProtectionTime = Int32.Parse(txtAntagonistSurvTime.Text); }
        }

        /// <summary>
        /// A method that controls the assignment of the probability of finding an evidence.
        /// </summary>
        public void EvidenceFindChanceControl()
        {
            if (txtBoxEvChance.Text == null) { txtBoxEvChance.Text = "60"; system.EvidenceFindChance = 60; }
            else
            {
                if (txtBoxEvChance.Enabled) { system.EvidenceFindChance = Int32.Parse(txtBoxEvChance.Text); }
            }
        }

        /// <summary>
        /// Controls the display of notes (tips) about various interface elements.
        /// </summary>
        /// <param name="noteText">Displayed text in text field.</param>
        public void PrintNote (string noteText)
        {
            txtBoxNotesField.Clear();
            txtBoxNotesField.Text = noteText;
        }

        //////////////////////////////////
        /*      SETTING CHOISE         */
        /////////////////////////////////

        /// <summary>
        /// The switch check handler - fantasy setting choose option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void RbtnFantasy_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnFantasy.Checked) { FantasySubSettingsOn(); }
            else { FantasySubSettingsOff(); }
            PrintNote("A setting representing a fantasy story, with corresponding agents, locations, and their actions. A choice of subsetting is available.");
        }

        /// <summary>
        /// The switch check handler - detective setting choose option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void RbtnDetective_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnDetective.Checked) { DetectiveSettingOn(); }
            else { DetectiveSettingOff(); }
            PrintNote("A setting representing a story in the detective genre, with corresponding agents, locations and their actions.");
        }

        /// <summary>
        /// The switch check handler - default setting choose option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void RbtnDefaultDemo_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnDefaultDemo.Checked) { DefaultDemoSettingOn(); }
        }

        /// <summary>
        /// A method that changes the availability of some interface elements when one of the fantasy subsettings is selected.
        /// </summary>
        private void FantasySubSettingsOn()
        {
            ClearingSettingChanges();
            grbxSubSettingFantasy.Visible = true;
            grbxSubSettingFantasy.Enabled = true;
            chBoxCanFindEvidence.Enabled = false;
            chBoxUniqKills.Enabled = false;
            chBoxStrOrdVicSec.Enabled = false;
            chBoxEachAgentsHasUG.Enabled = false;
            SelectedTargetItemsSectionDeactivation();
        }

        /// <summary>
        /// A method that changes the availability of some interface elements if none of the fantasy subsettings is selected.
        /// </summary>
        private void FantasySubSettingsOff() { grbxSubSettingFantasy.Visible = false; grbxSubSettingFantasy.Enabled = false; }

        /// <summary>
        /// A method that changes the availability of some interface elements and some settings when the detective setting is selected.
        /// </summary>
        private void DetectiveSettingOn()
        {
            ClearingSettingChanges();
            system.setting = Setting.Detective;
            selectedSetting = Setting.Detective;
            chBoxCanFindEvidence.Enabled = true;
            chBoxUniqKills.Enabled = true;
            chBoxStrOrdVicSec.Enabled = true;
            chBoxEachAgentsHasUG.Enabled = true;
            settingSelected = true;
            btnStart.Enabled = true;
            chBoxCanFindEvidence.Checked = true;
            txtBoxEvChance.Text = "50";
            system.AgentsCounter = 3;
            system.LocationsCounter = 4; // 8

            rbtnFantasy.Checked = false;
            rbtnDA.Checked = false;
            rbtnGF.Checked = false;

            SelectTargetLocationSectionActivation();
            SelectedTargetItemsSectionActivation();
        }

        /// <summary>
        /// A method that changes the availability of some interface elements if the detective setting is not selected.
        /// </summary>
        private void DetectiveSettingOff()
        {
            chBoxUniqKills.Enabled = false;
            chBoxStrOrdVicSec.Enabled = false;
            chBoxEachAgentsHasUG.Enabled = false;

            SelectTargetLocationSectionDeactivation();
            SelectedTargetItemsSectionDeactivation();
        }

        /// <summary>
        /// A method that changes the availability of some interface elements and some settings when the default setting is selected.
        /// </summary>
        private void DefaultDemoSettingOn()
        {
            ClearingSettingChanges();
            system.setting = Setting.DefaultDemo;
            selectedSetting = Setting.DefaultDemo;
            chBoxCanFindEvidence.Enabled = true;
            settingSelected = true;
            btnStart.Enabled = true;
            system.AgentsCounter = 3; // 7
            system.LocationsCounter = 4; // 8
        }

        /// <summary>
        /// A method that resets parameters that can be changed in different settings to standard values.
        /// </summary>
        public void ClearingSettingChanges()
        {
            chBoxRandConOfLoc.Checked = false;
            chBoxRandEnc.Checked = false;
            chBoxRandFightRes.Checked = false;
            chBoxRandDistOfInit.Checked = false;
            chBoxCanFindEvidence.Checked = false;
            txtBoxEvChance.Text = null;
            chBoxPrtgWillSurv.Checked = false;
            txtProtagonistSurvTime.Text = "0";
            chBoxAntgWillSurv.Checked = false;
            txtAntagonistSurvTime.Text = "0";
            chBoxUniqKills.Checked = false;
            chBoxStrOrdVicSec.Checked = false;
            chBoxEachAgentsHasUG.Checked = false;
            chBoxTA.Checked = false;
            chBoxTE.Checked = false;
            chBoxPA.Checked = false;
            chBoxPE.Checked = false;
            chBoxSP.Checked = false;
            chBoxSC.Checked = false;
            chBoxCP.Checked = false;
            chBoxCC.Checked = false;
        }

        //////////////////////////////////
        /*     SUB-SETTING CHOISE      */
        /////////////////////////////////

        /// <summary>
        /// The switch check handler - Dragon Age sub-setting choose option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void RbtnDA_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnFantasy.Checked && rbtnDA.Checked)
            {
                system.setting = Setting.DragonAge;
                selectedSetting = Setting.DragonAge;
                chBoxRandEnc.Enabled = true;
                chBoxRandConOfLoc.Enabled = false;
                chBoxPA.Enabled = false;
                chBoxPE.Enabled = false;
                settingSelected = true;
                btnStart.Enabled = true;
                system.AgentsCounter = 2;
                system.LocationsCounter = 6;

                SelectTargetLocationSectionActivation();
                SelectedTargetItemsSectionDeactivation();

                rbtnGF.Checked = false;
                rbtnDetective.Checked = false;
            }
            else { chBoxRandEnc.Enabled = false; chBoxRandConOfLoc.Enabled = true; }

            PrintNote("A sub-setting that simulates the main storyline of Dragon Age: Origins game, on a macro level. " +
                      "With corresponding agents, locations and their actions.");
        }

        /// <summary>
        /// The switch check handler - Generic Fantasy sub-setting choose option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void RbtnGF_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnFantasy.Checked && rbtnGF.Checked)
            {
                system.setting = Setting.GenericFantasy;
                selectedSetting = Setting.GenericFantasy;
                settingSelected = true;
                btnStart.Enabled = true;
                system.AgentsCounter = 6;
                system.LocationsCounter = 6;

                SelectTargetLocationSectionActivation();
                SelectedTargetItemsSectionActivation();

                rbtnDA.Checked = false;
                rbtnDetective.Checked = false;
            }

            PrintNote("A sub-setting that simulates an average fantasy, with agents, locations, and actions appropriate to the subgenre.");
        }

        //////////////////////////////////
        /*           SETTINGS          */
        /////////////////////////////////

        /// <summary>
        /// The chech-box check handler - "random connection of location" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxRandConOfLoc_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandConOfLoc.Checked) { system.RandomConnectionOfLocations = true; txtBoxSeed.Enabled = true; }
            else { system.RandomConnectionOfLocations = false; txtBoxSeed.Enabled = false; }
            PrintNote("When this setting is activated, connections (paths) between locations are randomly generated. Otherwise, the base presets are used.");
        }

        /// <summary>
        /// The chech-box check handler - "random encaunters" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxRandEnc_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandEnc.Checked)
            {
                system.RandomEncounters = true;
                txtBoxSeed.Enabled = true;
                system.AgentsCounter = system.AgentsCounter + additionalAgentsInEncounters;
                system.LocationsCounter = system.LocationsCounter + additionalLocationsInEncounters;
            }
            else
            {
                system.RandomEncounters = false;
                txtBoxSeed.Enabled = false;
                system.AgentsCounter = system.AgentsCounter - additionalAgentsInEncounters;
                system.LocationsCounter = system.LocationsCounter - additionalLocationsInEncounters;
            }
            PrintNote("When this setting is activated, random encounters (new locations and characters) are added to the story.");
        }

        /// <summary>
        /// The chech-box check handler - "random fights results" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxRandFightRes_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandFightRes.Checked) { system.RandomBattlesResults = true; txtBoxSeed.Enabled = true; }
            else { system.RandomBattlesResults = false; txtBoxSeed.Enabled = false; }
            PrintNote("When this setting is activated, the outcome of action-oppositions (for example, a fight) is determined randomly. " +
                      "Otherwise, such an action is always resolved as a success for the one who performed it.");
        }

        /// <summary>
        /// The chech-box check handler - "random distributions of agents initiative" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxRandDistOfInit_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandDistOfInit.Checked) { system.RandomDistributionOfInitiative = true; txtBoxSeed.Enabled = true; }
            else { system.RandomDistributionOfInitiative = false; txtBoxSeed.Enabled = false; }
            PrintNote("When this setting is activated, the value of initiative (which determines the order in which agents act) is randomly distributed.");
        }

        /// <summary>
        /// The chech-box check handler - hide "empty" actions option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxHideNTDActions_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxHideNTDActions.Checked) { system.HideNothingToDoActions = true; }
            else { system.HideNothingToDoActions = false; }
            PrintNote("When this setting is activated, empty actions will be removed from the generated render graph.");
        }

        /// <summary>
        /// The chech-box check handler - goal type is "kill all enemies".
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxGoalKill_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxGoalKill.Checked) { goalTypeSelected = true; goalTypeStatusSelected = true; system.goalManager.KillAntagonistOrAllEnemis = true; }
            else { goalTypeSelected = false; goalTypeStatusSelected = false; system.goalManager.KillAntagonistOrAllEnemis = false; }
            PrintNote("When choosing this type of target conditions, the winning (goal) condition for agents will be a change in status for agents with a certain role.");
        }

        /// <summary>
        /// The chech-box check handler - goal type is "be in specific location".
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxGoalReachFinLoc_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxGoalReachFinLoc.Checked) { goalTypeSelected = true; system.goalManager.ReachGoalLocation = true; }
            else { goalTypeSelected = false; system.goalManager.ReachGoalLocation = false; }
        }

        /// <summary>
        /// The chech-box check handler - goal type is "get item".
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void ChBoxGoalGetItem_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxGoalGetItem.Checked) { goalTypeSelected = true; system.goalManager.GetImportantItem = true; }
            else { goalTypeSelected = false; system.goalManager.GetImportantItem = false; }
        }

        /// <summary>
        /// The chech-box check handler - "unique kill description for every agent" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxUniqKills_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxUniqKills.Checked) { system.UniqWaysToKill = true; }
            else { system.UniqWaysToKill = false; }
            PrintNote("When this setting is activated, each murder (in a detective setting) will contain a unique description.");
        }

        /// <summary>
        /// The chech-box check handler - "strict order of victims selection" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxStrOrdVicSec_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxStrOrdVicSec.Checked) { system.StrOrdVicSec = true; }
            else { system.StrOrdVicSec = false; }
            PrintNote("When this setting is activated, murders (in a detective setting) will be committed by the killer in a specific order.");
        }

        /// <summary>
        /// The chech-box check handler - "each agents has unique goals" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxEachAgentsHasUG_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxEachAgentsHasUG.Checked) { system.EachAgentsHasUG = true; }
            else { system.EachAgentsHasUG = false; }
            PrintNote("When this setting is activated, agents will receive random goals, which, however, do not contradict the global goal settings.");
        }

        /// <summary>
        /// The chech-box check handler - "talkative antagonist" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxTA_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxTA.Checked) { system.talkativeAntagonist = true; }
            else { system.talkativeAntagonist = false; }
            PrintNote("When this setting is activated, conversational actions become available to the antagonist agent.");
        }

        /// <summary>
        /// The chech-box check handler - "talkative enemies" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxTE_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxTE.Checked) { system.talkativeEnemies = true; }
            else { system.talkativeEnemies = false; }
            PrintNote("When this setting is activated, conversational actions become available to enemy agents.");
        }

        /// <summary>
        /// The chech-box check handler - "cunning antagonist" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxCA_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCA.Checked) { system.cunningAntagonist = true; }
            else { system.cunningAntagonist = false; }
            PrintNote("When this setting is activated, tricky actions (such as entraping) become available to the antagonist agent.");
        }

        /// <summary>
        /// The chech-box check handler - "cunning enemies" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxCE_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCE.Checked) { system.cunningEnemies = true; }
            else { system.cunningEnemies = false; }
            PrintNote("When this setting is activated, tricky actions (such as luring) become available to enemy agents.");
        }

        /// <summary>
        /// The chech-box check handler - "peaceful antagonist" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxPA_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxPA.Checked) { system.peacefulAntagonist = true; }
            else { system.peacefulAntagonist = false; }
            PrintNote("When this setting is activated, aggressive actions are not available to the antagonist agent.");
        }

        /// <summary>
        /// The chech-box check handler - "peaceful enemies" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxPE_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxPE.Checked) { system.peacefulEnemies = true; }
            else { system.peacefulEnemies = false; }
            PrintNote("When this setting is activated, aggressive actions are not available to enemy agents");
        }

        /// <summary>
        /// The chech-box check handler - "silent protagonist" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxSP_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxSP.Checked) { system.silentProtagonist = true; }
            else { system.silentProtagonist = false; }
            PrintNote("When this setting is activated, dialog actions are not available to the player.");
        }

        /// <summary>
        /// The chech-box check handler - "silent characters" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxSC_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxSC.Checked) { system.silentCharacters = true; }
            else { system.silentCharacters = false; }
            PrintNote("When this setting is activated, dialog actions are not available to usual agents.");
        }

        /// <summary>
        /// The chech-box check handler - "aggressive protagonist" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxAP_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAP.Checked) { system.aggresiveProtagonist = true; }
            else { system.aggresiveProtagonist = false; }
            PrintNote("When this setting is activated, the player will be able to take aggressive actions towards agents.");
        }

        /// <summary>
        /// The chech-box check handler - "aggressive characters" options.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxAC_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAC.Checked) { system.aggresiveCharacters = true; }
            else { system.aggresiveCharacters = false; }
            PrintNote("When this setting is enabled, aggressive actions towards other agents become available to usual agents.");
        }

        /// <summary>
        /// The chech-box check handler - "cowardly protagonist" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxCP_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCP.Checked) { system.cowardlyProtagonist = true; }
            else { system.cowardlyProtagonist = false; }
            PrintNote("When this setting is activated, cowardly actions become available to the player, such as fleeing from danger.");
        }

        /// <summary>
        /// The chech-box check handler - "cowardly characters" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxCC_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCC.Checked) { system.cowardlyCharacters = true; }
            else { system.cowardlyCharacters = false; }
            PrintNote("When this setting is activated, cowardly actions, such as fleeing from danger, become available to usual agents.");
        }

        /// <summary>
        /// The chech-box check handler - "can find evidence" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxCanFindEvidence_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCanFindEvidence.Checked) { system.CanFindEvidence = true; txtBoxEvChance.Enabled = true; }
            else { system.CanFindEvidence = false; txtBoxEvChance.Enabled = false; }
            PrintNote("When this setting is activated, in detective setting, agents can find evidence against the antagonist in locations. " +
                      "Otherwise, the search for evidence action will be unavailable to them. In the field on the right, " +
                      "enter the percentage chance of success for this action.");
        }

        /// <summary>
        /// The chech-box check handler - "protagonist will survive" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxPrtgWillSurv_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxPrtgWillSurv.Checked) { system.ProtagonistWillSurvive = true; txtProtagonistSurvTime.Enabled = true; }
            else { system.ProtagonistWillSurvive = false; txtProtagonistSurvTime.Enabled = false; }
            PrintNote("When this setting is activated, the system will ensure that aggressive actions against the player are not successful. " +
                      "In the field below, enter the protection period or leave the field empty (0) for unlimited protection.");
        }

        /// <summary>
        /// The chech-box check handler - "antagonist will survive" option.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event data.</param>
        private void chBoxAntgWillSurv_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxAntgWillSurv.Checked) { system.AntagonistWillSurvive = true; txtAntagonistSurvTime.Enabled = true; }
            else { system.AntagonistWillSurvive = false; txtAntagonistSurvTime.Enabled = false; }
            PrintNote("When this setting is activated, the system will ensure that aggressive actions against the antagonist are not successful. " +
                      "In the field below, enter the protection period or leave the field empty (0) for unlimited protection.");
        }

        private void comBoxGoalLocationSelect_SelectedIndexChanged (object sender, EventArgs e)
        {
            system.TargetLocationName = comBoxGoalLocationSelect.SelectedItem.ToString();
        }

        private void comBoxGoalLocationSelect_DropDown(object sender, EventArgs e)
        {
            SelectedGoalLocationActualization();
        }

        private void SelectedGoalLocationActualization()
        {
            comBoxGoalLocationSelect.Items.Clear();

            switch (selectedSetting)
            {
                case Setting.DragonAge:
                    if (goalTypeStatusSelected)
                    {
                        comBoxGoalLocationSelect.Items.AddRange(new string[] { "Denerim" });
                    }
                    else if (!goalTypeSelected && !chBoxRandEnc.Checked)
                    {
                        comBoxGoalLocationSelect.Items.AddRange(new string[] 
                        { "Lothering", "MagesTower", "Orzammar", "BrecilianForest", "Denerim" });
                    }
                    else if (!goalTypeSelected && chBoxRandEnc.Checked)
                    {
                        comBoxGoalLocationSelect.Items.AddRange(new string[] 
                        { "Lothering", "MagesTower", "Orzammar", "BrecilianForest", "Denerim", "Road", "Crossroad", "Riverside" });
                    }
                    break;
                case Setting.GenericFantasy:
                    if (goalTypeStatusSelected)
                    {
                        comBoxGoalLocationSelect.Items.AddRange(new string[] { "Town" });
                    }
                    else
                    {
                        comBoxGoalLocationSelect.Items.AddRange(new string[]
                        { "Village", "Town", "AncientRuins", "MysteriousForest", "Dungeon", "MountainPass" });
                    }
                    break;
                case Setting.Detective: 
                    comBoxGoalLocationSelect.Items.AddRange(new string[] { "LivingRooms", "Hall", "Rock", "Beach" });
                    break;
                case Setting.DefaultDemo:
                    comBoxGoalLocationSelect.Items.AddRange(new string[] 
                    { "kitchen", "dining-room", "hall", "garden", "bedroom", "guest-bedroom", "bathroom", "attic" });
                    break;
            }
        }

        private void SelectTargetLocationSectionActivation()
        {
            chBoxGoalReachFinLoc.Visible = true;
            comBoxGoalLocationSelect.Visible = true;
        }

        private void SelectTargetLocationSectionDeactivation()
        {
            chBoxGoalReachFinLoc.Visible = false;
            comBoxGoalLocationSelect.Visible = false;
        }

        private void listBoxItemsSelected_SelectedIndexChanged (object sender, EventArgs e)
        {
            // listBoxItemsSelected.SelectedItems();
        }

        private void SelectedTargetItemsSectionActivation()
        {
            listBoxItemsSelected.Items.Clear();

            if (rbtnDetective.Checked) { listBoxItemsSelected.Items.AddRange(new string[] { "Evidence" }); }
            if (rbtnGF.Checked) { listBoxItemsSelected.Items.AddRange(new string[] { "Weapon", "Armor" }); }

            chBoxGoalGetItem.Visible = true;
            listBoxItemsSelected.Visible = true;
        }

        private void SelectedTargetItemsSectionDeactivation()
        {
            chBoxGoalGetItem.Visible = false;
            listBoxItemsSelected.Visible = false;

            listBoxItemsSelected.Items.Clear();
        }
    }
}
