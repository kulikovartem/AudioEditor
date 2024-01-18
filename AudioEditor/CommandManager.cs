using System.Collections.Generic;
using CommandInterface;

namespace AudioEditor
{
    public class CommandManager
    {
        private Stack<byte[]> undoStack;
        private Stack<byte[]> redoStack;
        public static byte[] CurrentTrack { get; set; }

        public static string Type = ".mp3";

        public CommandManager(byte[] track)
        {
            CurrentTrack = track;
            undoStack = new Stack<byte[]>();
            redoStack = new Stack<byte[]>();
        }

        public void ExecuteCommand(ICommand command)
        {
            if (CurrentTrack != null)
            {
                undoStack.Push(CurrentTrack);
            }
            var a = command.Execute();
            if (a != "")
            {
                throw new System.Exception(a);
            }
            CurrentTrack = FileCommands.Mp3ToBytes(FileCommands.LastSaved);
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
