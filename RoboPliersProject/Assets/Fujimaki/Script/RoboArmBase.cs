using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboArmBase : MonoBehaviour {

    [SerializeField]
    private int _limitAxisRotation;

    [SerializeField]
    private RoboArm[] _roboArms;

    [SerializeField]
    private GameObject _cameraRig;

    private RoboArm _enableArm;

    private int _enableArmId;

    public GameObject cameraRig { get; private set; }
    public int limitAxisRotation { get; private set; }
    public bool _complianceArm;

    // Use this for initialization
    void Start () {

        cameraRig = _cameraRig;
        limitAxisRotation = _limitAxisRotation;

        _enableArmId = 0;
        SwitchArm(_enableArmId);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _enableArmId++;
            if (_enableArmId > _roboArms.Length - 1) 
            {
                _enableArmId = 0;
            }
            SwitchArm(_enableArmId);
            print(_enableArmId);
        }

        ArmInput input = new ArmInput();
        input.pliers = InputManager.GetArmStretch() > 0;
        input.ArmStretch = InputManager.GetPliersCatch();

        //アーム回転
        input.rotate += InputManager.GetArmNegativeTurn() ? 1 : 0;
        input.rotate -= InputManager.GetArmPositiveTurn() ? 1 : 0;

        //ロボアームベース回転
        _enableArm.ArmUpdate(input);
        if (_enableArm.GetCatchObject() == null)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(0, _enableArmId * 90, 0)), 0.08f);
            //transform.localRotation = Quaternion.Euler(new Vector3(0, _enableArmId * 90, 0));
        }

        //アーム切り替え
        if (InputManager.GetSelectArm().isDown)
        {
            SwitchArm(InputManager.GetSelectArm().id - 1);
        }

	}

    public void SwitchArm(int id)
    {
        _enableArmId = id;
        _enableArm = _roboArms[id];
        _enableArm._roboArmManager = this;
    }

    //現在の有効なアームを取得
    public RoboArm GetEnableArm()
    {
        return _enableArm;
    }

    public int GetRoboArmID()
    {
        return _enableArmId;
    }
}
