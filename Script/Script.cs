using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Script
{
    public enum ScriptEventType
    {
        OnUpdate
    }

    public class ScriptEvent
    {
        public static readonly List<string> EventNames = new List<string>(Enum.GetNames(typeof(ScriptEventType)));

        public ScriptEvent(MethodInfo methodInfo)
        {
            method = methodInfo;
        }

        public void ResetMethod(MethodInfo methodInfo)
        {
            method = methodInfo;
        }

        public object Invoke(object obj, params object[] parameters)
        {
            return method.Invoke(obj, parameters);
        }

        MethodInfo method;
    }

    public abstract class Script
    {
        public string Name { get; protected set; }
        public Type ScriptableType { get; protected set; }
        public IDictionary<string, ScriptEvent> Events { get; protected set; }

        public abstract ScriptEvent GetEvent(string eventName);
        public ScriptEvent this[string eventName] => GetEvent(eventName);
        public ScriptEvent this[ScriptEventType eventType] => GetEvent(eventType.ToString());

        public void SetEvents(Type type)
        {
            ScriptableType = type;
            foreach (string eventName in ScriptEvent.EventNames)
            {
                MethodInfo method = ScriptableType.GetMethod(eventName);
                SetEvent(eventName, method);
            }
        }

        protected abstract void SetEvent(string eventName, MethodInfo method);
    }

    public class TechnoScript : Script
    {
        public TechnoScript(string name)
        {
            Name = name;
            Events = new Dictionary<string, ScriptEvent>();
        }

        public override ScriptEvent GetEvent(string eventName)
        {
            return Events[eventName];
        }

        protected override void SetEvent(string eventName, MethodInfo method)
        {
            if (Events.ContainsKey(eventName))
            {
                Events[eventName].ResetMethod(method);
            }
            else
            {
                Events.Add(eventName, new ScriptEvent(method));
            }
        }
    }
}
