using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Services.Interfaces
{
    public interface IHeartbeatService
    {
        event Action<DateTime>? Ticked;
        public void Start();
        public void Stop();
    }
}
