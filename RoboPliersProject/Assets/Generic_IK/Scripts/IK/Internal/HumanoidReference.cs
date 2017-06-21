using UnityEngine;

namespace Generics.Dynamics
{
    /// <summary>
    /// this class contains all the required limbs for Humanoid IK solving
    /// </summary>
    public class HumanoidReference
    {

        /// <summary>
        /// the right side of the lowerbody limbs
        /// </summary>
        public struct LowerbodyRight
        {
            public Transform r_UpperLeg;
            public Transform r_LowerLeg { get { if (r_UpperLeg && r_UpperLeg.childCount == 1) return r_UpperLeg.GetChild(0); return null; } }
            public Transform r_Foot { get { if (r_LowerLeg && r_LowerLeg.childCount == 1) return r_LowerLeg.GetChild(0); return null; } }
        }
        public LowerbodyRight r_lowerbody;

        /// <summary>
        /// the left side of the lowerbody limbs
        /// </summary>
        public struct LowerbodyLeft
        {
            public Transform l_UpperLeg;
            public Transform l_LowerLeg { get { if (l_UpperLeg && l_UpperLeg.childCount == 1) return l_UpperLeg.GetChild(0); return null; } }
            public Transform l_Foot { get { if (l_LowerLeg && l_LowerLeg.childCount == 1) return l_LowerLeg.GetChild(0); return null; } }
        }
        public LowerbodyLeft l_lowerbody;

        /// <summary>
        /// the right side of the upperbody limbs
        /// </summary>
        public struct UpperbodyRight
        {
            public Transform r_shoulder;
            public Transform r_upperArm { get { if (r_shoulder && r_shoulder.childCount == 1) return r_shoulder.GetChild(0); return null; } }
            public Transform r_lowerArm { get { if (r_upperArm && r_upperArm.childCount == 1) return r_upperArm.GetChild(0); return null; } }
            public Transform r_hand { get { if (r_lowerArm && r_lowerArm.childCount == 1) return r_lowerArm.GetChild(0); return null; } }
        }
        public UpperbodyRight r_upperbody;

        /// <summary>
        /// the left side of the upperbody limbs
        /// </summary>
        public struct UpperbodyLeft
        {
            public Transform l_shoulder;
            public Transform l_upperArm { get { if (l_shoulder && l_shoulder.childCount == 1) return l_shoulder.GetChild(0); return null;} }
            public Transform l_lowerArm { get { if (l_upperArm && l_upperArm.childCount == 1) return l_upperArm.GetChild(0); return null; } }
            public Transform l_hand { get { if (l_lowerArm && l_lowerArm.childCount == 1) return l_lowerArm.GetChild(0); return null; } }
        }
        public UpperbodyLeft l_upperbody;

        /// <summary>
        /// the spine of the character, a typical humanoid has 3 spine components
        /// </summary>
        public struct Spine
        {
            public Transform spine;
            public Transform spine1 { get { if (spine && spine.childCount == 1) return spine.GetChild(0); return null; } }
            public Transform spine2 { get { if (spine1 && spine1.childCount == 1) return spine1.GetChild(0); return null; } }
        }
        public Spine spine;

        public Transform root;
        public Transform head;
    }
}
