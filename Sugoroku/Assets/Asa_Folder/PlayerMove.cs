using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    public Transform[] points; // マスの位置
    public GameManager g_manager;
    public bool isMoving = false; // 移動中かどうか
    public bool hasGoal = false; // ゴールしたかどうか
    public  int item = 0;
    public Vector3  item_position;
    public int currentIndex = 0;
    public int oxygen = 0;
    public bool isCPU = false;

    public int point = 0;

    private int direction = 1; // 1前進 -1逆走
    public bool isReturning = false; // Buttonフラグ
    private bool hasChosenDirection = true;
    void Start()
    {
        currentIndex = 4;//0
        transform.position = points[currentIndex].position;

        item_position = transform.position;
    }

    public void Move(int steps)
    {
        StartCoroutine(MoveCoroutine(steps));
    }

    // 移動処理
    IEnumerator MoveCoroutine(int steps)
    {
        isMoving = true;

        for (int i = 0; i < steps; i++)
        {
            int nextIndex = currentIndex + direction;

            // 範囲外なら終了
            if (nextIndex >= points.Length || nextIndex < 0)
                break;

            // 他プレイヤーをスキップ
            while (GameManager.Instance.IsTileOccupied(nextIndex))
            {
                nextIndex += direction;

                if (nextIndex >= points.Length || nextIndex < 0)
                    break;
            }

            // 範囲外チェック
            if (nextIndex >= points.Length || nextIndex < 0)
                break;

            // 更新
            currentIndex = nextIndex;

            Vector3 target = points[currentIndex].position;

            // 移動
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    5f * Time.deltaTime
                );

                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }

        isMoving = false;
    }

    public void Item()
    {
        item++;
    }

    // 現在のマス番号を返す
    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    public bool IsReturning()
    {
        return isReturning;
    }
    public bool HasChosenDirection()
    {
        return hasChosenDirection;
    }

    public void PlayerReset()
    {
        item = 0;
        currentIndex = 0;
        oxygen = 0;
        hasGoal = false;
        isMoving = false;

        transform.position = points[currentIndex].position;
    }

    public void StartReturn()
    {
        if (isReturning) return;

        isReturning = true;
        hasChosenDirection = true;

        direction = -1;
    }
    public void ContinueForward()
    {
        hasChosenDirection = true;
    }

    public void Reset()
    {
        if (isReturning) return;

        hasChosenDirection = false;
    }
}