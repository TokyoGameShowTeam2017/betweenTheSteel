using UnityEngine;
using UnityEditor;

namespace Generics.Dynamics
{
    /// <summary>
    /// Custom editor for the default IK component
    /// </summary>
    [CustomEditor(typeof(InverseKinematics))]
    public class IKEditor : Editor
    {
        private InverseKinematics IK { get { return target as InverseKinematics; } }

        private SerializedProperty rA, lA;        //right/left arm
        private SerializedProperty rL, lL;        //right/left leg

        //internal
        private bool drawSolver = false;


        //Design
        private const string Hrig = "Humanoid rig";
        private const string Grig = "Generic rig";

        private const string joints = "joints";
        private const string weight = "weight";

        private const string rATooltip = "the right arm IK chain";
        private const string lATooltip = "the left arm IK chain";
        private const string rLTooltip = "the right leg IK chain";
        private const string lLTooltip = "the left leg IK chain";

        private const string SSboolTooltip = "Include spine and shoulders bones to the upperbody chains";
        private const string solverTooltip = "each solving method is unique to something but all achieve the same goal";



        /// <summary>
        /// Override the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("IK solution for " + (IK.isHumanoid() ? Hrig : Grig), GUIStyle.none);

            if (IK.isHumanoid() == false) DrawGenericInspector();
            else DrawHumanoidInspector();

            //base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(IK);
        }

        /// <summary>
        /// draw the inspector to best suit generic characters
        /// </summary>
        private void DrawGenericInspector()
        {
            EditorGUILayout.Space();
            SerializedProperty _solver = serializedObject.FindProperty("solver");
            EditorGUILayout.PropertyField(_solver, new GUIContent("Solver", solverTooltip));
            EditorGUILayout.Space();
            SerializedProperty _kinematicBone = serializedObject.FindProperty("kinematicBones");
            EditorGUILayout.PropertyField(_kinematicBone, new GUIContent("Kinematic Bones"), true);
            if(_kinematicBone.isExpanded) EditorGUILayout.Separator();
            SerializedProperty _genericChains = serializedObject.FindProperty("genericChains");
            EditorGUILayout.PropertyField(_genericChains, new GUIContent("Generic Chains"), true);
        }

        /// <summary>
        /// draw the inspector and automatically build the IK chains.
        /// </summary>
        private void DrawHumanoidInspector()
        {
            EditorGUILayout.Separator();

            rA = serializedObject.FindProperty("rightUpperbody");
            rA = serializedObject.FindProperty("rightUpperbody");
            lA = serializedObject.FindProperty("leftUpperbody");
            rL = serializedObject.FindProperty("rightLeg");
            lL = serializedObject.FindProperty("leftLeg");

            SerializedProperty _head = serializedObject.FindProperty("head");
            SerializedProperty _spineShoulder = serializedObject.FindProperty("spineAndShoulders");
            SerializedProperty _solver = serializedObject.FindProperty("solver");
            drawSolver = EditorGUILayout.Toggle(new GUIContent("Draw Solver", "Draw solver's helper GUI in the scene view"), drawSolver);
            EditorGUILayout.PropertyField(_spineShoulder, new GUIContent("Spine & Shoulders", SSboolTooltip));
            EditorGUILayout.PropertyField(_solver, new GUIContent("Solver", solverTooltip));

            EditorGUILayout.PropertyField(rA, new GUIContent("Right Arm", rATooltip), true);
            EditorGUILayout.PropertyField(lA, new GUIContent("Left Arm", lATooltip), true);
            EditorGUILayout.PropertyField(rL, new GUIContent("Right Leg", rLTooltip), true);
            EditorGUILayout.PropertyField(lL, new GUIContent("Left Leg", lLTooltip), true);
            EditorGUILayout.PropertyField(_head, new GUIContent("Head", "use the head as a Kinematic Bone"), true);

            EditorGUILayout.Separator();

            if(GUILayout.Button("Initialise IK"))
            {
                IK.BuildRightArm(out IK.rightUpperbody.joints, IK.spineAndShoulders);
                IK.BuildLeftArm(out IK.leftUpperbody.joints, IK.spineAndShoulders);
                IK.BuildRightLeg(out IK.rightLeg.joints);
                IK.BuildLeftLeg(out IK.leftLeg.joints);
                IK.head.bone = IK.HR.head;
            }

        }

