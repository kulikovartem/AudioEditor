using System;
using System.Diagnostics;
using CommandInterface;

namespace AudioEditor
{
    public class MixAudioCommand : ICommand
    {
        private string inputFilePath2;

        public MixAudioCommand(string inputFilePath2)
        {
            this.inputFilePath2 = inputFilePath2;
        }

        public string Execute()
        {
            try
            {
                string errorOutput = "";
                var output = FileCommands.OutputFilePath;
                var input = FileCommands.InputFilePath;
                var command = $"-y -i \"{input}\" -i \"{inputFilePath2}\" -filter_complex \"amix=inputs=2:duration=longest\" \"{output}\"";

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
