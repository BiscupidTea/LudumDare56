using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerUI : MonoBehaviour
{
    public void LoadNewScene(int NewSceneID)
    {
        SceneManager.LoadScene(NewSceneID);
    }

    public void CloseApplication()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}