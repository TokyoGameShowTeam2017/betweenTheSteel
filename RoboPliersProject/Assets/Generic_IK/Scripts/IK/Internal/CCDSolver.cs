using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// A Cyclic Coordinate Descent Solver
    /// </summary>
    public class CCDSolver
    {

        /// <summary>
        /// Solve the IK chain using CCD method
        /// </summary>
        /// <param name="_IKChain"></param>
        public void SolveCCD(RootIK.Chain _IKChain)
        {

            if (_IKChain.weight <= 0f) return;
            if (_IKChain.joints.Count <= 0) return;

            _IKChain.SetIKPosition(_IKChain.target ? Vector3.zero : _IKChain.GetIKPosition());

            for (int j = 0; j < _IKChain.iterations; j++)
            {
                for (int i = _IKChain.joints.Count - 1; i >= 0; i--)
                {
                    _IKChain.weight = Mathf.Clamp(_IKChain.weight, 0f, 1f);

                    float _weight = _IKChain.weight * _IKChain.joints[i].weight;

                    Vector3 _v0 = _IKChain.GetIKPosition() - _IKChain.joints[i].transform.position;
                    Vector3 _v1 = _IKChain.joints[_IKChain.joints.Count - 1].transform.position - _IKChain.joints[i].transform.position;

                    Quaternion _sourceRotation = _IKChain.joints[i].transform.rotation;
                    Quaternion _targetRotation = Quaternion.Lerp(Quaternion.identity, RootIK.RotateFromTo(_v0, _v1), _weight);

                    _IKChain.joints[i].transform.rotation = Quaternion.Lerp(_sourceRotation, GenericMaths.ApplyQuaternion(_targetRotation, _sourceRotation), _weight);
                }
            }

            _IKChain.joints[_IKChain.joints.Count - 1].transform.rotation = _IKChain.GetIKRotation();
        }


        /// <summary>
        /// A helping method to update 2 IK chains based on the dist to their IK target
        /// </summary>
        /// <param name="_IKChain"></param>
        /// <param name="_IKChain2"></param>
        public void SolveCCD(RootIK.Chain _IKChain, RootIK.Chain _IKChain2)
        {
            //in CCD algorithm, the last chain to be updated will have the biggest influence on the overall results

            if (_IKChain.joints.Count <= 0 || _IKChain2.joints.Count <= 0) return;

            float _distToTarget = Vector3.Distance(_IKChain.GetIKPosition(), _IKChain.joints[0].transform.position);
            float _distToTarget2 = Vector3.Distance(_IKChain2.GetIKPosition(), _IKChain2.joints[0].transform.position);

            if(_distToTarget > _distToTarget2)
            {
                SolveCCD(_IKChain);
                SolveCCD(_IKChain2);
            }else
            {
                SolveCCD(_IKChain2);
                SolveCCD(_IKChain);
            }
        }

    }
}
