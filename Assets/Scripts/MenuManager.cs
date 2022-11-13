using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMenager: MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}