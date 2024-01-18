using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandInterface;


namespace AudioEditor
{
    public class ChangeSpeedCommand : ICommand
    {
        private double speed;
        public ChangeSpeedCommand(double speed)
        {
            this.speed = speed;
        }

        public string Execute()
        {
            try
            {
                string errorOutput = "";
                var input = FileCommands.LastSaved;
                FileCommands.name = FileCommands.name + "1";
                var command = $"-y -i \"{input}\" -filter:a \"atempo={speed.ToString(System.Globalization.CultureInfo.InvariantCulture)}\" -vn \"{FileCommands.LastSaved}\"";

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
