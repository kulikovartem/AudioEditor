using System;
using System.Diagnostics;
using CommandInterface;

namespace AudioEditor
{
    internal class MergeCommand
    {
        public class MergeAudioCommand : ICommand
        {
            private string inputFilePath2;

            public MergeAudioCommand(string inputFilePath2)
            {
                this.inputFilePath2 = inputFilePath2;
            }

            public string Execute()
            {
                try
                {
                    string errorOutput = "";
                    var input = FileCommands.InputFilePath;
                    var output = FileCommands.OutputFilePath;
                    var command = $"-y -i \"{input}\" -i \"{inputFilePath2}\" -filter_complex \"amerge=inputs=2\" -ac 2 \"{output}\"";

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
}
