using Scripts.Core;
using System;

namespace Scripts.Settings
{
    [Serializable]
    public class Setting<T> : ISetting
    {
        public string name;
        public T currentValue;

        public string Name { get { return name; } }
        public object Value { get { return currentValue; } }

        public Action<T> action;
        public readonly T defaultValue;

        public Setting(T defaultValue)
        {
            this.defaultValue = defaultValue;
            currentValue = defaultValue;
        }
        public void Invoke() => action?.Invoke(currentValue);
        public void ChangeSetting(T value, bool withInvoke = true)
        {
            currentValue = value;

            if (withInvoke)
                Invoke();
        }
        public void ChangeSetting(object value, bool withInvoke = true)
        {
            if (value.GetType() == typeof(T))
            {
                ChangeSetting((T)value, withInvoke);
            }
            else
            {
                currentValue = GameManager.AdvancedConvert<T>(value);
            }

            if (withInvoke)
                Invoke();
        }
    }
    public struct SettingToSave
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SettingToSave(string name = null, object value = null)
        {
            Name = name;
            Value = value;
        }
    }
    public interface ISetting
    {
        string Name { get; }
        object Value { get; }

        void Invoke();
        void ChangeSetting(object value, bool withInvoke = true);
    }
}
