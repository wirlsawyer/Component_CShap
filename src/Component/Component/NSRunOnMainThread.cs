using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component
{
    public class NSRunOnMainThread
    {
        static public void Run(Form form, Delegate method)
        {
            //EX: NSRunOnMainThread.Run(new Action(()=>func(param1, param2)));
            form.Invoke(method);
        }
    }
}
