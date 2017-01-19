using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace TeleprompterConsole
{
    // Configures the Teleprompter to use a given delay
    internal class TeleprompterConfig
    {
        private bool done;
        public bool Done => done;

        public void SetDone()
        {
            done = true;
        }

        private object lockHandle = new object();
        public int DelayInMilliseconds { get; private set; } = 200;


        public void UpdateDelay(int increment)
        {
            var newDelay = Min(DelayInMilliseconds + increment, 1000);
            newDelay = Max(newDelay, 20);
            lock (lockHandle)
            {
                DelayInMilliseconds = newDelay;
            }
        }
    }
}
