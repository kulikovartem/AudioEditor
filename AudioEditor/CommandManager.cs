using NAudio.Wave;
using System.Collections.Generic;
using CommandInterface;

namespace AudioEditor
{
    public class CommandManager
    {
        private Stack<WaveStream> undoStack;
        private Stack<WaveStream> redoStack;
        public static WaveStream CurrentTrack { get; set; }

        public static string Type { get; set; }

        public CommandManager(WaveStream track)
        {
            CurrentTrack = track;
            undoStack = new Stack<WaveStream>();
            redoStack = new Stack<WaveStream>();
        }

        public void ExecuteCommand(ICommand command)
        {
            if (CurrentTrack != null)
            {
                undoStack.Push(CurrentTrack);
            }
            FileCommands.SaveWaveStreamToFile(CurrentTrack);
            var a = command.Execute();
            var root = FileCommands.OutputFilePath;
            CurrentTrack = FileCommands.LoadAudioFile(root);
            redoStack.Clear();
        }
        private bool CanUndo => undoStack.Count > 0;
        private bool CanRedo => redoStack.Count > 0;

        public void Undo()
        {
            if (CanUndo)
            {
                redoStack.Push(CurrentTrack);
                CurrentTrack = undoStack.Pop();
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                undoStack.Push(CurrentTrack);
                CurrentTrack = redoStack.Pop();
            }
        }
    }
}
