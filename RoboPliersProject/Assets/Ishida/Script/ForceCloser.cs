using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceCloser : MonoBehaviour
{
    // 操作が無い場合に自動で終了するまでの秒数
    static readonly float AutoCloseDuration = 120;
    // 最後に入力があった時間
    float lastInputTime;

    private float m_PvTimer;
    [SerializeField, Tooltip("何秒後にPVシーンに遷移するか")]
    private float m_PvSceneTime;

    private enum State
    {
        PvStartState,
        ExitState,
        DoNothing,

        None
    }

    [SerializeField, Tooltip("強制終了orPV再生ループor何もしない")]
    private State m_State;

    void Awake()
    {
        if (FindObjectsOfType<ForceCloser>().Length > 1)
        {
            // 自分の他に ForceCloser が既に存在する場合は、自分を消す
            Destroy(gameObject);
            return;
        }
        // 全シーンにまたがって生存する（シーン切り替えで破棄されない）ようにする
        DontDestroyOnLoad(gameObject);
        lastInputTime = Time.time;
    }
    void Update()
    {
        if (m_State == State.ExitState)
        {
            // XBOX コントローラーの BACK ボタンと START ボタン同時押し
            // または
            // キーボードのエスケープキーでゲームを強制終了する
            if ((Input.GetKey(KeyCode.JoystickButton6) && Input.GetKey(KeyCode.JoystickButton7))
            || Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
            // 何かしらの入力があれば、最後に入力があった時間として現在の時間を記録する
            if (InputAnyKey())
            {
                lastInputTime = Time.time;
            }
            // 最後に入力があった瞬間からの経過時間が、指定秒数を超えたら、強制終了する
            if ((Time.time - lastInputTime) >= AutoCloseDuration)
            {
                Application.Quit();
                return;
            }
        }
        else if (m_State == State.PvStartState)
        {
            // 何かしらの入力があれば、最後に入力があった時間として現在の時間を記録する
            if (InputAnyKey())
            {
                lastInputTime = Time.time;
            }
            // 最後に入力があった瞬間からの経過時間が、指定秒数を超えたら、タイトル画面へ
            if ((Time.time - lastInputTime) >= AutoCloseDuration)
            {
                SceneManager.LoadScene("title");
            }

            MigrationPvScene();
        }
    }


    //何秒かタイトルで放置したらPVシーンへ
    public void MigrationPvScene()
    {
        if (GameObject.Find("SceneCollection") == null) return;
        if (GameObject.Find("SceneCollection").GetComponent<SceneCollection>().GetSceneState() == 0)
        {
            m_PvTimer += Time.deltaTime;
            if (m_PvTimer >= m_PvSceneTime)
                SceneManager.LoadScene("movie");
        }
        else
        {
            m_PvTimer = 0;
        }
    }



    // 何らかの入力があればtrue、無ければfalse を返却する
    bool InputAnyKey()
    {
        return Input.anyKey ||
        Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3f ||
        Mathf.Abs(Input.GetAxis("Vertical")) > 0.3f ||
        Mathf.Abs(Input.GetAxis("Mouse X")) > 0 ||
        Mathf.Abs(Input.GetAxis("Mouse Y")) > 0 ||
        Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0;
    }
}