        /// <summary>
        /// Draw the cool lines in the editor
        /// </summary>
        public void OnSceneGUI()
        {
            if (drawSolver == false) return;
            if (IK.ValidateChains() == false) return;
            if (IK.HR == null) IK.AssignLimbs();

            Handles.color = new Color(0.6f, 1f, 0.5f, 1f); //we need a vibrant screaming color
                                                           //right upperbody
            for (int i = 0; i < IK.rightUpperbody.joints.Count - 1; i++)
            {
                Handles.SphereCap(0, IK.rightUpperbody.joints[i].transform.position, Quaternion.identity, 0.03f);
                Handles.DrawLine(IK.rightUpperbody.joints[i].transform.position, IK.rightUpperbody.joints[i + 1].transform.position);
            }

            //left upperbody
            for (int i = 0; i < IK.leftUpperbody.joints.Count - 1; i++)
            {
                Handles.SphereCap(0, IK.leftUpperbody.joints[i].transform.position, Quaternion.identity, 0.03f);
                Handles.DrawLine(IK.leftUpperbody.joints[i].transform.position, IK.leftUpperbody.joints[i + 1].transform.position);
            }

            //right lowerbody
            for (int i = 0; i < IK.rightLeg.joints.Count - 1; i++)
            {
                Handles.SphereCap(0, IK.rightLeg.joints[i].transform.position, Quaternion.identity, 0.03f);
                Handles.DrawLine(IK.rightLeg.joints[i].transform.position, IK.rightLeg.joints[i + 1].transform.position);
            }

            //left lowerbody
            for (int i = 0; i < IK.leftLeg.joints.Count - 1; i++)
            {
                Handles.SphereCap(0, IK.leftLeg.joints[i].transform.position, Quaternion.identity, 0.03f);
                Handles.DrawLine(IK.leftLeg.joints[i].transform.position, IK.leftLeg.joints[i + 1].transform.position);
            }

            //spine
            if(IK.HR.head && IK.rightUpperbody.joints.Count >= 5 && !(IK.rightUpperbody.joints.Count <= 3))
            {
                Handles.DrawLine(IK.HR.spine.spine2.position, IK.HR.head.position);
            }

            //draw end effectors
            if (IK.HR.head)
            {
                Handles.color = Color.Lerp(new Color(0.5f, 0.8f, 0.8f, 0.8f), new Color(1f, 0.1f, 0.1f), IK.head.weight);
                Handles.CubeCap(0, IK.HR.head.position, Quaternion.identity, 0.02f);
            }

            Handles.color = Color.Lerp(new Color(0.5f, 0.8f, 0.8f, 0.8f), new Color(1f, 0.1f, 0.1f), IK.rightUpperbody.weight);
            Handles.CubeCap(0, IK.rightUpperbody.GetEndEffector().position, Quaternion.identity, 0.025f);

            Handles.color = Color.Lerp(new Color(0.5f, 0.8f, 0.8f, 0.8f), new Color(1f, 0.1f, 0.1f), IK.leftUpperbody.weight);
            Handles.CubeCap(0, IK.leftUpperbody.GetEndEffector().position, Quaternion.identity, 0.025f);

            Handles.color = Color.Lerp(new Color(0.5f, 0.8f, 0.8f, 0.8f), new Color(1f, 0.1f, 0.1f), IK.rightLeg.weight);
            Handles.CubeCap(0, IK.rightLeg.GetEndEffector().position, Quaternion.identity, 0.025f);

            Handles.color = Color.Lerp(new Color(0.5f, 0.8f, 0.8f, 0.8f), new Color(1f, 0.1f, 0.1f), IK.leftLeg.weight);
            Handles.CubeCap(0, IK.leftLeg.GetEndEffector().position, Quaternion.identity, 0.025f);

            //draw targets with different colors
            Handles.color = Color.red;

            if (IK.rightUpperbody.weight > 0f) Handles.DrawDottedLine(IK.rightUpperbody.GetEndEffector().position, IK.rightUpperbody.GetIKPosition(), 2f);
            if (IK.leftUpperbody.weight > 0f) Handles.DrawDottedLine(IK.leftUpperbody.GetEndEffector().position, IK.leftUpperbody.GetIKPosition(), 2f);
            if (IK.rightLeg.weight > 0f) Handles.DrawDottedLine(IK.rightLeg.GetEndEffector().position, IK.rightLeg.GetIKPosition(), 2f);
            if (IK.leftLeg.weight > 0f) Handles.DrawDottedLine(IK.leftLeg.GetEndEffector().position, IK.leftLeg.GetIKPosition(), 2f);
            if (IK.head.target && IK.HR.head && IK.head.weight > 0f) Handles.DrawDottedLine(IK.HR.head.position, IK.head.target.position, 2f);

        }

    }
}
