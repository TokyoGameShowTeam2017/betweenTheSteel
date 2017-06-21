using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmInput
{
    public bool pliers;
    public float ArmStretch;
    public float rotate;
}

public class RoboArm : MonoBehaviour {

    [SerializeField]
    private RoboPliers _roboPliers;

    [SerializeField]
    private GameObject _roboArmMesh;

    [Space(5)]
    [SerializeField]
    private GameObject _armBone;
    [SerializeField]
    private float _maxArmLength;

    private float _armlength;
    private Vector3 _defaultArmPosition;

    public RoboArmBase _roboArmManager;

	void Start () {
        _defaultArmPosition = _armBone.transform.localPosition;
        _roboPliers.roboArm = this;
	}
	
    //アームを更新
	public void ArmUpdate (ArmInput input) {

        PlayerMove player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        _roboArmMesh.transform.eulerAngles = _roboArmManager.cameraRig.transform.eulerAngles - player.transform.eulerAngles;
        
        Vector3 clampAngle = Vector3.zero;
        Vector3 _roboArmMeshEuler = _roboArmMesh.transform.eulerAngles;

        //アームが180以上回らないように角度を制限
        clampAngle.x = Mathf.Clamp(Mathf.Abs(_roboArmMeshEuler.x - 180), 90, 180) * ((_roboArmMeshEuler.x - 180) < 0 ? 1 : -1);
        clampAngle.y = Mathf.Clamp(Mathf.Abs(_roboArmMeshEuler.y - 180), 90, 180) * ((_roboArmMeshEuler.y - 180) > 0 ? 1 : -1);

        //クランプした回転を適用
        _roboArmMesh.transform.localEulerAngles = clampAngle;

        //ペンチを更新
        _roboPliers.PliersUpdate(input.pliers);

        //アームの縦回転
        _armBone.transform.Rotate(0, 0, Time.deltaTime * 100 * input.rotate);

        //アームを伸ばす
        _armlength += (input.ArmStretch - _armlength) / 10.0f;
        _armlength = Mathf.Clamp(_armlength, 0, 1);


        _armBone.transform.localPosition = _defaultArmPosition + Vector3.forward  *Mathf.Lerp(0, _maxArmLength, _armlength);

    }

    //カメラの角度とロボアームの角度差を取得
    public float GetArmYawClampOver()
    {
        return _roboArmManager.cameraRig.transform.eulerAngles.y - _roboArmMesh.transform.eulerAngles.y;
    }

    //アームがキャッチしているオブジェクトを取得
    public CatchObject GetCatchObject()
    {
        return _roboPliers.GetCatchObject();
    }

    //アームのロボペンチを取得
    public RoboPliers GetRoboPliers()
    {
        return _roboPliers;
    }
}
