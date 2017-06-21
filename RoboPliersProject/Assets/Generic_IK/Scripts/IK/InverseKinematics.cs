using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// the out-of-the-box default IK component
    /// </summary>
    [DisallowMultipleComponent]
    public class InverseKinematics : IKChainBuilder
    {


        public enum SolverMethod { Analytical, CCD, FABRIK}

        [Header("Base Interface")]
        public bool spineAndShoulders;
        public SolverMethod solver;
        public bool debugUpdate = false;

        [Header("Chains")]
        public RootIK.Chain rightUpperbody = new RootIK.Chain();
        public RootIK.Chain leftUpperbody = new RootIK.Chain();
        public RootIK.Chain rightLeg = new RootIK.Chain();
        public RootIK.Chain leftLeg = new RootIK.Chain();
        public RootIK.KinematicBone head = new RootIK.KinematicBone();
        public RootIK.Chain[] genericChains;
        public RootIK.KinematicBone[] kinematicBones;

        //Solvers
        private FABRIKSolver FABRIK = new FABRIKSolver();
        private CCDSolver CCD = new CCDSolver();
        private AnalyticalSolver analyticalSolver = new AnalyticalSolver();
        private KinematicSolver kinematicSolver = new KinematicSolver();


        void OnEnable()
        {
            if (base.isHumanoid())
            {
                base.AssignLimbs();
            }
        }

        void LateUpdate()
        {
            //SolveIK();
        }

        /// <summary>
        /// Update the IK for all assigned chains
        /// </summary>
        public void SolveIK()
        {
            if (base.isHumanoid()) SolveHumanoid();
            else SolveGeneric();
        }

        /// <summary>
        /// Solve for Humanoid
        /// </summary>
        private void SolveHumanoid()
        {
            kinematicSolver.SolveBone(head);

            switch (solver)
            {
                case SolverMethod.CCD:
                    CCD.SolveCCD(rightUpperbody, leftUpperbody);
                    CCD.SolveCCD(rightLeg);
                    CCD.SolveCCD(leftLeg);
                    break;
                case SolverMethod.FABRIK:
                    FABRIK.SolveFABRIK(rightUpperbody);
                    FABRIK.SolveFABRIK(leftUpperbody);
                    FABRIK.SolveFABRIK(rightLeg);
                    FABRIK.SolveFABRIK(leftLeg);
                    break;
                case SolverMethod.Analytical:
                    analyticalSolver.SolveAnalytically(rightUpperbody, transform.forward, Vector3.down);
                    analyticalSolver.SolveAnalytically(leftUpperbody, transform.forward, Vector3.down);
                    analyticalSolver.SolveAnalytically(rightLeg, transform.forward, Vector3.up);
                    analyticalSolver.SolveAnalytically(leftLeg, transform.forward, Vector3.up);
                    break;
            }
        }

        /// <summary>
        /// Solve Generic Chain
        /// </summary>
        private void SolveGeneric()
        {
            for(int i = 0; i < kinematicBones.Length; i++)
            {
                kinematicSolver.SolveBone(kinematicBones[i]);
            }


            for(int i = 0; i < genericChains.Length; i++)
            {
                switch(solver)
                {
                    case SolverMethod.CCD:
                        CCD.SolveCCD(genericChains[i]);
                        break;
                    case SolverMethod.FABRIK:
                        FABRIK.SolveFABRIK(genericChains[i]);
                        break;
                    case SolverMethod.Analytical:
                        analyticalSolver.SolveAnalytically(genericChains[i], transform.forward, Vector3.up);    //the Vector3.Up part may be changed
                        break;
                }
            }
        }

        /// <summary>
        /// Check if the Humanoid chains are correctlly assigned
        /// </summary>
        /// <returns></returns>
        public bool ValidateChains()
        {
            if (base.isHumanoid() == false) return false;

            if (rightUpperbody == null || leftUpperbody == null || rightLeg == null || leftLeg == null) return false;

            if (rightUpperbody.joints.Count <= 0 || rightUpperbody.joints.Contains(null)) return false;
            if (leftUpperbody.joints.Count <= 0 || leftUpperbody.joints.Contains(null)) return false;
            if (rightLeg.joints.Count <= 0 || rightLeg.joints.Contains(null)) return false;
            if (leftLeg.joints.Count <= 0 || leftLeg.joints.Contains(null)) return false;

            return true;
        }
    }
}
