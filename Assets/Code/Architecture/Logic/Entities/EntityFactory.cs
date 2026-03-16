using System;
using System.Collections.Generic;
using ImageCampus.ToolBox.ServiceProvider;

namespace Architecture.Logic.Entities
{
    internal sealed class EntityFactory : IService, IDisposable
    {
        public bool IsPersistant => false;

        private readonly List<uint> ids = new();

        public void Dispose()
        {
            ids.Clear();
        }

        public TEntityType CreateEntity<TEntityType>(params object[] parameters) where TEntityType : Entity
        {
            TEntityType entity = Activator.CreateInstance(typeof(TEntityType), GetId()) as TEntityType;
            
            entity?.Configure(parameters);
            
            return entity;
        }

        private uint GetId()
        {
            uint id = 0;
            
            while (ids.Contains(id))
                id++;
            
            ids.Add(id);
            
            return id;
        }
    }
}