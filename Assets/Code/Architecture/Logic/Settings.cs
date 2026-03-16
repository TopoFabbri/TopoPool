using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic
{
    internal class Settings : IService
    {
        public bool IsPersistant => true;
        
        public float HorizontalSensitivity { get; set; } = .1f;
        public float VerticalSensitivity { get; set; } = .1f;
    }
}