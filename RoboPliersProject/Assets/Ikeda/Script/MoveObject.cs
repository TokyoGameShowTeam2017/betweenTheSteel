using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private enum MoveType
    {
        Up,
        Down,
        Right,
        Left,
        Front,
        Back
    }

    [SerializeField, Tooltip("動く向きの設定")]
    private MoveType moveType;

    [SerializeField, Tooltip("速度の設定")]
    private float speed = 1.0f;

    [SerializeField, Tooltip("止まる距離の設定")]
    private float stopDistances = 0.0f;

    [SerializeField, Tooltip("往復する時チェックする")]
    private bool isRepeat = false;

    [SerializeField, Tooltip("往復する時自動で戻ってくるか")]
    private bool isAutoBuck = false;

    public bool isMotion = false;

    //リピートが終わったか
    private bool repeatEnd = false;

    //リピートしないとき動き終わったか
    private bool isEnd = false;

    //動く前のポジションを保存する
    private Vector3 beforePosition;

    //動いた距離を保存する
    private float length = 0.0f;

    //最初のポジションを保存する
    private Vector3 startPosition;

    [SerializeField, Tooltip("何秒後に自動で戻るか設定")]
    private int startTime = 0;

    private float secondTimer = 0.0f;

    private float stopDistances2 = 0.0f;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AutoBack();
        MovingObject();
    }


    /// <summary>
    ///自動で戻る 
    /// </summary>
    private void AutoBack()
    {
        if (isAutoBuck && repeatEnd)
        {
            if ((startTime * 60) > secondTimer)
            {
                secondTimer++;
            }
            else if (repeatEnd)
            {
                secondTimer = 0.0f;
                isMotion = true;
            }
        }
    }
    
    /// <summary>
    /// Repeatする時の処理
    /// </summary>
    private void RepeatMoving()
    {
        if (repeatEnd == false)
        {
            if (length >= stopDistances)
            {
                isMotion = false;
                speed *= -1;
                repeatEnd = true;
                isEnd = true;
                length = 0.0f;
                stopDistances2 += Vector3.Magnitude(beforePosition - startPosition);
            }
        }
        else
        {
            if (length >= stopDistances2)
            {
                speed *= -1;
                isMotion = false;
                repeatEnd = false;
                isEnd = true;
                length = 0.0f;
                stopDistances2 = 0.0f;
            }
        }
    }


    /// <summary>
    /// MoveObjectの動き全般
    /// </summary>
    private void MovingObject()
    {
        if (isMotion == true)
        {
            beforePosition = transform.position;

            switch (moveType)
            {
                //上
                case MoveType.Up:
                    //Repeatするときの処理
                    if (isRepeat == true)
                    {
                        transform.Translate(0.0f, speed * Time.deltaTime, 0.0f);

                        RepeatMoving();
                    }

                    //Repeatしないときの処理
                    else
                    {
                        transform.Translate(0.0f, speed * Time.deltaTime, 0.0f);
                        if (length >= stopDistances)
                        {
                            speed = 0.0f;
                            isMotion = false;
                            isEnd = true;
                        }
                    }
                    break;

                //右
                case MoveType.Right:
                    //Repeatするときの処理
                    if (isRepeat == true)
                    {
                        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
                        RepeatMoving();
                    }

                    //Repeatしないときの処理
                    else
                    {
                        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);

                        if (length >= stopDistances)
                        {
                            speed = 0.0f;
                            isMotion = false;
                            isEnd = true;
                        }
                    }
                    break;

                //下
                case MoveType.Down:
                    //Repeatするときの処理
                    if (isRepeat == true)
                    {
                        transform.Translate(0.0f, -speed * Time.deltaTime, 0.0f);
                        RepeatMoving();
                    }

                    //Repeatしないときの処理
                    else
                    {
                        transform.Translate(0.0f, -speed * Time.deltaTime, 0.0f);

                        if (length >= stopDistances)
                        {
                            speed = 0.0f;
                            isMotion = false;
                            isEnd = true;
                        }
                    }
                    break;

                //左
                case MoveType.Left:
                    //Repeatするときの処理
                    if (isRepeat == true)
                    {
                        transform.Translate(-speed * Time.deltaTime, 0.0f, 0.0f);

                        RepeatMoving();
                    }

                    //Repeatしないときの処理
                    else
                    {
                        transform.Translate(-speed * Time.deltaTime, 0.0f, 0.0f);

                        if (length >= stopDistances)
                        {
                            speed = 0.0f;
                            isMotion = false;
                            isEnd = true;
                        }
                    }
                    break;

                //前
                case MoveType.Front:
                    //Repeatするときの処理
                    if (isRepeat == true)
                    {
                        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
                        RepeatMoving();
                    }

                    //Repeatしないときの処理
                    else
                    {
                        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);

                        if (length >= stopDistances)
                        {
                            speed = 0.0f;
                            isMotion = false;
                            isEnd = true;
                        }
                    }
                    break;

                //後
                case MoveType.Back:
                    //Repeatするときの処理
                    if (isRepeat == true)
                    {
                        transform.Translate(0.0f, 0.0f, -speed * Time.deltaTime);
                        RepeatMoving();
                    }

                    //Repeatしないときの処理
                    else
                    {
                        transform.Translate(0.0f, 0.0f, -speed * Time.deltaTime);

                        if (length >= stopDistances)
                        {
                            speed = 0.0f;
                            isMotion = false;
                            isEnd = true;
                        }
                    }
                    break;
            }
            length += Vector3.Magnitude(beforePosition - transform.position);
        }
    }


    public void MoveEnd()
    {
        isEnd = false;
    }

    /// <summary>
    /// Objectが動き終わったか
    /// </summary>
    /// <returns></returns>
    public bool IsMoveEnd()
    {
        return isEnd;
    }
}