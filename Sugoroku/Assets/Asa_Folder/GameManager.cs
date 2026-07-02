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
    public TileManager tileManager;
    public GameObject ChoicePanel; // ポイント取得ボタン
    public GameObject ItemPanel;
    public GameObject DropPanel;
    public TextMeshProUGUI oxygenText; // 酸素数UI
    public Text messageText;
    public Text p_ItemText1;
    public Text p_ItemText2;
    public Text p_ItemText3;

    public Text p_PointText1;
    public Text p_PointText2;
    public Text p_PointText3;

    public Text Playernum;

    public Vector3 item_position;
    public int oxygen = 0; // 酸素 50
    public int baseDecrement = 1; // 毎ターン減る酸素数
    public int energyCores = 0; // 持っているコア数
    public int safety = 0;
    public List<int> Item_Index = new List<int>();

    public static int currentPlayer = 0;
    private bool isMoving = false;
    private bool isChoosing = false;
    private bool isDice = true;

    public int index = 0;
    public int i = 0;
    public Vector3[] Item_Pos;

    bool Next = true;

    void Start()
    {

        currentPlayer = 0;
        Debug.Log("開始 currentPlayer = " + currentPlayer);

        if (ChoicePanel != null)
            ChoicePanel.SetActive(false);

        if (ItemPanel != null)
            ItemPanel.SetActive(false);

        if (DropPanel != null)
            DropPanel.SetActive(false);

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

        if (s_manager.CPU1 == true)
        {
            players[1].isCPU = false;
            players[2].isCPU = true;
        }

        if (s_manager.CPU2 == true)
        {
            players[1].isCPU = true;
            players[2].isCPU = true;
        }

        Playernum.text = "" + (currentPlayer + 1);
    
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
            Debug.Log($"プレイヤー: {currentPlayer}, 今の位置: {players[currentPlayer].currentIndex}");
            isDice = false;
            dice.Roll();
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isDice == true ||
            Mouse.current.leftButton.wasPressedThisFrame && isDice == true &&
            players[currentPlayer].isCPU == false)
        {
            Debug.Log($"プレイヤー: {currentPlayer}, 今の位置: {players[currentPlayer].currentIndex}");
            isDice = false;
            dice.Roll();
        }
    }

    void SkipGoalPlayers()
    {
        Debug.Log($"Scene = {SceneManager.GetActiveScene().name}");

        Debug.Log(
            $"P1={players[0].hasGoal} " +
            $"P2={players[1].hasGoal} " +
            $"P3={players[2].hasGoal}"
        );


        // 全員ゴール

        if (players[0].hasGoal == true
            && players[1].hasGoal == true
            && players[2].hasGoal == true)
        {
            enabled = false;

            if (SceneManager.GetActiveScene().name == "Stage")
            {
                s_manager.Score();
                s_manager.Reset();
                Item_Index.Clear();
                currentPlayer = 0;
                SceneManager.LoadScene("Result");
            }
            else if (SceneManager.GetActiveScene().name == "Stage1")
            {
                s_manager.Score();
                s_manager.Reset();
                Item_Index.Clear();
                currentPlayer = 0;
                SceneManager.LoadScene("Result1");
            }
            else if (SceneManager.GetActiveScene().name == "Stage2")
            {
                s_manager.Score();
                s_manager.Reset();
                Item_Index.Clear();
                currentPlayer = 0;
                SceneManager.LoadScene("LastResult");
            }
            return;
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

        oxygen -= consumption;

        if (oxygen <= 0)
        {
            oxygen = 0;
            if (SceneManager.GetActiveScene().name == "Stage")
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (players[i].hasGoal == false)
                    {
                        players[i].item = 0;
                        players[i].oxygen = 0;
                        players[i].point = 0;
                    }
                }
                s_manager.Score();
                s_manager.Reset();
                Item_Index.Clear();
                currentPlayer = 0;
                SceneManager.LoadScene("Result");
            }
            else if (SceneManager.GetActiveScene().name == "Stage1")
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (players[i].hasGoal == false)
                    {
                        players[i].item = 0;
                        players[i].oxygen = 0;
                        players[i].point = 0;
                    }
                }
                s_manager.Score();
                s_manager.Reset();
                Item_Index.Clear();
                currentPlayer = 0;
                SceneManager.LoadScene("Result1");
            }
            else if (SceneManager.GetActiveScene().name == "Stage2")
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (players[i].hasGoal == false)
                    {
                        players[i].item = 0;
                        players[i].oxygen = 0;
                        players[i].point = 0;
                    }
                }
                s_manager.Score();
                s_manager.Reset();
                Item_Index.Clear();
                currentPlayer = 0;
                SceneManager.LoadScene("LastResult");
            }

        }

        UpdateOxygenUI();
    }

    // 所持アイテム数に総じて移動量減少
    public int Dice2(int diceValue)
    {
        diceValue -= players[currentPlayer].item;

        if (diceValue < 1)
        {
            ItemPanel.SetActive(false);
        }

        return diceValue;
    }
    void EndTurn()
    {
        Debug.Log($"プレイヤー: {currentPlayer}, 最後の位置: {players[currentPlayer].currentIndex}");
        // ゴール判定
        if (players[currentPlayer].currentIndex <= 4 && players[currentPlayer].isReturning)
        {
            Debug.Log($"プレイヤー: {currentPlayer}, ゴール");
            players[currentPlayer].hasGoal = true;
            players[currentPlayer].oxygen = oxygen;
        }

        SkipGoalPlayers();

        ConsumeOxygen();

        index = players[currentPlayer].currentIndex;

        if(index <= 4)
        {
            isMoving = false;
            isDice = true;

            NextTurn();
        }
        else if (Item_Index.Contains(index))
        {
            isMoving = false;
            StartCoroutine(ShowMessage("もうアイテムないよ"));

            if (players[currentPlayer].item >= 1 && players[currentPlayer].isCPU == false)
            {
                DropPanel.SetActive(true);
            }
            else if (players[currentPlayer].item >= 1 && players[currentPlayer].isCPU == true)
            {
                int cpuChoice = Random.Range(0, 2);

                if (cpuChoice == 0)
                {
                    TileData tile =
                    players[currentPlayer]
                    .points[players[currentPlayer].currentIndex]
                    .GetComponent<TileData>();

                    for (int t = 0; t < Item_Pos.Length; t++)
                    {
                        if (Item_Pos[t] == players[currentPlayer].item_position)
                        {
                            Item_Pos[t] = Vector3.zero;
                        }
                    }

                    players[currentPlayer].point -= 30;
                    if (players[currentPlayer].point < 0)
                    {
                        players[currentPlayer].point = 0;
                    }

                    Item_Index.Remove(players[currentPlayer].currentIndex);
                    players[currentPlayer].ItemDown();
                    tileManager.ChangeTileColor((players[currentPlayer].currentIndex - 5), Color.white);


                    UpdateUI();

                    DropPanel.SetActive(false);
                    isDice = true;

                    NextTurn();
                }
                else
                {
                    isDice = true;
                    NextTurn();
                }
            }
            else
            {
                isDice = true;
                NextTurn();
            }
        }
        else
        {
            if (players[currentPlayer].isCPU == false && index > 4)
            {
                isMoving = false;
                ItemPanel.SetActive(true);
            }
            //このプレイヤーがCPUなら
            else
            {
                isMoving = false;
                int cpuChoice = Random.Range(0, 2);

                if (cpuChoice == 0 && index > 4)
                {
                    item_position = players[currentPlayer].item_position;
                    players[currentPlayer].ItemUp();

                    TileData tile =
                     players[currentPlayer]
                    .points[players[currentPlayer].currentIndex]
                    .GetComponent<TileData>();

                    if (tile != null && !tile.collected)
                    {
                        players[currentPlayer].point += tile.point;

                        tile.collected = true;

                        UpdateUI();
                    }

                    if (i < Item_Pos.Length)
                    {
                        Item_Pos[i] = players[currentPlayer].item_position;
                        i++;
                    }

                    tileManager.ChangeTileColor((players[currentPlayer].currentIndex - 5), Color.gray);
                    isDice = true;

                    NextTurn();
                }
                else
                {
                    isMoving = false;
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
        //このプレイヤーが最後だったら最初のプレイヤーの戻す
        if (currentPlayer >= players.Length)
        {
            currentPlayer = 0;
            //次のプレイヤーがゴール済みならその次のプレイヤーのターンにする
            if (players[currentPlayer].hasGoal)
            {
                NextTurn();
                return;
            }

            foreach (var p in players)
            {
                p.Reset();
            }
        }
        else
        {
            //次のプレイヤーがゴール済みならその次のプレイヤーのターンにする
            if (players[currentPlayer].hasGoal)
            {
                NextTurn();
                return;
            }
        }
    }
    // 酸素数UI
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
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        players[currentPlayer].StartReturn();

        ChoicePanel.SetActive(false);

        isChoosing = false;
    }

    // 何もしないボタン
    public void OnClickChoice2()
    {
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        players[currentPlayer].ContinueForward();

        ChoicePanel.SetActive(false);

        isChoosing = false;
    }

    // アイテムボタン
    public void OnClikItem1()
    {
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        item_position = players[currentPlayer].item_position;
        players[currentPlayer].ItemUp();

        Item_Index.Add(index);
        //アイテムを拾ったマスをみる(マスごとにポイントが変わる)
        TileData tile =
        players[currentPlayer]
        .points[players[currentPlayer].currentIndex]
        .GetComponent<TileData>();
        //プレイヤーのポイントを増やす
        if (tile != null && !tile.collected)
        {
            players[currentPlayer].point += tile.point;

            tile.collected = true;

            UpdateUI();
        }
        //プレイヤーのアイテムを増やす
        if (i < Item_Pos.Length)
        {
            Item_Pos[i] = players[currentPlayer].item_position;
            i++;
        }
        //マスの色が変わる
        tileManager.ChangeTileColor((players[currentPlayer].currentIndex - 5), Color.gray);
        ItemPanel.SetActive(false);
        isDice = true;

        NextTurn();
    }

    // 何もしないボタン
    public void OnClickItem2()
    {
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        ItemPanel.SetActive(false);
        isDice = true;

        NextTurn();
    }
    public void OnClickDrop1()
    {
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        TileData tile =
        players[currentPlayer]
        .points[players[currentPlayer].currentIndex]
        .GetComponent<TileData>();

        for (int t = 0; t < Item_Pos.Length; t++)
        {
            if (Item_Pos[t] == players[currentPlayer].item_position)
            {
                Item_Pos[t] = Vector3.zero;
            }
        }

        players[currentPlayer].item -= 1;
        players[currentPlayer].point -= 30;
        if (players[currentPlayer].point < 0)
        {
            players[currentPlayer].point = 0;
        }

        Item_Index.Remove(players[currentPlayer].currentIndex);
        players[currentPlayer].ItemDown();
        tileManager.ChangeTileColor((players[currentPlayer].currentIndex - 5), Color.white);


        UpdateUI();

        DropPanel.SetActive(false);
        isDice = true;

        NextTurn();
    }
    public void OnClickDrop2()
    {
        SEManager se = FindFirstObjectByType<SEManager>();

        if (se != null)
        {
            se.PlayClick();
        }

        DropPanel.SetActive(false);
        isDice = true;

        NextTurn();
    }
    IEnumerator ShowMessage(string msg)
    {
        messageText.text = msg;

        yield return new WaitForSeconds(2f);

        messageText.text = "";
    }
    public void UpdateUI()
    {
        p_PointText1.text = "" + players[0].point;
        p_PointText2.text = "" + players[1].point;
        p_PointText3.text = "" + players[2].point;
        p_ItemText1.text = "" + players[0].item;
        p_ItemText2.text = "" + players[1].item;
        p_ItemText3.text = "" + players[2].item;
    }
}
