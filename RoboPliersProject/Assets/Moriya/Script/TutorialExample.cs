/**==========================================================================*/
/**
 * チュートリアルで使えそうな動きの例
 * 作成者：守屋   作成日：17/06/21
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExample : MonoBehaviour 
{
	/*==所持コンポーネント==*/

    /*==外部設定変数==*/

    /*==内部設定変数==*/
    PlayerTutorialControl control;

    /*==外部参照変数==*/

	void Start() 
	{
        control = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
	}
	
	void Update()
	{
        //元にもどす
        if (Input.GetKeyDown(KeyCode.Z))
        {
            control.SetIsPlayerMove(true);
            control.SetIsCamerMove(true);
            control.SetIsArmMove(true);
            control.SetIsArmCatchAble(true);
            control.SetIsArmRelease(true);


            control.SetIsResetAble(true);
        }

        //プレイヤーを操作不能にし、カメラを右スティックで操作できる
        if (Input.GetKeyDown(KeyCode.X))
        {
            control.SetIsPlayerMove(false);
            control.SetIsCamerMove(true);

            control.SetIsResetAble(false);
        }
        //プレイヤーとカメラを操作不能にする
        if (Input.GetKeyDown(KeyCode.C))
        {
            control.SetIsPlayerMove(false);
            control.SetIsCamerMove(false);
        }

        //プレイヤーとカメラを操作不能にし、カメラを自由に動かす
        if (Input.GetKeyDown(KeyCode.V))
        {
            control.SetIsPlayerMove(false);
            control.SetIsCamerMove(false);

            StartCoroutine(CamMove());
        }


        //アームだけ動かす（エイムアシストをチュートリアルする時等）
        if (Input.GetKeyDown(KeyCode.N))
        {
            control.SetIsPlayerMove(false);
            control.SetIsCamerMove(false);
            control.SetIsArmMove(true);
        }

        //掴み中に離せなくする
        if (Input.GetKeyDown(KeyCode.M))
        {
            control.SetIsPlayerMove(true);
            control.SetIsCamerMove(true);
            control.SetIsArmMove(true);
            control.SetIsArmRelease(false);
            control.SetIsArmCatchAble(true);
        }

        //掴めなくする（離せはする）
        if (Input.GetKeyDown(KeyCode.P))
        {
            control.SetIsPlayerMove(true);
            control.SetIsCamerMove(true);
            control.SetIsArmMove(true);
            control.SetIsArmRelease(true);
            control.SetIsArmCatchAble(false);
        }

        ////"TutorialTarget"という名前のオブジェクトをエイムアシストの対象にしているかを調べる
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    print(control.GetIsAimAssistName("TutorialTarget"));
        //}

        if (Input.GetKeyDown(KeyCode.T))
        {
            //control.SetIsArmCatchAble(false);
            //control.SetIsArmRelease(false);
            control.SetIsArmStretch(false);
            //control.SetIsResetAble(false);

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //control.SetIsArmCatchAble(true);
            control.SetIsArmStretch(true);
            //control.SetIsResetAble(true);
        }
	}


    IEnumerator CamMove()
    {
        float timer = 0.0f;
        while(timer < 5.0f)
        {
            //適当に回転、移動
            timer += Time.deltaTime;
            control.GetPlayerCameraTr().eulerAngles += new Vector3(0, 90, 0) * Time.deltaTime;
            control.GetPlayerCameraTr().position += new Vector3(0, 0, -1) * Time.deltaTime;
            yield return null;
        }

        //カメラの移動計算に補完がかかってるので、
        //状態を戻すだけである程度補完がかかりながらカメラの位置が戻る
        control.SetIsPlayerMove(true);
        control.SetIsCamerMove(true);
        yield break;
    }
}
