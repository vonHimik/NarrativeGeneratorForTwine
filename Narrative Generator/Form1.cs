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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        StoryAlgorithm system = new StoryAlgorithm();

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (rbtn1.Checked || rbtn2.Checked)
            {
                system.Start();
            }
        }

        private void rbtn1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn1.Checked) { system.setting = Setting.Fantasy; }
        }

        private void rbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn2.Checked) { system.setting = Setting.Detective; }
        }
    }
}
