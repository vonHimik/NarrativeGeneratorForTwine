using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that controls the launch of the renderer.
    /// </summary>
    class Graphviz
    {
        /// <summary>
        /// Wrapper object.
        /// </summary>
        GraphvizWrapper wrapper;
        /// <summary>
        /// An indicator of the success or failure of rendering.
        /// </summary>
        public bool isSuccess;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public Graphviz()
        {
            wrapper = new GraphvizWrapper();
            isSuccess = false;
        }

        /// <summary>
        /// The method that starts the renderer.
        /// </summary>
        /// <param name="filePath">The path to write the resulting file.</param>
        /// <param name="fileName">The name of the resulting file.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void Run (string filePath, string fileName, ref TextBox note)
        {
            note.Text = "RUN GRAPHVIZ";

            if (wrapper.RunGraphviz(filePath, fileName, ref note)) { isSuccess = true; }
            else { isSuccess = false; }
        }
    }
}