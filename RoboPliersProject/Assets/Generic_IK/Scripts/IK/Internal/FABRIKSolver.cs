using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// a Forward And Backward Reaching Inverse Kinematics solver
    /// </summary>
    public class FABRIKSolver
    {

        /*
         * IMPORTANT NOTE !!! 
         * the local bone weight will not effect the influence of the solver on that bone in FABRIK
         * because the FABRIK is a pure Vectors maths problem and it depends on the change in pos of the end effector to derive the other IK joints pos
         * credits and for more info: http://www.andreasaristidou.com/FABRIK.html;
         * */

        private RootIK.Chain chain;


        /// <summary>
        /// Solve the IK chain using FABRIK method
        /// </summary>
        /// <param name="_IKChain"></param>
        public void SolveFABRIK(RootIK.Chain _IKChain)
        {
            chain = _IKChain;

            if (_IKChain.weight <= 0) return;
            if (_IKChain.joints.Count <= 0) return;

            for(int i = 0; i < _IKChain.joints.Count - 1; i++)
            {
                //reset from last iteration
                _IKChain.joints[i].solvePos = _IKChain.joints[i].transform.position;
            }

            for (int i = 0; i < _IKChain.iterations; i++)
            {
                SolveInward();
                SolveOutward();
                CorrectRotation();
            }
        }


        /// <summary>
        /// solve the joints backward
        /// </summary>
        private void SolveInward()
        {
            chain.joints[chain.joints.Count - 1].solvePos = GenericMaths.Interpolate(chain.GetEndEffector().position, chain.GetIKPosition(), chain.weight);

            for (int i = chain.joints.Count -2; i >= 0; i--)
            {
                Vector3 _v1 = chain.joints[i + 1].solvePos;
                Vector3 _v0 = chain.joints[i].solvePos - _v1;

                _v0.Normalize();
                _v0 *= Vector3.Distance(chain.joints[i].transform.position, chain.joints[i + 1].transform.position);

                chain.joints[i].solvePos = GenericMaths.Interpolate(chain.joints[i].transform.position, _v0 + _v1, chain.weight);
            }
        }

        /// <summary>
        /// solve the joints forward
        /// </summary>
        private void SolveOutward()
        {
            chain.joints[0].solvePos = chain.joints[0].transform.position;

            for (int i = 1; i < chain.joints.Count; i++)
            {
                Vector3 _v1 = chain.joints[i - 1].solvePos;
                Vector3 _v0 = chain.joints[i].solvePos - _v1;

                _v0.Normalize();
                _v0 *= Vector3.Distance(chain.joints[i].transform.position, chain.joints[i - 1].transform.position);

                chain.joints[i].solvePos = GenericMaths.Interpolate(chain.joints[i].transform.position, _v0 + _v1, chain.weight);
            }

        }

        /// <summary>
        /// correct the orientation of the bones after the solve
        /// </summary>
        private void CorrectRotation()
        {
            for (int i = 0; i < chain.joints.Count - 1; i++)
            {
                //apply the new pos
                chain.joints[i].transform.position = chain.joints[i].solvePos;

                Vector3 _targetDir = chain.joints[i + 1].transform.position - chain.joints[i].transform.position;
                Quaternion _sourceRot = chain.joints[i].transform.rotation;
                Quaternion _targetRot = RootIK.RotateFromTo(RootIK.TransformVector(Vector3.up, _sourceRot), _targetDir);

                _targetRot = Quaternion.Inverse(_targetRot);
                chain.joints[i].transform.rotation = Quaternion.Lerp(_sourceRot, GenericMaths.ApplyQuaternion(_targetRot, _sourceRot), chain.weight);
            }

            //Bounce back to the 1st bone before the endeffector and correct its rotation
            Vector3 _t = chain.GetIKPosition() - chain.joints[chain.joints.Count - 2].transform.position;
            Vector3 _t2 = RootIK.TransformVector(Vector3.up, chain.joints[chain.joints.Count - 2].transform.rotation);
            Quaternion _s = chain.joints[chain.joints.Count - 2].transform.rotation;
            Quaternion _tr = RootIK.RotateFromTo(_t, _t2);
            chain.joints[chain.joints.Count - 2].transform.rotation = Quaternion.Lerp(_s, GenericMaths.ApplyQuaternion(_tr, _s), chain.weight);

            chain.joints[chain.joints.Count - 1].transform.rotation = chain.GetIKRotation();   //apply end effector rotation

        }
    }
}
