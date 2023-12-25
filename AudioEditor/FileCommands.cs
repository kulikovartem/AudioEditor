using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioEditor
{
    public class FileCommands
    {
        public static readonly string RootFfmpeg = "ffmpeg";
        public static string InputFilePath => $"C:\\Users\\artem\\Downloads\\audiofile{CommandManager.Type}";
        public static string OutputFilePath => $"C:\\Users\\artem\\Documents\\audiofile{CommandManager.Type}";


        public static WaveStream LoadAudioFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            try
            {
                var lastFour = filePath.Substring(filePath.Length - 4);
                if (lastFour == ".wav")
                {
                    CommandManager.Type = ".wav";
                    return new WaveFileReader(filePath);
                }
                else if (lastFour == ".mp3")
                {
                    CommandManager.Type = ".mp3";
                    return new Mp3FileReader(filePath);
                }
                else
                {
                    throw new Exception("Неправильный формат");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the file: {ex.Message}");
                throw; // Повторно вызывает исключение для внешнего обработчика
            }
        }

        public static void SaveWaveStreamToFile(WaveStream waveStream, string outputPath)
        {
            waveStream.Position = 0;
            using (var waveFileWriter = new WaveFileWriter(outputPath, waveStream.WaveFormat))
            {
                byte[] buffer = new byte[waveStream.WaveFormat.AverageBytesPerSecond];
                int bytesRead;
                while ((bytesRead = waveStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    waveFileWriter.Write(buffer, 0, bytesRead);
                }
            }
        }

        public static void SaveWaveStreamToFile(WaveStream waveStream)
        {
            var root = InputFilePath;
            SaveWaveStreamToFile(waveStream, root);
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"Файл {filePath} успешно удален.");
                }
                else
                {
                    Console.WriteLine($"Файл {filePath} не найден.");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Произошла ошибка при удалении файла: {e.Message}");
            }
        }
    }
}
