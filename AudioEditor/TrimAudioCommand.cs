using System.Diagnostics;
using System;
using CommandInterface;


namespace AudioEditor
{
    public class TrimAudioCommand : ICommand
    {
        private double startTime;
        private double endTime;

        public TrimAudioCommand(double startTime, double endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public string Execute()
        {
            try
            {
                string errorOutput = "";
                var input = FileCommands.LastSaved;
                FileCommands.name = FileCommands.name + "1";
                var command = $"-y -i \"{input}\" -af \"atrim=start={startTime.ToString(System.Globalization.CultureInfo.InvariantCulture)}:end={endTime.ToString(System.Globalization.CultureInfo.InvariantCulture)}\" -vn \"{FileCommands.LastSaved}\"";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = FileCommands.RootFfmpeg,
                    Arguments = command,

                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    errorOutput = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        throw new InvalidOperationException($"FFmpeg завершился с кодом ошибки {process.ExitCode}: {errorOutput}");
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}