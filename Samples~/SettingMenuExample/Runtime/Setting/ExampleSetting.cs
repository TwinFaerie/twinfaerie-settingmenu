using TF.SettingMenu;
using UnityEngine;

namespace TF.Samples.SettingMenu
{
    public class ExampleSetting : SettingComponent
    {
        public override string DefaultPath => "Example Setting.asset";
        
        // just use any class you need here
        [SerializeField] private bool boolOption;
        [SerializeField] private float floatOption;
        [SerializeField] private int intOption;
        [SerializeField] private string stringOption;
    }
}