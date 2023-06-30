using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// Class serving for low-level interaction with the renderer.
    /// </summary>
    class GraphvizWrapper
    {
        /// <summary>
        /// The method in which the process settings (renderer launch) are sets and it is launched.
        /// </summary>
        /// <param name="path">The path to write the resulting file.</param>
        /// <param name="name">The name of the resulting file.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>An indicator of the success or failure of the process launch.</returns>
        public bool Run (string path, string name, ref TextBox note)
        {
            Process process = new Process();
            process.StartInfo.FileName = Directory.GetCurrentDirectory() + "//graphviz//dot.exe";
            int remainder = 3; // ".dt"
            process.StartInfo.WorkingDirectory = path.Remove(path.Count() - (name.Length + remainder), name.Length + remainder);
            process.StartInfo.Arguments = "-Tpng -O " + name + ".dt";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Hides GUI
            process.StartInfo.CreateNoWindow = true;                   // Hides console

            if (process.Start())
            {
                note.Text = "PROCESS START - CMD";
                process.WaitForExit();
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// A method that launches a method that controls the launch and configuration of the rendering process, 
        /// passing to it method the necessary information from high-level.
        /// </summary>
        /// <param name="filePath">The path to write the resulting file.</param>
        /// <param name="fileName">The name of the resulting file.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>An indicator of the success or failure of the process launch.</returns>
        public bool RunGraphviz (string filePath, string fileName, ref TextBox note)
        {
            if (Run(filePath, fileName, ref note))
            {
                return true;
            }
            else { return false; }
        }
    }
}
