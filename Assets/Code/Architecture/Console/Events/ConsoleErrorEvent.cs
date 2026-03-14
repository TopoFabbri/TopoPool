using ImageCampus.ToolBox.Events;

namespace Architecture.Console.Events
{
    public struct ConsoleErrorEvent : IEvent
    {
        public string message;

        public void Assign(params object[] parameters)
        {
            message = parameters[0] as string;
        }

        public void Reset()
        {
            message = default(string);
        }
    }
}
