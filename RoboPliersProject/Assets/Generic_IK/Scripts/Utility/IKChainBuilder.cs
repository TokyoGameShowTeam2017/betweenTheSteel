using UnityEngine;
using System.Collections.Generic;

namespace Generics.Dynamics
{
    /// <summary>
    /// Helping class for building Humanoid IK chains
    /// </summary>
    public class IKChainBuilder : MonoBehaviour
    {
        protected bool assigned { get; private set; }
        protected RootIK.Joints[] spine;
        public HumanoidReference HR { get; private set; }

        private Animator anim;


        /// <summary>
        /// Assign the body parts automatically
        /// </summary>
        public void AssignLimbs()
        {
            HR = new HumanoidReference();

            if (isHumanoid() == false)
            {
                Debug.LogWarning("The current rig is not marked as 'Humanoid', Can not Assign Limbs automatically");
                Debug.LogWarning("Animator component could not be found, the system will consider the model as Generic");
                return;
            }

            HR.root = anim.GetBoneTransform(HumanBodyBones.Hips);
            HR.head = anim.GetBoneTransform(HumanBodyBones.Head);
            HR.spine.spine = anim.GetBoneTransform(HumanBodyBones.Spine);
            HR.r_upperbody.r_shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
            HR.l_upperbody.l_shoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
            HR.r_lowerbody.r_UpperLeg = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            HR.l_lowerbody.l_UpperLeg = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);

            spine = new RootIK.Joints[3];
            spine[0] = new RootIK.Joints(HR.spine.spine, 0.2f);
            spine[1] = new RootIK.Joints(HR.spine.spine1, 0.4f);
            spine[2] = new RootIK.Joints(HR.spine.spine2, 0.6f);

            assigned = true;

        }

        /// <summary>
        /// the Head Transform of the character
        /// </summary>
        /// <returns></returns>
        protected Transform Head()
        {
            if(!assigned || HR == null)
            {
                AssignLimbs();
                return null;
            }

            if (!HR.head) return null;

            return HR.head;
        }

        /// <summary>
        /// The root Transform of the character
        /// </summary>
        /// <returns></returns>
        protected Transform Root()
        {
            if (!assigned)
            {
                AssignLimbs();
                return null;
            }

            if (!HR.root) return null;

            return HR.root;
        }

        /// <summary>
        /// Build the IK chain for the right arm
        /// </summary>
        /// <param name="_joints"></param>
        /// <param name="_includeSpineShoulders">whether to include the shoulder and the spine in the chain or not</param>
        public void BuildRightArm(out List<RootIK.Joints> _joints, bool _includeSpineShoulders)
        {
            _joints = new List<RootIK.Joints>();

            AssignLimbs();

            if (!assigned)
            {
                AssignLimbs();
                return;
            }


            if (_includeSpineShoulders)
            {
                if (HR.spine.spine == null) return;

                _joints.InsertRange(0, spine);
                _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_upperbody.r_shoulder, 0.80f));
            }

            _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_upperbody.r_upperArm, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_upperbody.r_lowerArm, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_upperbody.r_hand, 1f));
        }

        /// <summary>
        /// Build the IK chain for the right arm
        /// </summary>
        /// <param name="_joints"></param>
        /// <param name="_includeSpineShoulders">whether to include the shoulder and the spine in the chain or not</param>
        public void BuildLeftArm(out List<RootIK.Joints> _joints, bool _includeSpineShoulders)
        {
            _joints = new List<RootIK.Joints>();

            AssignLimbs();

            if (!assigned)
            {
                AssignLimbs();
                return;
            }

            if (_includeSpineShoulders)
            {
                if (HR.spine.spine == null) return;

                _joints.InsertRange(0, spine);
                _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_upperbody.l_shoulder, 0.80f));
            }

            _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_upperbody.l_upperArm, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_upperbody.l_lowerArm, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_upperbody.l_hand, 1f));
        }

        /// <summary>
        /// Build the IK chain for the right leg
        /// </summary>
        public void BuildRightLeg(out List<RootIK.Joints> _joints)
        {
            _joints = new List<RootIK.Joints>();

            AssignLimbs();

            if (!assigned)
            {
                AssignLimbs();
                return;
            }

            _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_lowerbody.r_UpperLeg, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_lowerbody.r_LowerLeg, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.r_lowerbody.r_Foot, 1f));
        }

        /// <summary>
        /// Build the IK chain for the left leg
        /// </summary>
        public void BuildLeftLeg(out List<RootIK.Joints> _joints)
        {
            _joints = new List<RootIK.Joints>();

            AssignLimbs();

            if (!assigned)
            {
                AssignLimbs();
                return;
            }

            _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_lowerbody.l_UpperLeg, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_lowerbody.l_LowerLeg, 1f));
            _joints.Insert(_joints.Count, new RootIK.Joints(HR.l_lowerbody.l_Foot, 1f));
        }

        /// <summary>
        /// Check whether the model is Humanoid or Generic
        /// </summary>
        /// <returns></returns>
        public bool isHumanoid()
        {
            if (anim == null) anim = GetComponent<Animator>();
            if (anim == null) anim = GetComponentInChildren<Animator>();
            if (anim == null) anim = GetComponentInParent<Animator>();
            if (anim == null) return false;

            return anim.isHuman;
        }

    }
}
