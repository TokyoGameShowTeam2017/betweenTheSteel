using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics.Dynamics;

public class IKLegController : MonoBehaviour {

    private Animator _animator;
    private CharacterController _characterController;
    private PlayerManager _playerManager;
    private GameObject[] _rayDefaultObject;

    [SerializeField]
    private CameraMove _cameraMove;

    [SerializeField]
    private IKLeg[] _ikLegTargets;

    [SerializeField]
    private GameObject[] _nails;

    private Quaternion[] _defaultNailRotation;


    void Start()
    {
        //各種コンポーネント取得
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerManager = GetComponent<PlayerManager>();

        //レイの発射原点を生成
        _rayDefaultObject = new GameObject[_ikLegTargets.Length];

        _defaultNailRotation = new Quaternion[_nails.Length];
        for (int i = 0; i < _defaultNailRotation.Length; i++) 
        {
            _defaultNailRotation[i] = _nails[i].transform.localRotation;
        }

    }
	
	void Update ()
    {
        Vector3 _input = InputManager.GetMove();
        _input = Quaternion.Euler(0, 0, _cameraMove.transform.eulerAngles.y - transform.eulerAngles.y) * _input;

        //設置していなかったらジャンプモーション
        _animator.SetBool("Jump", !Physics.Raycast(transform.position, -Vector3.up, 1.5f));
        Debug.DrawLine(transform.position, transform.position - Vector3.up * 1f, Color.yellow);

        //キャラクターコントローラーが移動していなければ移動をゼロにする
        if (_characterController.velocity.magnitude < 0.1f || _playerManager.IsMove)
        {
            _input = Vector3.zero;
        }

        //アニメータのパラメーターを更新
        _animator.SetFloat("MoveAxisX", _input.x);
        _animator.SetFloat("MoveAxisZ", _input.y);

        //ターンの回転を計算
        _animator.SetFloat("TurnRotate", _playerManager.GetArmAngleOver()/10);
        _animator.SetBool("Turn", Mathf.Abs(_playerManager.GetArmAngleOver()) > 1);
    }

    private void LateUpdate()
    {
        //それぞれの足からレイを飛ばしてIK補正位置を計算
        foreach(var i in _ikLegTargets)
        {
            Vector3 _offset = Vector3.up * 3;
            float _legYOffset = i.ikTargetBone.transform.position.y-i.defaultObject.transform.position.y;
            RaycastHit hit;
            int mask = LayerMask.NameToLayer("ArmAndPliers");

            if (Physics.Raycast(i.ikTargetBone.transform.position + _offset, -Vector3.up, out hit, _offset.magnitude*10,mask))
            {
                i.TranslateLeg(hit.point + new Vector3(0, _legYOffset, 0));
            }
            else
            {
                i.TranslateLeg(i.ikTargetBone.transform.position);
            }

            Debug.DrawLine(i.ikTargetBone.transform.position + _offset, hit.point + new Vector3(0, _legYOffset, 0), Color.yellow);
        }

        //手動でIKUpdateを呼ぶ(実験的)
        GetComponent<InverseKinematics>().SolveIK();

        for (int i = 0; i < _defaultNailRotation.Length; i++)
        {
            _nails[i].transform.localRotation = _defaultNailRotation[i];
        }
    }
}
