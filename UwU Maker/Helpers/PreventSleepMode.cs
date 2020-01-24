using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UwU_Maker
{
    class PreventSleepMode
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [FlagsAttribute]
        private enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }

        public static void ToggleSleepMode(bool Toggle)
        {
            if(Toggle)
            {
                SetThreadExecutionState(ExecutionState.EsContinuous | ExecutionState.EsSystemRequired);
            }
            else
            {
                SetThreadExecutionState(ExecutionState.EsContinuous);
            }
        }

     
    }
}
