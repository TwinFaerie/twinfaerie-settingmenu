#if TF_HAS_TFODINEXTENDER
using Sirenix.OdinInspector;
#else
using UnityEngine;
#endif

namespace TF.SettingMenu
{
    #if TF_HAS_TFODINEXTENDER
    public abstract class SettingComponent : SerializedScriptableObject
    #else
    public abstract class SettingComponent : ScriptableObject
    #endif
    {
        // empty, just used for classification of inheritance

        public abstract string DefaultPath { get; }
    }
}
