# Overview
This module created to centralize scriptable object used for in-game setting

# How To Use
## Create New Setting using SettingComponent
- Create your own Setting by creating new class that holds your game setting
  ```csharp
  using TF.SettingMenu;
  using UnityEngine;
  
  namespace TF.Samples.SettingMenu
  {
      public class ExampleSetting : SettingComponent
      {
          // just use any class you need here
          [SerializeField] private bool boolOption;
          [SerializeField] private float floatOption;
          [SerializeField] private int intOption;
          [SerializeField] private string stringOption;
      }
  }
  ```
- This setting file needs to be extended from SettingComponent to make it shows on Game Setting Window
  
## Create Setup Menu using SettingComponentMenu

- Create editor script to create Setting that you just created before by using Setup menu 
  ```csharp
  using TF.SettingMenu.Editor;
  using UnityEditor;
  
  namespace TF.Samples.SettingMenu.Editor
  {
      public class ExampleSettingMenu : SettingComponentMenu<ExampleSetting>
      {
          private const string SETTING_NAME = "Example Setting";
  
          [MenuItem(DEFAULT_CREATE_MENU_PATH + SETTING_NAME, priority = 100)]
          public static void CreateSettingMenu()
          {
              CreateSettingMenu(SETTING_NAME);
          }
      }
  }
  ```
- You can create your own setup function without SettingComponentMenu, but it is easier to just use method

## Setup Your Setting
- Now you can just use Setup option to automatically create the Setting that you just created on  
  ```TwinFaerie -> Setup -> YOUR_SETTING_NAME```  
  ![image](https://github.com/TwinFaerie/twinfaerie-settingmenu/assets/34561311/e2250594-c5e6-488d-8dfc-ef87f14395f4)

## Open Game Setting Window
- That's it, now when you open game setting your setting should shows inside  
  ![image](https://github.com/TwinFaerie/twinfaerie-settingmenu/assets/34561311/933bce47-5ccf-4a2e-b348-eba18298438f)

## (OPTIONAL) Adding Icon to your Setting
- You can also add icon to your setting by placing .png image inside Setting/Icon folder with same name as your setting name  
  ![image](https://github.com/TwinFaerie/twinfaerie-settingmenu/assets/34561311/8c2c5fbf-6e2c-4732-a491-9e5603c5c91f)

# Notes
- This module is free-to-use, however as this module is specific to solve problem in our project, the support for other project is limited. Feel free to fork this if needed
