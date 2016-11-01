using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slalom.Boost.VisualStudio.Events;

namespace Slalom.Boost.Learn.WindowsHost
{
    public class CreateSolutionEventHandler : IHandleEvent<CommandRequested>
    {
        public void Handle(CommandRequested instance)
        {
            if (instance.Name == "CreateSolution")
            {
                var form = new VisualStudio.Forms.CreateSolutionWindow();
                if (form.ShowDialog() == true)
                {
                    //MessageBox.Show(form.SolutionPath);
                }
            }
        }
    }
}
