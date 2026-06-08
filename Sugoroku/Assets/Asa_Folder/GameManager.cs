using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public Dice dice;
    public PlayerMove[] players;
    SystemManager s_manager;
    public static GameManager Instance; // どこからでも参照可能
    public GameObject ChoicePanel; // ポイント取得ボタン
    public GameObject ItemPanel;

    public TextMeshProUGUI oxygenText; // 酸素数UI
    public Text messageText;
    public Text p_ItemText1;
    public Text p_ItemText2;
    public Text p_ItemText3;

    public Vector3 item_position;
    public int oxygen = 0; // 酸素 50
    public int baseDecrement = 1; // 毎ターン減る酸素数
    public int energyCores = 0; // 持っているコア数
    public int safety = 0;
    public List<int> Item_Index = new List<int>();

    //public int item_n = PlayerMove.item;
    public static int currentPlayer = 0;
    private bool isMoving = false;
    private bool isChoosing = false;
    private bool isDice = true;

    public int i = 0;
    public  Vector3 [] Item_Pos;
   
    bool Next = true;

    void Start()
    {

        if (ChoicePanel != null)
            ChoicePanel.SetActive(false);

        if (ItemPanel != null)
            ItemPanel.SetActive(false);

        if (dice != null)
            dice.OnDiceRolled += OnDiceRolled;

        UpdateOxygenUI();

        s_manager = FindFirstObjectByType<SystemManager>();

        s_manager.players = players;
    }
    // ボタンを押すまでサイコロ不可
    void Update()
    {
        if (isMoving || isChoosing) return;

        SkipGoalPlayers();
     
        PlayerMove current = players[currentPlayer];
        // まだ戻る選択していないならUI表示
        if (!current.HasChosenDirection() && !current.IsReturning())
        {
            if (players[currentPlayer].isCPU == false)
            {
                ChoicePanel.SetActive(true);
                isChoosing = true;
                return;
            }
            else
            {
                int cpuChoice = Random.Range(0, 2);

                if (cpuChoice == 0)
                {
                    players[currentPlayer].StartReturn();

                    isChoosing = false;
                }
                else if (cpuChoice == 1)
                {
                    players[currentPlayer].ContinueForward();

                    isChoosing = false;
                }
            }
        }

        // サイコロ
        if (players[currentPlayer].isCPU == true)
        {
            isDice = false;
            //Debug.Log(players[currentPlayer].currentIndex);
            dice.Roll();
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isDice == true ||
            Mouse.current.leftButton.wasPressedThisFrame && isDice == true &&
            players[currentPlayer].isCPU == false)
        {
            isDice = false;
            dice.Roll();
        }
    }

    void SkipGoalPlayers()
    {
        int goalCount = 0;

        foreach (var p in players)
        {
            if (p.hasGoal)
            {
                goalCount++;
            }
        }

        // 全員ゴール
        if (goalCount >= players.Length)
        {
            enabled = false;

            if (SceneManager.GetActiveScene().name == "Stage")
            {
                s_manager.Score();
                s_manager.Reset();
                SceneManager.LoadScene("Result");
            }
            else if (SceneManager.GetActiveScene().name == "Stage1")
            {
                s_manager.Score();
                s_manager.Reset();
                SceneManager.LoadScene("Result");
            }
            else if (SceneManager.GetActiveScene().name == "Stage2")
            {
                s_manager.Score();
                s_manager.Reset();
                SceneManager.LoadScene("Result");
            }
            return;
        }

        // ゴール済みプレイヤーを飛ばす
        while (players[currentPlayer].hasGoal)
        {
            NextTurn();
        }
    }
    // ほかのスクリプトからアクセスできるようになる(シングルトン設定)
    void Awake()
    {
        Instance = this;
    }
    // 酸素減少
    void ConsumeOxygen()
    {
        int consumption = baseDecrement + players[currentPlayer].item;

        oxygen -= consumption ;

        if (oxygen <= 0)
        {
            oxygen = 0;
            if(SceneManager.GetActiveScene().name == "Stage")
            {
                for (int i = 0;i <= 2;i++)
                {
                    if (players[i].hasGoal == false)
                    {
                        players[i].item = 0;
                        players[i].oxygen = 0;
                    }
                }
                s_manager.Score();
                s_manager.Reset();
                SceneManager.LoadScene("Stage1");
            }
            else if (SceneManager.GetActiveScene().name == "Stage1")
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (players[i].hasGoal == false)
                    {
                        players[i].item = 0;
                        players[i].oxygen = 0;
                    }
                }
                s_manager.Score();
                s_manager.Reset();
                SceneManager.LoadScene("Stage2");
            }

        }

        UpdateOxygenUI();
    }

    // 所持アイテム数に総じて移動量減少
    public int Dice2(int diceValue)
    {
        diceValue -= players[currentPlayer].item;

        if(diceValue < 1)
        {
            ItemPanel.SetActive(false);
        }

        return diceValue;
    }
    void EndTurn()
    {
        int index = players[currentPlayer].currentIndex;

        ConsumeOxygen();

        if (Item_Index.Contains(index))
        {
            isMoving = false;
            isDice = true;
            Debug.Log("もうとったよ");
            StartCoroutine(ShowMessage("もうとったよ")); NextTurn();
        }
        else
        {
            if (players[currentPlayer].isCPU == false)
            {
                isMoving = false;
                Item_Index.Add(index);
                ItemPanel.SetActive(true);
            }
            else
            {
                isMoving = false;
                int cpuChoice = Random.Range(0, 2);

                if (cpuChoice == 0)
                {
                    item_position = players[currentPlayer].item_position;
                    players[currentPlayer].Item();

                    if (i < Item_Pos.Length)
                    {
                        Item_Pos[i] = players[currentPlayer].item_position;
                        i++;
                    }

                    isDice = true;

                    NextTurn();
                }
                else if (cpuChoice == 1)
                {
                    isDice = true;

                    NextTurn();
                }
            }
        }
    }

    // ターン交代
    void NextTurn()
    {
        currentPlayer++;

        if (currentPlayer >= players.Length)
        {
            currentPlayer = 0;
            if (players[currentPlayer].hasGoal)
            {
                NextTurn();
            }

            foreach (var p in players)
            {
                p.Reset();
            }
        }
    }
    // 酸素数ui
    void UpdateOxygenUI()
    {
        oxygenText.text = "" + oxygen;
    }
    // サイコロの結果を受け取る
    void OnDiceRolled(int value)
    {
        if (isMoving) return;

        if (players[currentPlayer])
        {
            isMoving = true;

            players[currentPlayer].Move(value);

            StartCoroutine(WaitMove());
        }
    }
    System.Collections.IEnumerator WaitMove()
    {
        PlayerMove movingPlayer = players[currentPlayer];

        while (movingPlayer.isMoving)
        {
            yield return null;
        }

        EndTurn();
    }

    // そのマスにプレイヤーがいるかチェック
    public bool IsTileOccupied(int index)
    {
        foreach (var p in players)
        {
            if (p.GetCurrentIndex() == index)
            {
                return true;
            }
        }
        return false;
    }

    // 引き返しボタン
    public void OnClickChoice1()
    {
        players[currentPlayer].StartReturn();

        ChoicePanel.SetActive(false);

        isChoosing = false;
    }

    // 何もしないボタン
    public void OnClickChoice2()
    {
        players[currentPlayer].ContinueForward();

        ChoicePanel.SetActive(false);

        isChoosing = false;
    }

    // アイテムボタン
    public void OnClikItem1()
    {
        item_position = players[currentPlayer].item_position;
        players[currentPlayer].Item();

        if (i < Item_Pos.Length)
        {
            Item_Pos[i] = players[currentPlayer].item_position;
            i++;
        }

        if (currentPlayer == 0)
        {
            p_ItemText1.text = "" + players[currentPlayer].item;
        }
        if (currentPlayer == 1)
        {
            p_ItemText2.text = "" + players[currentPlayer].item;
        }
        if (currentPlayer == 2)
        {
            p_ItemText3.text = "" + players[currentPlayer].item;
        }
        ItemPanel.SetActive(false);
        isDice = true;

        NextTurn();
    }

    // 何もしないボタン
    public void OnClickItem2()
    {
        //players[currentPlayer].ContinueForward();

        ItemPanel.SetActive(false);
        isDice = true;

        NextTurn();
    }
    IEnumerator ShowMessage(string msg)
    {
        messageText.text = msg;

        yield return new WaitForSeconds(2f);

        messageText.text = "";
    }
}
