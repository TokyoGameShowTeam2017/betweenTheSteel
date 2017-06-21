using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// Handles the LookAt behaviour of Kinematics Bones
    /// </summary>
    public class KinematicSolver
    {
        /// <summary>
        /// Solve the KinematicBone
        /// </summary>
        /// <param name="_bone"></param>
        public void SolveBone(RootIK.KinematicBone _bone)
        {
            if (_bone.weight <= 0) return;
            if(_bone.target == null)
            {
                Debug.LogError("No target was assigned");
                return;
            }

            Vector3 _v0 = RootIK.TransformVector(_bone.axis == Vector3.zero ? Vector3.forward : _bone.axis, _bone.bone.rotation);
            Vector3 _v1 = _bone.target.position - _bone.bone.position;
            Quaternion _targetRot = Quaternion.Lerp(Quaternion.identity, RootIK.RotateFromTo(_v0, _v1), _bone.weight);

            _targetRot = Quaternion.Inverse(_targetRot);
            _bone.bone.rotation = GenericMaths.ApplyQuaternion(_targetRot, _bone.bone.rotation);
        }
    }
}
