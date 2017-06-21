using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// Analytical Solver for simple hinge joints (legs & arms)
    /// </summary>
    public class AnalyticalSolver
    {

        //Internal
        private float upperLength;
        private float lowerLength;
        private float systemLength;


        /// <summary>
        /// Prepare the solver and solve the IK problem analytically with specific axis of rotation
        /// (the chain must contain 2 joints, no more and no less)
        /// </summary>
        /// <param name="_hingeChain">the chain</param>
        /// <param name="_direction">direction of the player</param>
        /// <param name="_axis">axis of rotation for the 2nd joint (the Hinge joint)</param>
        public void SolveAnalytically(RootIK.Chain _hingeChain, Vector3 _direction, Vector3 _axis)
        {
            if (_hingeChain.joints.Count > 3) return;
            if (_hingeChain.iterations <= 0) return;
            if (_hingeChain.joints.Count <= 0) return;


            //calculate bone length;
            upperLength = Vector3.Distance(_hingeChain.joints[0].transform.position, _hingeChain.joints[1].transform.position);
            lowerLength = Vector3.Distance(_hingeChain.joints[1].transform.position, _hingeChain.joints[2].transform.position);
            systemLength = Vector3.Distance(_hingeChain.joints[0].transform.position, _hingeChain.GetIKPosition());

            //lowerjoint 1DOF
            float _angle = GenericMaths.Formula(upperLength, lowerLength, systemLength) + Mathf.PI * Mathf.Rad2Deg;
            if (_axis == Vector3.zero)
            _axis = Vector3.Cross(RootIK.TransformVector(Vector3.up, _hingeChain.joints[1].transform.rotation), _direction);
            else _axis = Vector3.Cross(RootIK.TransformVector(_axis, _hingeChain.joints[1].transform.rotation), _direction);
            Quaternion _src = _hingeChain.joints[1].transform.rotation;
            Quaternion _t = GenericMaths.QuaternionFromAngleAxis(_axis, _angle + Mathf.Acos(Mathf.Clamp(_src.w, -1f, 1f)) * 10f);
            Quaternion _finalRot = Quaternion.Lerp(Quaternion.identity, _t, _hingeChain.weight);
            _hingeChain.joints[1].transform.rotation = GenericMaths.ApplyQuaternion(_finalRot, _src);


            //Upperjoint 3DOF
            Vector3 _v1 = _hingeChain.GetIKPosition() - _hingeChain.joints[0].transform.position;
            Vector3 _v2 = _hingeChain.joints[2].transform.position - _hingeChain.joints[0].transform.position;
            Quaternion _src2 = _hingeChain.joints[0].transform.rotation;
            Quaternion _t2 = RootIK.RotateFromTo(_v2, _v1);
            Quaternion _finalRot2 = Quaternion.Lerp(Quaternion.identity, _t2, _hingeChain.weight);
            _hingeChain.joints[0].transform.rotation = GenericMaths.ApplyQuaternion(Quaternion.Inverse(_finalRot2), _src2);

            //foot 6DOF
            _hingeChain.GetEndEffector().rotation = _hingeChain.GetIKRotation();

        }
    }
}
