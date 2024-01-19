﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using NAudio.Lame;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NLayer.NAudioSupport;
using NLayer;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;

namespace AudioEditor
{
    public class FileCommands
    {
        public static readonly string RootFfmpeg = "ffmpeg";

        // Получаем путь к текущей рабочей директории
        public static string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        // Составляем полный путь к целевой папке
        public static string folderPath = Path.Combine(CleanAfter(currentDirectory, "bin"), "temp");

        //MakeFolder(folderPath);
        public static string LastSaved => $"{folderPath}\\{name}{CommandManager.Type}";         
        public static string name = "audiofile";


        private static string CleanAfter(string path, string word)
        {
            string[] words = path.Split('\\');
            List<string> res = new List<string>();
            var adding = false;
            for(var i = words.Length - 1; i >= 0; i--)
            {
                if(adding)
                    res.Add(words[i]);
                if (words[i] == word)
                    adding = true;
            }
            res.Reverse();
            return string.Join('\\', res);

        }
        private static void MakeFolder(string folderPath)
        {

            // Удаляем папку и создаем
            Directory.Delete(folderPath, true);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
          
        }

        public static void BytesToMp3(byte[] audioBytes, string outputPath)
        {
            using (var ms = new MemoryStream(audioBytes))
            {
                using (var waveStream = new RawSourceWaveStream(ms, new WaveFormat()))
                {
                    using (var mp3Writer = new LameMP3FileWriter(outputPath, waveStream.WaveFormat, LAMEPreset.VBR_90))
                    {
                        waveStream.CopyTo(mp3Writer);
                    }
                }
            }
        }

        public static void BytesToMp3(byte[] track)
        {
            BytesToMp3(track, FileCommands.LastSaved);
        }


        public static byte[] Mp3ToBytes(string mp3FilePath)
        {
            using (var mp3Stream = new FileStream(mp3FilePath, FileMode.Open))
            {
                using (var mp3Reader = new Mp3FileReader(mp3Stream))
                {
                    using (var ms = new MemoryStream())
                    {
                        int bytesRead;
                        byte[] buffer = new byte[8192];

                        while ((bytesRead = mp3Reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                        }

                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
