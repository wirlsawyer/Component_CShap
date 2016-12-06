using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component
{
    public class NSSleep
    {
        static public void Sleep(long milliseconds)
        {
            long target = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (true)
            {
                Application.DoEvents();
                long now_milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (now_milliseconds > target + milliseconds)
                {
                    break;
                }
            }
        }
    }
}
