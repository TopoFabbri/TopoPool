using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic
{
    internal class Settings : IService
    {
        public bool IsPersistant => true;
        
        public float HorizontalSensitivity { get; set; } = .1f;
        public float VerticalSensitivity { get; set; } = .1f;
        
        public float MinSpeed { get; set; } = 0.01f;
        public float MaxSpeed { get; set; } = 10f;
    }
}