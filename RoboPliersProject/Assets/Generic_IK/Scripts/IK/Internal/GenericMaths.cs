using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// Helpful Maths helping methods
    /// </summary>
    public static class GenericMaths
    {
        /// <summary>
        /// Apply the rotation through Quaternion Multipication
        /// </summary>
        /// <param name="_qA"></param>
        /// <param name="_qB"></param>
        /// <returns>the final quaternion from _qB applied over _qA</returns>
        public static Quaternion ApplyQuaternion(Quaternion _qA, Quaternion _qB)
        {
            Quaternion qr = Quaternion.identity;
            Vector3 va = new Vector3(_qA.x, _qA.y, _qA.z);
            Vector3 vb = new Vector3(_qB.x, _qB.y, _qB.z);
            qr.w = _qA.w * _qB.w - Vector3.Dot(va, vb);
            Vector3 vr = Vector3.Cross(va, vb) + _qA.w * vb + _qB.w * va;
            qr.x = vr.x;
            qr.y = vr.y;
            qr.z = vr.z;
            return qr;
        }

        /// <summary>
        /// Creating a Quaternion from a Vector3
        /// </summary>
        /// <param name="_vector"></param>
        /// <returns></returns>
        public static Quaternion QuaternionFromVector(Vector3 _vector)
        {
            _vector.x = 2 * Mathf.PI * (_vector.x / 360f);
            _vector.y = 2 * Mathf.PI * (_vector.y / 360f);
            _vector.z = 2 * Mathf.PI * (_vector.z / 360f);

            Quaternion q = Quaternion.identity;

            q.w = Mathf.Sqrt(1 + (Mathf.Cos(_vector.y / 2) * Mathf.Cos(_vector.z / 2)) + (Mathf.Cos(_vector.y / 2) * Mathf.Cos(_vector.x / 2)) - (Mathf.Sin(_vector.y / 2) * Mathf.Sin(_vector.z / 2) * Mathf.Sin(_vector.x / 2)) + (Mathf.Cos(_vector.z / 2) * Mathf.Cos(_vector.x / 2))) / 2f;
            q.x = ((Mathf.Cos(_vector.z / 2) * Mathf.Sin(_vector.x / 2)) + (Mathf.Cos(_vector.y / 2) * Mathf.Sin(_vector.x / 2)) + (Mathf.Sin(_vector.y / 2) * Mathf.Sin(_vector.z / 2) * Mathf.Cos(_vector.x / 2))) / 4 * q.w;
            q.y = ((Mathf.Sin(_vector.y / 2) * Mathf.Cos(_vector.z / 2)) + (Mathf.Sin(_vector.y / 2) * Mathf.Cos(_vector.x / 2)) + (Mathf.Cos(_vector.y / 2) * Mathf.Sin(_vector.z / 2) * Mathf.Sin(_vector.x / 2))) / 4 * q.w;
            q.z = ((-Mathf.Sin(_vector.y / 2) * Mathf.Sin(_vector.x / 2)) + (Mathf.Cos(_vector.y / 2) * Mathf.Sin(_vector.z / 2) * Mathf.Cos(_vector.x / 2)) + Mathf.Sin(_vector.z / 2)) / 4 * q.w;

            return q;
        }

        /// <summary>
        /// Create a Quaternion from an axis and an angle
        /// </summary>
        /// <param name="_axis"></param>
        /// <param name="_angle"></param>
        /// <returns></returns>
        public static Quaternion QuaternionFromAngleAxis(Vector3 _axis, float _angle)
        {
            Quaternion q = Quaternion.identity;

            _axis.Normalize();
            _angle *= Mathf.Deg2Rad;

            q.x = _axis.x * Mathf.Sin(_angle / 2f);
            q.y = _axis.y * Mathf.Sin(_angle / 2f);
            q.z = _axis.z * Mathf.Sin(_angle / 2f);
            q.w = Mathf.Cos(_angle / 2f);

            return q;
        }

        /// <summary>
        /// Linearly Interpolate 2 Vectors
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <param name="_weight"></param>
        /// <returns></returns>
        public static Vector3 Interpolate(Vector3 _from, Vector3 _to, float _weight)
        {
            _weight = Mathf.Clamp(_weight, 0f, 1f);
            Vector3 _local = new Vector3((1 - _weight) * _from.x + _weight * _to.x, (1 - _weight) * _from.y + _weight * _to.y, (1 - _weight) * _from.z + _weight * _to.z);
            return _local;
        }

        /// <summary>
        /// A formula that finds an angle based on Trignometery laws
        /// (Note: order of _l input matters)
        /// </summary>
        /// <param name="_l1"></param>
        /// <param name="_l2"></param>
        /// <param name="_l3"></param>
        /// <returns>the angle in dgress</returns>
        public static float Formula(float _l1, float _l2, float _l3)
        {
            float _l1Sqr = Mathf.Pow(_l1, 2f);
            float _l2Sqr = Mathf.Pow(_l2, 2f);
            float _l3Sqr = Mathf.Pow(_l3, 2f);

            float formula = Mathf.Clamp((_l1Sqr + _l2Sqr - _l3Sqr) / (2f * _l1 * _l2), -1f, 1f);
            float local = Mathf.Acos(formula);

            return local * Mathf.Rad2Deg;
        }

        /// <summary>
        /// The angle between 2 vectors
        /// </summary>
        /// <param name="_v0"></param>
        /// <param name="_v1"></param>
        /// <returns>the angle between the vectors</returns>
        public static float VectorsAngle(Vector3 _v0, Vector3 _v1)
        {
            _v0.Normalize();
            _v1.Normalize();

            float _dot = Vector3.Dot(_v0, _v1);
            _dot = Mathf.Acos(Mathf.Clamp(_dot, -1f, 1f));

            return _dot * Mathf.Rad2Deg;
        }
    }
}
