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
    //public int player1_score = 0;
    //public int player2_score = 0;
    //public int player3_score = 0;
    //public int player1_comprehensivescore = 0;
    //public int player2_comprehensivescore = 0;
    //public int player3_comprehensivescore = 0;

    void Start()
    {
        //for (int i = 0; i <= 2; i++)
        //{
        //    if (i == 0)
        //    {
        //        player1_score = s_manager.score[i];
        //        player1_scoreText.text = "" + player1_score;
        //        Debug.Log(player1_score);
        //        player1_comprehensivescore += player1_score;
        //        player1_comprehensivescoreText.text = "" + player1_comprehensivescore;
        //    }
        //    if (i == 1)
        //    {
        //        player2_score = s_manager.score[i];
        //        player2_comprehensivescore += player2_score;
        //    }
        //    if (i == 2)
        //    {
        //        player3_score = s_manager.score[i];
        //        player3_comprehensivescore += player3_score;
        //    }

        //}
        player1_scoreText.text =
            SystemManager.Instance.score[0].ToString();

        player2_scoreText.text =
            SystemManager.Instance.score[1].ToString();

        player3_scoreText.text =
            SystemManager.Instance.score[2].ToString();
    }
}
