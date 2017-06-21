using UnityEngine;
using System.Collections.Generic;

namespace Generics.Dynamics
{
    /// <summary>
    /// Essential methodes for the core of the IK system
    /// </summary>
    public class RootIK
    {
        /// <summary>
        /// the _source vector will rotate to point at the _target vector
        /// </summary>
        /// <param name="_source"></param>
        /// <param name="_target"></param>
        /// <returns></returns>
        public static Quaternion RotateFromTo(Vector3 _source, Vector3 _target)
        {
            _source.Normalize();
            _target.Normalize();

            Quaternion q = GenericMaths.QuaternionFromAngleAxis(Vector3.Cross(_source, _target).normalized, GenericMaths.VectorsAngle(_source, _target));

            return Quaternion.Inverse(q);
        }

        /// <summary>
        /// Rotate a vector by a quaternion
        /// </summary>
        /// <param name="_v"></param>
        /// <param name="_q"></param>
        /// <returns>the vector's new coordinates after being transformed by a quaternion</returns>
        public static Vector3 TransformVector(Vector3 _v, Quaternion _q)
        {
            Quaternion _qv = new Quaternion(_v.x, _v.y, _v.z, 0f);
            Quaternion _qr = _q * _qv * Quaternion.Inverse(_q);
            return new Vector3(_qr.x, _qr.y, _qr.z);
        }


