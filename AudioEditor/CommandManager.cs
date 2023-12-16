using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandInterface;


namespace AudioEditor
{
    public class CommandManager
    {
        private LinkedList<Fragment> fragments = new LinkedList<Fragment>();
        private Stack<ICommand> undoStack = new Stack<ICommand>();
        private Stack<ICommand> redoStack = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                var command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }

        public Fragment GetLast() { return fragments.Last(); }
    }

}
