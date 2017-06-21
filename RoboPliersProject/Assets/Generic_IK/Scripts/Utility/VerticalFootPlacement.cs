using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// Correct the vertical feet placement
    /// </summary>
    public class VerticalFootPlacement : IKChainBuilder
    {
        /*
         * Note that this class in not perfect yet, and only meant to show off the AnaylticalSolver.cs
         * though it could be furthur improved in upcoming updates to the package.
         * but you're free to play with it as much as you want
         * */

        
        public RootIK.Chain rightLeg = new RootIK.Chain();
        public RootIK.Chain leftLeg = new RootIK.Chain();

        [Header("Interface")]
        [Range(0f, 3f)]
        public float rayLength = 2f;
        public float maxStep = 0.4f;
        public float footHeight = 0.1f;
        public float rootLerp = 10f;
        public float footLerp = 40f;
        public LayerMask solverLayer;


        //internals
        private bool grounded;
        private float rootY;
        private Vector3[] offsetedIK = new Vector3[2];
        private Vector3[] IKTarget = new Vector3[2];


        void Start()
        {
            base.BuildRightLeg(out rightLeg.joints);
            base.BuildLeftLeg(out leftLeg.joints);

            //set everything intially to default to avoid any errors that might come
            rightLeg.SetIKRotation(rightLeg.GetEndEffector().rotation);
            leftLeg.SetIKRotation(leftLeg.GetEndEffector().rotation);
        }

        void LateUpdate()
        {
            grounded = Grounded();

            FindGround();
            OffsetTargets();
            Solve();
        }

        private void FindGround()
        {

            for(int i = 0; i < 2; i++)
            {
                Vector3 _endEffector = i == 0 ? rightLeg.GetEndEffector().position : leftLeg.GetEndEffector().position;
                Ray _ray = new Ray(_endEffector + Vector3.up * maxStep, Vector3.down);
                RaycastHit _hit = new RaycastHit();

                if(Physics.Raycast(_ray, out _hit, rayLength, solverLayer))
                {
                    Quaternion _ankleRot = RootIK.RotateFromTo(_hit.normal, RootIK.TransformVector(Vector3.up, Root().rotation));

                    rightLeg.SetIKRotation(i == 0 ? GenericMaths.ApplyQuaternion(_ankleRot, rightLeg.GetEndEffector().rotation) : rightLeg.GetIKRotation());
                    leftLeg.SetIKRotation(i == 1 ? GenericMaths.ApplyQuaternion(_ankleRot, leftLeg.GetEndEffector().rotation) : leftLeg.GetIKRotation());

                    IKTarget[i] = _hit.point;
                }

            }

            rightLeg.SetIKPosition(IKTarget[0]);
            leftLeg.SetIKPosition(IKTarget[1]);

        }

        /// <summary>
        /// we need to offset the root to achieve the results
        /// </summary>
        private void OffsetTargets()
        {
            Vector3 _newRootPos = Root().position;
            float _toRight = Vector3.Distance(_newRootPos, rightLeg.GetIKPosition());
            float _toLeft = Vector3.Distance(_newRootPos, leftLeg.GetIKPosition());
            float _rootOffset = Mathf.Max(_toLeft, _toRight) - Mathf.Min(_toRight, _toLeft);

            rootY = Mathf.Lerp(rootY, _rootOffset, rootLerp * Time.fixedDeltaTime);
            _newRootPos.y -= rootY;


            for(int i = 0; i < 2; i++)
            {
                Vector3 _targetOffset = i == 0 ? rightLeg.GetIKPosition() : leftLeg.GetIKPosition();
                _targetOffset += Vector3.up * footHeight;
                offsetedIK[i] = GenericMaths.Interpolate(offsetedIK[i], _targetOffset, footLerp * Time.fixedDeltaTime);
            }

            rightLeg.SetIKPosition(offsetedIK[0]);
            leftLeg.SetIKPosition(offsetedIK[1]);
            Root().position = _newRootPos;

            //RayArtist.DrawHitPoints(rightLeg.GetEndEffector().position + Vector3.up * maxStep + Vector3.right * 0.3f, rightLeg.GetIKPosition() + Vector3.right * 0.3f, Color.red);
            //RayArtist.DrawHitPoints(leftLeg.GetEndEffector().position + Vector3.up * maxStep + Vector3.left * 0.2f, leftLeg.GetIKPosition() + Vector3.left * 0.2f, Color.red);
        }


        /// <summary>
        /// ROCK AND ROLL !
        /// </summary>
        private void Solve()
        {
            if (!grounded) return;
            rightLeg.SolveChainAnalytically(transform.forward, Vector3.up);
            leftLeg.SolveChainAnalytically(transform.forward, Vector3.up);
        }

        /// <summary>
        /// Simple raycast to find the ground
        /// </summary>
        /// <returns></returns>
        private bool Grounded()
        {
            Ray _ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            RaycastHit _hit = new RaycastHit();
            //RayArtist.DrawRay(_ray, rayLength, Color.green);
            if(Physics.Raycast(_ray, out _hit, 1f, solverLayer))
            {
                return true;
            }

            return false;
        }
    }
}
