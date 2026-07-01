using UnityEngine;

public class SEManager : MonoBehaviour
{
    public AudioClip clickSE;
    public AudioClip diceSE;

    public AudioSource seSource;    // クリック音用
    public AudioSource diceSource;  // サイコロ用
    public AudioClip footStepSE;    // 移動音

    void Awake()
    {
        if (FindObjectsByType<SEManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        seSource.PlayOneShot(clickSE);
    }

    // サイコロを振り始める
    public void PlayDiceLoop()
    {
        if (!diceSource.isPlaying)
        {
            diceSource.clip = diceSE;
            diceSource.loop = true;
            diceSource.Play();
        }
    }

    // サイコロが止まる
    public void StopDiceLoop()
    {
        diceSource.Stop();
        diceSource.loop = false;
        diceSource.clip = null;
    }
    public void PlayFootStep()
    {
        seSource.PlayOneShot(footStepSE);
    }
}