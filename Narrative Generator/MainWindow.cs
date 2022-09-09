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
        private int additionalAgentsInEncounters = 2;
        private int additionalLocationsInEncounters = 3;

        /// <summary>
        /// Unconditional constructor with initialization of components.
        /// </summary>
        public MainWindow() { InitializeComponent(); }

        private void BtnStart_Click (object sender, EventArgs e)
        {
            if (settingSelected && goalTypeSelected)
            {
                btnStart.Enabled = false;
                SeedControl();
                system.Start();
                btnStart.Enabled = true;
            }
        }

        private void FantasySubSettingsOn() { grbxSubSettingFantasy.Visible = true; grbxSubSettingFantasy.Enabled = true; }

        private void FantasySubSettingsOff() { grbxSubSettingFantasy.Visible = false; grbxSubSettingFantasy.Enabled = false; }

        private void TxtBoxSeed_KeyPress (object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
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
        /// Controls the display of notes (tips) about various interface elements.
        /// </summary>
        /// <param name="noteText">Display text.</param>
        public void PrintNote (string noteText)
        {
            txtBoxNotesField.Clear();
            txtBoxNotesField.Text = noteText;
        }

        //////////////////////////////////
        /*      SETTING CHOISE         */
        /////////////////////////////////

        private void RbtnFantasy_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnFantasy.Checked) { FantasySubSettingsOn(); }
            else { FantasySubSettingsOff(); }
        }

        private void RbtnDetective_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnDetective.Checked)
            {
                system.setting = Setting.Detective;
                chBoxUniqKills.Enabled = true;
                chBoxStrOrdVicSec.Enabled = true;
                chBoxEachAgentsHasUG.Enabled = true;
                settingSelected = true;
                btnStart.Enabled = true;
                system.AgentsCounter = 5;
                system.LocationsCounter = 5; // 8
            }
            else
            {
                chBoxUniqKills.Enabled = false;
                chBoxStrOrdVicSec.Enabled = false;
                chBoxEachAgentsHasUG.Enabled = false;
            }
        }

        private void RbtnDefaultDemo_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnDefaultDemo.Checked)
            {
                system.setting = Setting.DefaultDemo;
                settingSelected = true;
                btnStart.Enabled = true;
                system.AgentsCounter = 3; // 7
                system.LocationsCounter = 4; // 8
            }
        }

        //////////////////////////////////
        /*     SUB-SETTING CHOISE      */
        /////////////////////////////////

        private void RbtnDA_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnFantasy.Checked && rbtnDA.Checked)
            {
                system.setting = Setting.DragonAge;
                chBoxRandEnc.Enabled = true;
                settingSelected = true;
                btnStart.Enabled = true;
                system.AgentsCounter = 2;
                system.LocationsCounter = 6;
            }
            else { chBoxRandEnc.Enabled = false; }
        }

        private void RbtnGF_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtnFantasy.Checked && rbtnGF.Checked)
            {
                system.setting = Setting.GenericFantasy;
                settingSelected = true;
                btnStart.Enabled = true;
                system.AgentsCounter = 6;
                system.LocationsCounter = 6;
            }
        }

        //////////////////////////////////
        /*           SETTINGS          */
        /////////////////////////////////

        private void ChBoxRandConOfLoc_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandConOfLoc.Checked) { system.RandomConnectionOfLocations = true; txtBoxSeed.Enabled = true; }
            else { system.RandomConnectionOfLocations = false; txtBoxSeed.Enabled = false; }
        }

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
        }

        private void ChBoxRandFightRes_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandFightRes.Checked) { system.RandomBattlesResults = true; txtBoxSeed.Enabled = true; }
            else { system.RandomBattlesResults = false; txtBoxSeed.Enabled = false; }
        }

        private void ChBoxRandDistOfInit_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandDistOfInit.Checked) { system.RandomDistributionOfInitiative = true; txtBoxSeed.Enabled = true; }
            else { system.RandomDistributionOfInitiative = false; txtBoxSeed.Enabled = false; }
        }

        private void ChBoxHideNTDActions_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxHideNTDActions.Checked) { system.HideNothingToDoActions = true; }
            else { system.HideNothingToDoActions = false; }
        }

        private void ChBoxGoalKill_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxGoalKill.Checked) { goalTypeSelected = true; system.goalManager.KillAntagonistOrAllEnemis = true; }
        }

        private void ChBoxGoalReachFinLoc_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxGoalReachFinLoc.Checked) { goalTypeSelected = true; system.goalManager.ReachGoalLocation = true; }
        }

        private void ChBoxGoalGetItem_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxGoalGetItem.Checked) { goalTypeSelected = true; system.goalManager.GetImportantItem = true; }
            PrintNote("This type of goal is under development." + Environment.NewLine + 
                      "Currently, selecting this item may cause the application to become unstable.");
        }

        private void chBoxUniqKills_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxUniqKills.Checked) { system.UniqWaysToKill = true; }
            else { system.UniqWaysToKill = false; }
        }

        private void chBoxStrOrdVicSec_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxStrOrdVicSec.Checked) { system.StrOrdVicSec = true; }
            else { system.StrOrdVicSec = false; }
        }

        private void chBoxEachAgentsHasUG_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxEachAgentsHasUG.Checked) { system.EachAgentsHasUG = true; }
            else { system.EachAgentsHasUG = false; }
        }

        private void chBoxTA_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxTA.Checked) { system.talkativeAntagonist = true; }
        }

        private void chBoxTE_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxTE.Checked) { system.talkativeEnemies = true; }
        }

        private void chBoxCA_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCA.Checked) { system.cunningAntagonist = true; }
        }

        private void chBoxCE_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCE.Checked) { system.cunningEnemies = true; }
        }

        private void chBoxPA_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxPA.Checked) { system.peacefulAntagonist = true; }
        }

        private void chBoxPE_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxPE.Checked) { system.peacefulEnemies = true; }
        }

        private void chBoxSP_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxSP.Checked) { system.silentProtagonist = true; }
        }

        private void chBoxSC_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxSC.Checked) { system.silentCharacters = true; }
        }

        private void chBoxAP_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAP.Checked) { system.aggresiveProtagonist = true; }
        }

        private void chBoxAC_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAC.Checked) { system.aggresiveCharacters = true; }
        }

        private void chBoxCP_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCP.Checked) { system.cowardlyProtagonist = true; }
        }

        private void chBoxCC_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxCC.Checked) { system.cowardlyCharacters = true; }
        }
    }
}
