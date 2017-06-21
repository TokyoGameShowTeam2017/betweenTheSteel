using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUI : MonoBehaviour
{
    //IDを保存
    private int ArmId = 0;

    [SerializeField, Tooltip("ArmManagerを入れる")]
    private GameObject ArmManager;


    // Use this for initialization
    void Start()
    {
        ArmId = ArmManager.GetComponent<ArmManager>().GetEnablArmID();
    }

    // Update is called once per frame
    void Update()
    {
        //アームのIDの更新
        ArmIdUpdate();

        //UIをアームに合わせて回転させる
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(0, 0, -ArmId * 90)), 0.08f);
    }

    /// <summary>
    /// ArmIDを更新する
    /// </summary>
    private void ArmIdUpdate()
    {
        ArmId = ArmManager.GetComponent<ArmManager>().GetEnablArmID();
    }

    /// <summary>
    /// 選択中のArmのIDを返す
    /// </summary>
    /// <returns></returns>
    public int GetArmId()
    {
        return ArmId;
    }
}
