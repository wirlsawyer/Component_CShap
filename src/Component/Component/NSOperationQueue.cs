using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Component
{
    public class NSOperationQueue
    {
        private Queue<NSOperation> _queue = null;
        private Thread _thread = null;
        private bool flag_continue;

        public NSOperationQueue()
        {
            _queue = new Queue<NSOperation>();
            _queue.Clear();


            this.Start();
        }

        public void Stop()
        {
            flag_continue = false;
            _thread.Abort();
            _thread.Join();
            _thread = null;
            Debug.WriteLine("NSOperationQueue did Stop");
        }

        public void Start()
        {
            flag_continue = true;
            _thread = new Thread(new ThreadStart(doloop));
            _thread.IsBackground = true;
            _thread.Start();
            Debug.WriteLine("NSOperationQueue did Start");
        }

        public void Add(NSOperation operation)
        {
            _queue.Enqueue(operation);
        }

        public void CancelAll()
        {
            this.Stop();
            _queue.Clear();
            this.Start();

            Debug.WriteLine("NSOperationQueue did CancelAll");
        }

        public bool IsIdle()
        {
            return _queue.Count == 0;
        }

        public int Count()
        {
            return _queue.Count;
        }
        private void doloop()
        {
            while (flag_continue)
            {
                if (_queue.Count == 0)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    NSOperation item = _queue.Dequeue();
                    item.Execution();
                }
            }
        }
    }
}
