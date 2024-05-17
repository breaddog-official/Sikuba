using TMPro;
using Unity.Burst;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TextMeshProTranslater : Translater
    {
        private TMP_Text text;

        protected override void Awake()
        {
            if (TryGetComponent(out TMP_Text text))
            {
                this.text = text;
            }
            else
            {
                this.text = GetComponentInChildren<TMP_Text>();
            }
            base.Awake();
        }
        public override void ChangeElement()
        {
            text.SetText(TranslationString);
        }
    }
}
