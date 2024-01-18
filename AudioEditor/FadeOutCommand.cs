using AudioEditor;
using System.Diagnostics;
using System;
using CommandInterface;

namespace AudioEditor
{
    public class FadeOutCommand : ICommand
    {
        private double duration;

        public FadeOutCommand(double duration)
        {
            this.duration = duration;
        }

        public string Execute()
        {
            try
            {
                string errorOutput = "";
                var input = FileCommands.LastSaved;
                FileCommands.name = FileCommands.name + "1";
                var command = $"-y -i \"{input}\" -af \"afade=t=in:d={duration.ToString(System.Globalization.CultureInfo.InvariantCulture)}\" \"{FileCommands.LastSaved}\"";


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