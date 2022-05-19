﻿using System;
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
    public partial class Form1 : Form
    {
        StoryAlgorithm system = new StoryAlgorithm();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (rbtn1.Checked || rbtn2.Checked || rbtn4.Checked)
            {
                btnStart.Enabled = false;
                system.seed = Int32.Parse(txtBoxSeed.Text);
                system.Start();
            }
        }

        private void rbtn1_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtn1.Checked)
            {
                system.setting = Setting.Fantasy;
                chBoxRandEnc.Enabled = true;
            }
        }

        private void rbtn2_CheckedChanged (object sender, EventArgs e)
        {
            if (rbtn2.Checked) { system.setting = Setting.Detective; }
        }

        private void rbtn4_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn1.Checked && rbtn4.Checked) { system.setting = Setting.GenericFantasy; }
        }

        private void chBoxRandEnc_CheckedChanged (object sender, EventArgs e)
        {
            if (chBoxRandEnc.Checked) { system.randomEncounters = true; }
        }

        private void chBoxRandConOfLoc_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRandConOfLoc.Checked) { system.randomConnectionOfLocations = true; }
        }

        private void chBoxRandFightRes_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRandFightRes.Checked) { system.randomBattlesResults = true; }
        }

        private void chBoxRandDistOfInit_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRandDistOfInit.Checked) { system.randomDistributionOfInitiative = true; }
        }

        private void chBoxHideNTDActions_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxHideNTDActions.Checked) { system.hideNothingToDoActions = true; }
        }
    }
}
