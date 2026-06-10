using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public SystemManager s_manager;

    public TextMeshProUGUI player1_scoreText;
    public TextMeshProUGUI player2_scoreText;
    public TextMeshProUGUI player3_scoreText;
    public TextMeshProUGUI player1_comprehensivescoreText;
    public TextMeshProUGUI player2_comprehensivescoreText;
    public TextMeshProUGUI player3_comprehensivescoreText;

    void Start()
    {
        player1_scoreText.text =
            SystemManager.Instance.score[0].ToString();

        player2_scoreText.text =
            SystemManager.Instance.score[1].ToString();

        player3_scoreText.text =
            SystemManager.Instance.score[2].ToString();
    }
}