        /// <summary>
        /// A method to enforce rotation limits through the twist and swing logic
        /// </summary>
        /// <param name="_limit"></param>
        /// <param name="_target"></param>
        /// <returns></returns>
        public static Quaternion LimitQuaternion(Joints.QuaternionLimits _limit, Quaternion _target)
        {
            Quaternion _q = _target;

            switch (_limit)
            {
                case Joints.QuaternionLimits.twistX_SwingYZ:
                    Quaternion _twistX = new Quaternion(_q.w / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.x, 2f)), _q.x / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.x, 2f)), 0f, 0f);
                    Quaternion _swingYZ = new Quaternion(Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.x, 2f)), 0f, (_q.w * _q.y) - (_q.x * _q.z) / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.x, 2f)), (_q.w * _q.z) - (_q.x * _q.y) / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.x, 2f)));
                    _q = _swingYZ * _twistX;
                    return _q;
                case Joints.QuaternionLimits.twistY_SwingXZ:
                    Quaternion _twistY = new Quaternion(_q.w / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.y, 2f)), 0f, _q.y / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.y, 2f)), 0f);
                    Quaternion _swingXZ = new Quaternion(Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.y, 2f)), 0f, (_q.w * _q.x) - (_q.y * _q.z) / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.y, 2f)), (_q.w * _q.z) - (_q.x * _q.y) / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.y, 2f)));
                    _q = _swingXZ * _twistY;
                    return _q;
                case Joints.QuaternionLimits.twistZ_swingXY:
                    Quaternion _twistZ = new Quaternion(_q.w / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.z, 2f)), 0f, 0f, _q.z / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.z, 2f)));
                    Quaternion _swingXY = new Quaternion(Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.z, 2f)), 0f, (_q.w * _q.x) - (_q.y * _q.z) / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.z, 2f)), (_q.w * _q.y) - (_q.x * _q.z) / Mathf.Sqrt(Mathf.Pow(_q.w, 2f) + Mathf.Pow(_q.z, 2f)));
                    _q = _swingXY * _twistZ;
                    return _q;
            }

            return _q;
        }

        /// <summary>
        /// Creates a rotation which will always be perpendicular to the _normal and parallel to the surface
        /// </summary>
        /// <param name="_normal"></param>
        /// <returns></returns>
        public static Quaternion RotationLookAt(Vector3 _normal)
        {
            Quaternion _local = Quaternion.identity;
            _local = Quaternion.LookRotation(-_normal);
            return _local;
        }


        /// <summary>
        /// the definition of a joint
        /// </summary>
        [System.Serializable]
        public class Joints
        {
            public Transform transform;
            [Range(0f, 1f)] public float weight;
            [HideInInspector] public Vector3 solvePos;

            //options for QuaternionLimits
            public enum QuaternionLimits { twistX_SwingYZ, twistY_SwingXZ, twistZ_swingXY}
            [HideInInspector] public QuaternionLimits limitMode; //currently in beta

            public Joints(Transform _joint, float _weight)
            {
                _weight = Mathf.Clamp(_weight, 0f, 1f);
                weight = _weight;

                transform = _joint;
            }
        }

        /// <summary>
        /// the definition of an IK chain
        /// </summary>
        [System.Serializable]
        public class Chain
        {
            public Transform target;

            private Vector3 IKHandle;
            private Quaternion IKRotation;

            [Range(0f, 1f)] public float weight = 1f;
            [Range(0f, 1f)] public float weightRotation = 0f;

            public int iterations;
            public List<RootIK.Joints> joints = new List<Joints>();

            private CCDSolver CCD = new CCDSolver();
            private FABRIKSolver FABRIK = new FABRIKSolver();
            private AnalyticalSolver Analytical = new AnalyticalSolver();


            #region Helping Methods
            /// <summary>
            /// Set the IK target Position which the chain will solve towards
            /// </summary>
            /// <param name="_target"></param>
            public void SetIKPosition(Vector3 _target)
            {
                IKHandle = target ? target.position : _target;
            }

            /// <summary>
            /// Find the current IK solver Position
            /// </summary>
            /// <returns>the current IK position</returns>
            public Vector3 GetIKPosition()
            {
                IKHandle = target ? target.position : IKHandle;
                return IKHandle;
            }

            /// <summary>
            /// Set the IK target rotation which will effect the rotation of the end effector
            /// </summary>
            public void SetIKRotation(Quaternion _rotation)
            {
                Quaternion _local = GetEndEffector() == null ? IKRotation : GetEndEffector().rotation;
                IKRotation = target ? Quaternion.Lerp(_local, target.rotation, weightRotation) : Quaternion.Lerp(_local, _rotation, weightRotation);
            }

            /// <summary>
            /// Get the current active IK rotation
            /// </summary>
            /// <returns>the rotation of the active rotation influencer</returns>
            public Quaternion GetIKRotation()
            {
                IKRotation = target ? target.rotation : IKRotation;
                Quaternion _local = Quaternion.Lerp(GetEndEffector().rotation, IKRotation, weightRotation);
                return _local;
            }

            /// <summary>
            /// Get the transform data of the end effector
            /// </summary>
            /// <returns>the endEffector transformation</returns>
            public Transform GetEndEffector()
            {
                return joints[joints.Count - 1].transform ? joints[joints.Count - 1].transform : null;
            }

            /// <summary>
            /// Set the IK position weight
            /// </summary>
            /// <param name="_weight"></param>
            public void SetIKPositionWeight(float _weight)
            {
                weight = _weight;
            }

            /// <summary>
            /// Set the IK Rotation weight
            /// </summary>
            /// <param name="_weight"></param>
            public void SetIKRotationWeight(float _weight)
            {
                weightRotation = _weight;
            }

            /// <summary>
            /// Solve the IK chain using heuristic iterative search methods (CCD and FABRIK);
            /// </summary>
            /// <param name="_solver"></param>
            public void SolveChain(InverseKinematics.SolverMethod _solver)
            {
                switch(_solver)
                {
                    case InverseKinematics.SolverMethod.CCD:
                        CCD.SolveCCD(this);
                        break;
                    case InverseKinematics.SolverMethod.FABRIK:
                        FABRIK.SolveFABRIK(this);
                        break;
                    case InverseKinematics.SolverMethod.Analytical:
                        Debug.Log("Use SolveChainAnalytically instead");
                        break;
                }
            }

            /// <summary>
            /// Solve the IK chain analytically (joint count must equal to 3)
            /// </summary>
            /// <param name="_direction"></param>
            /// <param name="_axis"></param>
            public void SolveChainAnalytically(Vector3 _direction, Vector3 _axis)
            {
                if (this.joints.Count != 3) return;
                Analytical.SolveAnalytically(this, _direction, _axis);
            }
            #endregion

        }

        /// <summary>
        /// The defenition of a Kinematic bone.
        /// Rotate the bone to look at the target;
        /// </summary>
        [System.Serializable]
        public class KinematicBone
        {
            [Range(0f, 1f)] public float weight;
            public Transform bone;
            public Transform target;
            public Vector3 axis;
        }

    }
}
