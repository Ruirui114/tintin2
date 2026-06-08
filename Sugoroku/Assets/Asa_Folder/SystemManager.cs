using JetBrains.Annotations;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public PlayerMove[] players;
    public int []score;
    public static SystemManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
            score = new int[3];
        }
        else
        {
            Destroy(gameObject);
        }

    }


    public void Score()
    {
        for (int i = 0; i < players.Length; i++)
        {
            score[i] += players[i].item + players[i].oxygen;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].PlayerReset();
        }
    }
    public void GameReset()
    {
        
    }
}
