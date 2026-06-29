using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange : MonoBehaviour
{
    public string sceneName;

    public void ChangeScenePlayer()
    {
        SystemManager.Instance.CPU = false;
        StartCoroutine(ChangeSceneCoroutine());
    }

    public void ChangeSceneCPU()
    {
        SystemManager.Instance.CPU = true;
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