/**==========================================================================*/
/**
 * 入力状態の取得はここから行う
 * 作成者：守屋   作成日：16/04/11
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StickState
{
    Up,
    Down,
    Left,
    Right,

    None
}

public class InputManager : MonoBehaviour 
{
    /// <summary>
    /// アーム選択の入力時に返す構造体
    /// </summary>
    public struct InputArms
    {
        //何らかのアーム選択ボタンが押されているか？
        public bool isDown;
        //押されたアーム選択ボタンのid(1～4) 押されていない場合は0
        public int id;
    }

    /// <summary>
    /// WASDキー入力方向を返す
    /// </summary>
    public static Vector2 GetWASD()
    {
        float h = 0.0f;
        float v = 0.0f;
        Vector2 result = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            h -= 1.0f;
        if (Input.GetKey(KeyCode.D))
            h += 1.0f;
        if (Input.GetKey(KeyCode.W))
            v += 1.0f;
        if (Input.GetKey(KeyCode.S))
            v -= 1.0f;


        if (h == 0.0f && v == 0.0f)
            return result;
        else
        {
            result = new Vector2(h, v);
            result.Normalize();
        }

        return result;
    }

    /// <summary>
    /// 十字キー入力方向を返す
    /// </summary>
    public static Vector2 GetFourWayKey()
    {
        float h = 0.0f;
        float v = 0.0f;
        Vector2 result = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            h -= 1.0f;
        else if (Input.GetKey(KeyCode.RightArrow))
            h += 1.0f;
        if (Input.GetKey(KeyCode.UpArrow))
            v += 1.0f;
        else if (Input.GetKey(KeyCode.DownArrow))
            v -= 1.0f;


        if (h == 0.0f && v == 0.0f)
            return result;
        else
        {
            result = new Vector2(h, v);
            result.Normalize();
        }

        return result;
    }

    /// <summary>
    /// ZCキー入力(-1～1)を返す（テスト用）
    /// </summary>
    public static float GetZC()
    {
        float h = 0.0f;
        if (Input.GetKey(KeyCode.Z))
            h -= 1.0f;
        else if (Input.GetKey(KeyCode.C))
            h += 1.0f;

        return h;
    }

    /// <summary>
    /// 左スティックとWASDキーの入力方向を返す
    /// </summary>
    public static Vector2 GetMove()
    {
        float h = Input.GetAxis(InputPadType.Instance.TypeName + "LeftStickH");
        float v = Input.GetAxis(InputPadType.Instance.TypeName + "LeftStickV");

        Vector2 vec = new Vector2(h, v);
        if(vec.magnitude <= 0.0f)
        {
            h = Input.GetAxis("LeftStickH");
            v = Input.GetAxis("LeftStickV");
            vec = new Vector2(h, v);
        }

        return vec;
    }

    public static StickState GetStick()
    {
        float vecX = GetMove().x;
        float vecY = GetMove().y;

        if (vecX > 0.3f) return StickState.Right;
        if (vecX < -0.3f) return StickState.Left;
        if (vecY > 0.3f) return StickState.Up;
        if (vecY < -0.3f) return StickState.Down;

        return StickState.None;
    }

    /// <summary>
    /// 右スティック入力方向を返す
    /// </summary>
    public static Vector2 GetCameraMove()
    {
        float h = Input.GetAxis(InputPadType.Instance.TypeName + "RightStickH");
        float v = Input.GetAxis(InputPadType.Instance.TypeName + "RightStickV");
        
        Vector2 vec = new Vector2(h, v);
        if (vec.magnitude <= 0.0f)
        {
            h = Input.GetAxis("RightStickH");
            v = Input.GetAxis("RightStickV");
            vec = new Vector2(h, v);
        }

        return vec;
    }

    /// <summary>
    /// アーム選択入力を返す
    /// </summary>
    public static InputArms GetSelectArm()
    {
        InputArms result;
        result.isDown = false;
        result.id = 0;

        if (Input.GetButtonDown(InputPadType.Instance.TypeName + "Arm1"))
            result.id = 1;
        if (Input.GetButtonDown(InputPadType.Instance.TypeName + "Arm2"))
            result.id = 2;
        if (Input.GetButtonDown(InputPadType.Instance.TypeName + "Arm3"))
            result.id = 3;
        if (Input.GetButtonDown(InputPadType.Instance.TypeName + "Arm4"))
            result.id = 4;

        if (result.id != 0)
            result.isDown = true;

        return result;
    }

    /// <summary>
    /// Ｌ２トリガー(キーボードのＵ)の入力値(0～1)を返す　アームの伸び
    /// </summary>
    public static float GetArmStretch()
    {
        float result = (Input.GetAxis(InputPadType.Instance.TypeName + "Left2") + 1.0f) / 2.0f;
        if(result <= 0.0f)
            result = (Input.GetKey(KeyCode.U) ? 1.0f : 0.0f);

        print(result);

        return result;
    }


    /// <summary>
    /// Ｌ２トリガー(キーボードのＵ)の入力を返す　アームの伸び
    /// </summary>
    public static bool GetArmStretchEasyMode()
    {
        if(InputPadType.Instance.TypeName == "PS4")
        {
            float result = Input.GetAxisRaw(InputPadType.Instance.TypeName + "Left2");
            print(result);
            if (result > -1)
                return true;
            else
                return (Input.GetKey(KeyCode.U) ? true : false);

        }
        else if (InputPadType.Instance.TypeName == "XBOX")
        {
            float result = Input.GetAxis(InputPadType.Instance.TypeName + "Left2");
            if (result > 0.0f)
                return true;
            else
                return Input.GetKey(KeyCode.U) ? true : false;
        }
        else
        {
            return Input.GetKey(KeyCode.U) ? true : false;
        }
    }



    /// <summary>
    /// Ｒ２トリガー(キーボードのＯ)の入力値(0～1)を返す アーム掴み
    /// </summary>
    public static float GetPliersCatch()
    {
        float result = (Input.GetAxis(InputPadType.Instance.TypeName + "Right2") + 1.0f) / 2.0f;

        if (result <= 0.0f)
            result = (Input.GetKey(KeyCode.O) ? 1.0f : 0.0f);

        return result;
    }

    /// <summary>
    /// Ｒ２トリガー(キーボードのＯ)の入力を返す　アーム、ペンチ掴み動作
    /// </summary>
    public static bool GetCatchingEasyMode()
    {
        if (InputPadType.Instance.TypeName == "PS4")
        {
            float result = Input.GetAxisRaw(InputPadType.Instance.TypeName + "Right2");
            if (result > -1)
                return true;
            else
                return (Input.GetKey(KeyCode.O) ? true : false);
        }
        else if (InputPadType.Instance.TypeName == "XBOX")
        {
            float result = Input.GetAxis(InputPadType.Instance.TypeName + "Right2");
            if (result < 0.0f)
                return true;
            else
                return Input.GetKey(KeyCode.O) ? true : false;
        }
        else
        {
            return Input.GetKey(KeyCode.O) ? true : false;
        }
    }

    /// <summary>
    ///左スティック押込み（ＬＳｈｉｆｔ）トリガー入力を返す　ジャンプ
    /// </summary>
    public static bool GetJump()
    {
        return Input.GetButtonDown(InputPadType.Instance.TypeName + "LeftStickPush");
    }

    /// <summary>
    ///右スティック押込み（スペースキー）入力を返す　ダッシュ
    /// </summary>
    public static bool GetDash()
    {
        return Input.GetButton(InputPadType.Instance.TypeName + "RightStickPush");
    }

    
    /// <summary>
    ///Ｌ１（７キー）の入力を返す　アーム反時計回転
    /// </summary>
    public static bool GetArmNegativeTurn()
    {
        return Input.GetButton(InputPadType.Instance.TypeName + "Left1");
    }

    /// <summary>
    ///Ｒ１（９キー）の入力を返す　アーム時計回転
    /// </summary>
    public static bool GetArmPositiveTurn()
    {
        return Input.GetButton(InputPadType.Instance.TypeName + "Right1");
    }


}
