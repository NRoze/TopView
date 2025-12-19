using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services
{
    public class HeartbeatService() : IHeartbeatService
    {
        private CancellationTokenSource? _cts; 
        public event Action<DateTime>? Ticked;

        public void Start()
        {
            if (_cts != null) return;

            _cts = new CancellationTokenSource();

            // Task.Run on a background thread
            Task.Run(() => RunTimerLoop(_cts.Token));
        }

        private async Task RunTimerLoop(CancellationToken token)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

            try
            {
                while (await timer.WaitForNextTickAsync(token))
                {
                    Ticked?.Invoke(DateTime.UtcNow);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
