using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace GdEcs
{

    public class EntityComponentStore : Reference
    {

        public event EntityDelegate ComponentsChanged = delegate { };

        public IEntity Entity { get; private set; }
        private Dictionary<Type, List<IEntityComponent>> componentTypeMap = new Dictionary<Type, List<IEntityComponent>>();

        public EntityComponentStore(IEntity entity)
        {
            this.Entity = entity;
        }

        public bool HasComponentsOfType<T>(uint atLeast = 1) where T : IEntityComponent
        {
            return componentTypeMap.ContainsKey(typeof(T)) && componentTypeMap[typeof(T)].Count >= atLeast;
        }

        public bool HasComponentsOfAllTypes(params Type[] types)
        {
            Debug.Assert(types.Length > 0);
            foreach (var type in types)
                if (!componentTypeMap.ContainsKey(type))
                    return false;
            return true;
        }

        public T? GetFirstComponentOfType<T>() where T : class, IEntityComponent
        {
            if (!componentTypeMap.ContainsKey(typeof(T)))
                return null;
            return (T)componentTypeMap[typeof(T)][0];
        }

        public T[] GetComponentsOfType<T>() where T : IEntityComponent
        {
            if (!componentTypeMap.ContainsKey(typeof(T)))
                return new T[] { };
            var components = componentTypeMap[typeof(T)];
            var ret = new T[components.Count];
            for (int i = 0; i < ret.Length; ++i)
                ret[i] = (T)components[i];
            return ret;
        }

        public void AddComponentToStore(IEntityComponent component)
        {
            var type = component.GetType();
            if (!componentTypeMap.ContainsKey(type))
            {
                componentTypeMap.Add(type, new List<IEntityComponent>());
            }
            Debug.Assert(!componentTypeMap[type].Contains(component));
            componentTypeMap[type].Add(component);
            ComponentsChanged(Entity);
        }

        public void RemoveComponentFromStore(IEntityComponent component)
        {
            var type = component.GetType();
            if (!componentTypeMap.ContainsKey(type))
                return;
            componentTypeMap[type].Remove(component);
            if (componentTypeMap[type].Count <= 0)
                componentTypeMap.Remove(type);
            ComponentsChanged(Entity);
        }

    }

}