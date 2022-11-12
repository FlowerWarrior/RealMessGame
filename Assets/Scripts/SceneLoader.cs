using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;

    private void OnEnable()
    {
        SC_Interactor.LevelEnded += HandleLevelEnd;
    }

    private void OnDisable()
    {
        SC_Interactor.LevelEnded -= HandleLevelEnd;
    }

    void HandleLevelEnd(bool result)
    {
        int levelToLoad;
        if (result == true)
        {
            levelToLoad = 0;
        }
        else
        {
            levelToLoad = 0;
        }
        StartCoroutine(LoadLevelAfter(levelToLoad, 2f));
    }

    IEnumerator LoadLevelAfter(int sceneID, float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        fadeAnimator.Play("FadeClose", 0, 0);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneID);
    }
}
