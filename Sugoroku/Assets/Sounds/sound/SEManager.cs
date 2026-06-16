using UnityEngine;

public class SEManager : MonoBehaviour
{
    public AudioClip clickSE;

    private AudioSource audioSource;

    void Awake()
    {
        if (FindObjectsByType<SEManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        Debug.Log("ÉNÉäÉbÉNâπçƒê∂");
        Debug.Log(gameObject.name);

        audioSource.PlayOneShot(clickSE);
    }
}