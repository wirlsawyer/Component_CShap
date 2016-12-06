using System;
using System.Diagnostics;
using System.Threading;

namespace Component
{
    public class NSOperation
    {
        public void Execution()
        {
            DateTime d = DateTime.Now;

            Debug.WriteLine(d.ToString());
            Thread.Sleep(200);

        }
    }
}
