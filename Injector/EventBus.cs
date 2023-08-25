using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector
{
    public static class EventBus
    {
        public static event Action onDisconnectController;
        public static event Action onReturnResultTesting;
        public static void Disconnecting()
        {
            onDisconnectController?.Invoke();
        }
        public static void GetResultTesting()
        {
            onReturnResultTesting?.Invoke();
        }
    }
}
