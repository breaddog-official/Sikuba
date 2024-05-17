using System.Reflection;
using Unity.Burst;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public abstract class Translater : MonoBehaviour
    {
        [field: SerializeField]
        public string Name { get; private set; }
        public string TranslationString { get; private set; }

        protected FieldInfo field;
        protected bool isFirst = true;

        protected virtual void Awake()
        {
            field = typeof(Translation).GetField(Name);
            Validate();
        }
        protected virtual void OnEnable()
        {
            TranslateManager.GameLanguageChanged += Validate;
            if (isFirst)
            {
                isFirst = false;
                return;
            }
            Validate();
        }
        protected virtual void Validate()
        {
            string currentValue = (string)field.GetValue(TranslateManager.Translation);
            if (currentValue != TranslationString)
            {
                TranslationString = currentValue;
                ChangeElement();
            }
        }
        protected virtual void OnDisable() => TranslateManager.GameLanguageChanged -= Validate;

        public abstract void ChangeElement();
    }
}
