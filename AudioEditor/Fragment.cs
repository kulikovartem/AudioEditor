using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;


namespace AudioEditor
{
    public class Fragment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PathToAudio { get; set; }
        public WaveStream Value { get; set; }

        public Fragment()
        {
            
        }

        private void SetValueByPath(string pathToAudio)
        {
            var extension = Path.GetExtension(pathToAudio).ToLower();
            switch (extension)
            {
                case ".wav":
                    Value = new WaveFileReader(pathToAudio);
                    break;
                case ".mp3":
                    Value = new Mp3FileReader(pathToAudio);
                    break;
                default:
                    throw new NotSupportedException("Unsupported audio format: " + extension);
            }
        }

        private void SetNameByPath(string pathToAudio)
        {
            Name = Path.GetFileNameWithoutExtension(pathToAudio);
        }
    }
}
