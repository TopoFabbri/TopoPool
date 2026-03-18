using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Architecture.Console;
using Architecture.Logic.Saving;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic
{
    public class Settings : IService, IDisposable
    {
        private readonly string filePath;

        public bool IsPersistant => true;

        [Save] public float HorizontalSensitivity { get; set; } = 1f;
        [Save] public float VerticalSensitivity   { get; set; } = 1f;
        [Save] public float StickSensitivity      { get; set; } = .1f;
        [Save] public float MinVerticalAngle      { get; set; } = -90f;
        [Save] public float MaxVerticalAngle      { get; set; } = 90f;

        [Save] public float StickRangeMin { get; set; } = -0.5f;
        [Save] public float StickRangeMax { get; set; } = 0.05f;
        
        [Save] public float MinSpeed { get; set; } = 0.01f;
        [Save] public float MaxSpeed { get; set; } = 10f;

        internal Settings(string filePath)
        {
            this.filePath = Path.Combine(filePath, "Settings.json");
            
            Load();
        }

        public void Dispose()
        {
            Save();
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(filePath)) return;

            StringBuilder sb = new();
            sb.AppendLine("{");

            List<MemberInfo> members = new(GetSaveMembers());
            
            for (int i = 0; i < members.Count; i++)
            {
                MemberInfo member = members[i];
                object value = member is PropertyInfo p ? p.GetValue(this) : ((FieldInfo)member).GetValue(this);

                string jsonValue = value switch
                {
                    null => "null",
                    bool b => b ? "true" : "false",
                    float f => f.ToString(CultureInfo.InvariantCulture),
                    double d => d.ToString(CultureInfo.InvariantCulture),
                    int j => j.ToString(),
                    string s => $"\"{s.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"",
                    _ => $"\"{value}\""
                };

                sb.Append($"  \"{member.Name}\": {jsonValue}");
                if (i < members.Count - 1) sb.Append(",");
                sb.AppendLine();
            }

            sb.AppendLine("}");

            try
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(filePath, sb.ToString());
            }
            catch (Exception e)
            {
                GameConsole.Error("Failed to save settings to " + filePath + ": " + e.Message);
            }
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            try
            {
                byte[] jsonBytes = File.ReadAllBytes(filePath);
                using (XmlReader reader = JsonReaderWriterFactory.CreateJsonReader(jsonBytes, new XmlDictionaryReaderQuotas()))
                {
                    XElement root = XElement.Load(reader);
                    IEnumerable<MemberInfo> members = GetSaveMembers();

                    foreach (MemberInfo member in members)
                    {
                        XElement element = root.Element(member.Name);
                        if (element == null) continue;

                        Type type = member is PropertyInfo p ? p.PropertyType : ((FieldInfo)member).FieldType;
                        object value = Convert.ChangeType(element.Value, type, CultureInfo.InvariantCulture);

                        if (member is PropertyInfo prop) prop.SetValue(this, value);
                        else if (member is FieldInfo field) field.SetValue(this, value);
                    }
                }
            }
            catch (Exception e)
            {
                GameConsole.Warning("Failed to load settings from file: " + filePath + "\n" + e);
            }
        }

        private IEnumerable<MemberInfo> GetSaveMembers()
        {
            MemberInfo[] allMembers = GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            List<MemberInfo> saveMembers = new();

            foreach (MemberInfo member in allMembers)
            {
                if (member is not FieldInfo && member is not PropertyInfo) 
                    continue;
                
                if (member.GetCustomAttribute<SaveAttribute>() != null)
                    saveMembers.Add(member);
            }

            return saveMembers;
        }
    }
}