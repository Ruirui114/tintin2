using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip titleBGM;
    public AudioClip stageBGM;
    public AudioClip resultBGM;
    public AudioClip lastResultBGM;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        ChangeBGM(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeBGM(scene.name);
    }

    void ChangeBGM(string sceneName)
    {
        AudioClip clip = null;

        switch (sceneName)
        {
            case "Title":
            case "Menu":
                clip = titleBGM;
                break;

            case "Stage":
            case "Stage1":
            case "Stage2":
                clip = stageBGM;
                break;

            case "Result":
            case "Result1":
                clip = resultBGM;
                break;

            case "LastResult":
                clip = lastResultBGM;
                break;
        }

        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Awake()
    {
        if (FindObjectsByType<BGMManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}