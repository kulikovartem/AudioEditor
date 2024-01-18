using System;
using System.Diagnostics;
using CommandInterface;

namespace AudioEditor
{
        public class MergeCommand : ICommand
        {
            private string inputFilePath2;

            public MergeCommand(string inputFilePath2)
            {
                this.inputFilePath2 = inputFilePath2;
            }

            public string Execute()
            {
                try
                {
                    string errorOutput = "";
                    var input = FileCommands.LastSaved;
                    FileCommands.name = FileCommands.name + "1";
                    var command = $"-i {input} -i {inputFilePath2} -filter_complex \"[0:a][1:a]concat=n=2:v=0:a=1[out]\" -map \"[out]\" {FileCommands.LastSaved}";

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
