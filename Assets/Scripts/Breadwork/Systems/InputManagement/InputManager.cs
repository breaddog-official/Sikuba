using Scripts.SaveManagement;
using Unity.Burst;
using UnityEngine.InputSystem;

namespace Scripts.InputManagement
{
    [BurstCompile]
    public static class InputManager
    {
        public static Controls Controls { get; set; }
        public const string CONTROLS_FILENAME = "controls";

        private readonly static ISaveSystem Saver = new WithoutSerializationSaver(".json");

        static InputManager()
        {
            Controls = new Controls();
            Controls.Enable();
        }
        public static void SaveControlsOverrides()
        {
            SaveManager.SaveToFile(Controls.SaveBindingOverridesAsJson(), CONTROLS_FILENAME, Saver);
        }
        public static void LoadControlsOverrides()
        {
            try
            {
                Controls.LoadBindingOverridesFromJson(SaveManager.LoadFromFile<string>(CONTROLS_FILENAME, Saver));
            }
            catch (FileNotFoundException)
            {
                return;
            }
        }
    }
}
