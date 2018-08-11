using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoSaveEditor
{
  static AutoSaveEditor()
  {
    EditorApplication.playModeStateChanged += (state) =>
    {
      if (state == PlayModeStateChange.ExitingEditMode)
      {
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
      }
    };
  }
}