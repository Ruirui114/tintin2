using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange : MonoBehaviour
{
    public string sceneName;

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