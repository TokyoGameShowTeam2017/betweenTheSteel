using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// Example of doing interations using IK
    /// </summary>
    public class InteractionIK : MonoBehaviour
    {
        private InverseKinematics IK { get { return GetComponent<InverseKinematics>(); } }
        private bool targetFound = false;
        private Vector3 currentIKPos;

        public float yOffset = 1f;
        public float xOffset = 0f;
        [Range(0.1f, 3f)] public float rayLength = 2f;
        public float IKLerp = 7f;
        public LayerMask SolverLayer;


        void Start()
        {
            IK.enabled = false;
        }

        void LateUpdate()
        {
            FindWalls();
            Solve();
        }

        /// <summary>
        /// Find the walls through Raycasting
        /// </summary>
        void FindWalls()
        {
            /*
             * You can replace this with a spherecast if you wanted to.
             * or even an overlap sphere
             * */


            //raycast to the left
            Ray _ray = new Ray(transform.position + Vector3.up * yOffset + Vector3.right * xOffset, transform.forward);
            //RayArtist.DrawRay(_ray, rayLength, Color.red);
            RaycastHit _hit = new RaycastHit();
            if (Physics.Raycast(_ray, out _hit, rayLength, SolverLayer))
            {
                Vector3 _targetIK = _hit.point;

                IK.rightUpperbody.SetIKPosition(_targetIK);
                IK.rightUpperbody.SetIKRotation(RootIK.RotationLookAt(_hit.normal));
                targetFound = true;
                IK.rightUpperbody.SetIKPositionWeight(Mathf.Clamp(Mathf.Lerp(IK.rightUpperbody.weight, 1f, IKLerp * Time.fixedDeltaTime), 0f, 1f));
                return;
            }

            //raycast to the right
            Ray _ray2 = new Ray(transform.position + Vector3.up * yOffset, transform.right);
            //RayArtist.DrawRay(_ray2, rayLength, Color.green);
            RaycastHit _hit2 = new RaycastHit();
            if(Physics.Raycast(_ray2, out _hit2, rayLength, SolverLayer))
            {
                Vector3 _targetIK = _hit2.point;

                IK.rightUpperbody.SetIKPosition(_targetIK);
                IK.rightUpperbody.SetIKRotation(RootIK.RotationLookAt(_hit2.normal));
                targetFound = true;
                IK.rightUpperbody.SetIKPositionWeight(Mathf.Clamp(Mathf.Lerp(IK.rightUpperbody.weight, 1f, IKLerp * Time.fixedDeltaTime), 0f, 1f));
                return;
            }

            IK.rightUpperbody.SetIKPosition(IK.rightUpperbody.GetEndEffector().position);
            targetFound = false;
            IK.rightUpperbody.SetIKPositionWeight(Mathf.Clamp(Mathf.Lerp(IK.rightUpperbody.weight, 0f, IKLerp * 3f * Time.fixedDeltaTime), 0f, 1f));

        }

        /// <summary>
        /// EXECUTE !
        /// </summary>
        void Solve()
        {
            if (targetFound == false) return;
            IK.SolveIK();
        }
    }
}
