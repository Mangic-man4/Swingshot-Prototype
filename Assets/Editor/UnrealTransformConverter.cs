using UnityEditor;
using UnityEngine;

public class UnrealTransformConverter : EditorWindow
{
    [MenuItem("Tools/Unreal Transform Converter")]
    public static void ShowWindow()
    {
        GetWindow<UnrealTransformConverter>("Unreal Transform");
    }

    private void OnGUI()
    {
        Transform t = Selection.activeTransform;

        if (t == null)
        {
            EditorGUILayout.HelpBox(
                "Select an object in the Hierarchy or Scene.",
                MessageType.Info
            );
            return;
        }

        EditorGUILayout.LabelField(
            "Selected Object",
            Selection.activeGameObject.name,
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space();

        // -------------------------
        // POSITION
        // -------------------------

        Vector3 unrealPosition = UnityPositionToUnreal(t.position);

        EditorGUILayout.LabelField("Unreal World Position", EditorStyles.boldLabel);
        EditorGUILayout.Vector3Field("", RoundVector(unrealPosition, 3));

        if (GUILayout.Button("Copy Position"))
        {
            EditorGUIUtility.systemCopyBuffer =
                FormatVector(unrealPosition);
        }

        EditorGUILayout.Space();

        // -------------------------
        // ROTATION
        // -------------------------

        Vector3 unrealRotation = UnityRotationToUnreal(t);

        EditorGUILayout.LabelField("Unreal World Rotation", EditorStyles.boldLabel);

        EditorGUILayout.Vector3Field(
            "X Roll / Y Pitch / Z Yaw",
            RoundVector(unrealRotation, 3)
        );

        if (GUILayout.Button("Copy Rotation"))
        {
            EditorGUIUtility.systemCopyBuffer =
                FormatVector(unrealRotation);
        }

        EditorGUILayout.Space();

        // -------------------------
        // SCALE
        // -------------------------

        Vector3 unrealScale = UnityScaleToUnreal(t.lossyScale);

        EditorGUILayout.LabelField("Unreal World Scale", EditorStyles.boldLabel);
        EditorGUILayout.Vector3Field("", RoundVector(unrealScale, 3));

        if (GUILayout.Button("Copy Scale"))
        {
            EditorGUIUtility.systemCopyBuffer =
                FormatVector(unrealScale);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Copy All"))
        {
            EditorGUIUtility.systemCopyBuffer =
                $"Location: {FormatVector(unrealPosition)}\n" +
                $"Rotation: {FormatVector(unrealRotation)}\n" +
                $"Scale: {FormatVector(unrealScale)}";
        }
    }

    private static Vector3 UnityPositionToUnreal(Vector3 unity)
    {
        return new Vector3(
            unity.z * 100f, // Unreal X = Unity Z
            unity.x * 100f, // Unreal Y = Unity X
            unity.y * 100f  // Unreal Z = Unity Y
        );
    }

    private static Vector3 UnityScaleToUnreal(Vector3 unity)
    {
        return new Vector3(
            unity.z, // Unreal X = Unity Z
            unity.x, // Unreal Y = Unity X
            unity.y  // Unreal Z = Unity Y
        );
    }

    private static Vector3 UnityDirectionToUnreal(Vector3 unity)
    {
        return new Vector3(
            unity.z,
            unity.x,
            unity.y
        );
    }

    private static Vector3 UnityRotationToUnreal(Transform t)
    {
        // Convert the actual world-space orientation rather than
        // trying to rearrange Unity Euler angles directly.
        Vector3 forward = UnityDirectionToUnreal(t.forward).normalized;
        Vector3 right   = UnityDirectionToUnreal(t.right).normalized;
        Vector3 up      = UnityDirectionToUnreal(t.up).normalized;

        float pitch = Mathf.Asin(
            Mathf.Clamp(forward.z, -1f, 1f)
        ) * Mathf.Rad2Deg;

        float yaw = Mathf.Atan2(
            forward.y,
            forward.x
        ) * Mathf.Rad2Deg;

        float cosPitch = Mathf.Cos(pitch * Mathf.Deg2Rad);

        float roll;

        if (Mathf.Abs(cosPitch) > 0.0001f)
        {
            roll = Mathf.Atan2(
                -right.z,
                up.z
            ) * Mathf.Rad2Deg;
        }
        else
        {
            // Gimbal-lock fallback.
            roll = 0f;
        }

        // Unreal Transform panel displays:
        // X = Roll
        // Y = Pitch
        // Z = Yaw
        return new Vector3(
            NormalizeAngle(roll),
            NormalizeAngle(pitch),
            NormalizeAngle(yaw)
        );
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle > 180f)
            angle -= 360f;

        while (angle < -180f)
            angle += 360f;

        return angle;
    }

    private static Vector3 RoundVector(Vector3 value, int decimals)
    {
        return new Vector3(
            (float)System.Math.Round(value.x, decimals),
            (float)System.Math.Round(value.y, decimals),
            (float)System.Math.Round(value.z, decimals)
        );
    }

    private static string FormatVector(Vector3 value)
    {
        value = RoundVector(value, 3);

        return $"{value.x}, {value.y}, {value.z}";
    }

    private void OnSelectionChange()
    {
        Repaint();
    }
}