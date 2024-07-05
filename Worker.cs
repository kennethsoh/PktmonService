using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PktmonService
{
    public class Worker : BackgroundService
    {
        private const string CaptureFilePath = @"C:\Users\vagrant\capture.etl";
        private const string OutputFilePath = @"C:\Users\vagrant\capture.pcap";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => StopPktmon());
            await Task.Run(() => StartPktmon(), stoppingToken);
        }

        private void StartPktmon()
        {
            if (File.Exists(CaptureFilePath))
            {
                File.Delete(CaptureFilePath);
            }

            var pktmonProcess = new Process();
            pktmonProcess.StartInfo.FileName = "pktmon";
            pktmonProcess.StartInfo.Arguments = "start --capture --comp all --file-name " + CaptureFilePath;
            pktmonProcess.StartInfo.UseShellExecute = false;
            pktmonProcess.StartInfo.RedirectStandardOutput = true;
            pktmonProcess.StartInfo.RedirectStandardError = true;

            pktmonProcess.Start();
            pktmonProcess.WaitForExit();
        }

        private void StopPktmon()
        {
            var stopProcess = new Process();
            stopProcess.StartInfo.FileName = "pktmon";
            stopProcess.StartInfo.Arguments = "stop";
            stopProcess.StartInfo.UseShellExecute = false;
            stopProcess.StartInfo.RedirectStandardOutput = true;
            stopProcess.StartInfo.RedirectStandardError = true;

            stopProcess.Start();
            stopProcess.WaitForExit();

            if (stopProcess.ExitCode != 0)
            {
                // Handle error
            }
            else
            {
                var convertProcess = new Process();
                convertProcess.StartInfo.FileName = "pktmon";
                convertProcess.StartInfo.Arguments = $"etl2pcap {CaptureFilePath} --out {OutputFilePath}";
                convertProcess.StartInfo.UseShellExecute = false;
                convertProcess.StartInfo.RedirectStandardOutput = true;
                convertProcess.StartInfo.RedirectStandardError = true;

                convertProcess.Start();
                convertProcess.WaitForExit();
            }
        }
    }
}
