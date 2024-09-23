using UnityEngine;

namespace TF.SettingMenu
{
    public abstract class SettingComponent : ScriptableObject
    {
        // empty, just used for classification of inheritance

        public abstract string DefaultPath { get; }
    }
}
