using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArror : MonoBehaviour
{
    [SerializeField, Tooltip("アームマネジャーを入れる")]
    private GameObject ArmManager;

    //テスト用の数値
    [SerializeField, Range(0, 100)]
    private int TestValue;
    //最大回転角度
    private int maxAngle = -40;
    //最小回転角度
    private int minAngle = 40;


    //アームIDを保存
    private int mArmId;

    //アームがObjectを掴んでいるかを保存
    private bool mIsArmCatching;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ArrorAction();
    }


    private void ArrorAction()
    {
        //一番上に来たら起動
        if (ArmManager.GetComponent<ArmManager>().GetEnablArmID() == 0)
        {
            //transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, TestValue), 0.05f);

            //    float rotateZ = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
            //    float angleZ = Mathf.Clamp(TestValue + rotateZ, minAngle, maxAngle);

            //    transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, angleZ), 0.05f);  

            
        }
        else
        {
            //挟んでいるけど選択中ではないとき
            if (mIsArmCatching)
            {

            }
            else
            {
                //何も挟んでいない時は初期位置に戻す
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 40), 0.05f);
            }
        }
    }
}
