using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange : MonoBehaviour
{
    public string sceneName;

    public void ChangeScenePlayer()
    {
        SystemManager.Instance.CPU1 = false;
        SystemManager.Instance.CPU2 = false;
        StartCoroutine(ChangeSceneCoroutine());
    }

    public void ChangeScene1CPU()
    {
        SystemManager.Instance.CPU1 = true;
        SystemManager.Instance.CPU2 = false;
        StartCoroutine(ChangeSceneCoroutine());
    }

    public void ChangeScene2CPU()
    {
        SystemManager.Instance.CPU2 = true;
        SystemManager.Instance.CPU1 = false;
        StartCoroutine(ChangeSceneCoroutine());
    }

    public void ChangeScene()
    {
        StartCoroutine(ChangeSceneCoroutine());
    }
    IEnumerator ChangeSceneCoroutine()
    {
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(sceneName);
    }
}