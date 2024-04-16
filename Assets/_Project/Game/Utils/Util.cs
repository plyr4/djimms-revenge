using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public static class Util
{
    public static string DefaultControlScheme()
    {
        return Constants.CONTROL_SCHEME_KEYBOARD_MOUSE;
    }

    public static bool IsControlSchemeKeyboardMouse(string controlScheme)
    {
        return controlScheme == Constants.CONTROL_SCHEME_KEYBOARD_MOUSE;
    }

    public static bool IsControlSchemeGamepad(string controlScheme)
    {
        return controlScheme == Constants.CONTROL_SCHEME_GAMEPAD;
    }

    public static string ToString(this Constants.ControlScheme controlScheme)
    {
        switch (controlScheme)
        {
            case Constants.ControlScheme.KEYBOARD_MOUSE:
                return Constants.CONTROL_SCHEME_KEYBOARD_MOUSE;

            case Constants.ControlScheme.GAMEPAD:
                return Constants.CONTROL_SCHEME_GAMEPAD;
            default:
                return Constants.CONTROL_SCHEME_GAMEPAD;
        }
    }

    public static Constants.ControlScheme ToControlScheme(this string controlScheme)
    {
        switch (controlScheme)
        {
            case Constants.CONTROL_SCHEME_KEYBOARD_MOUSE:
                return Constants.ControlScheme.KEYBOARD_MOUSE;
            case Constants.CONTROL_SCHEME_GAMEPAD:
                return Constants.ControlScheme.GAMEPAD;
            default:
                return Constants.ControlScheme.GAMEPAD;
        }
    }

    public static int RandomSign()
    {
        return UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    public static int SafeMod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    public static float RoundToIncrement(float f, float n)
    {
        if (n == 0) return f;
        return Mathf.Round(f / n) * n;
    }

    public static float Round(float f, float n)
    {
        return Mathf.Round(f * n) / n;
    }

    public static float RoundUp(float f, float n)
    {
        return Mathf.Ceil(f * n) / n;
    }

    public static float RoundUpEven(float f, float n)
    {
        return Mathf.Ceil(f * n) / n;
    }

    public static string TrimName(string name)
    {
        int index = name.IndexOf(" ");
        if (index >= 0) name = name.Substring(0, index);
        name = name.Replace('-', ' ').Replace('_', ' ');
        return name;
    }

    public static string TrimBeamableShortname(string name)
    {
        int index = name.IndexOf(" ");
        if (index >= 0) name = name.Substring(0, index);
        name = name.Replace('-', ' ').Replace('_', ' ');
        return name;
    }
    

    public static void SetLayerRecursively(this Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        for (int i = 0, count = parent.childCount; i < count; i++)
        {
            parent.GetChild(i).SetLayerRecursively(layer);
        }
    }

    public static string GetAspectRatio(int aScreenWidth, int aScreenHeight)
    {
        float r = (float)aScreenWidth / (float)aScreenHeight;
        string _r = r.ToString("F2");
        string ratio = _r.Substring(0, 4);
        switch (ratio)
        {
            case "2.37":
            case "2.39":
                return "21:9";
            case "1.25":
                return "5:4";
            case "1.33":
                return "4:3";
            case "1.50":
                return "3:2";
            case "1.60":
            case "1.56":
                return "16:10";
            case "1.67":
            case "1.78":
            case "1.77":
                return "16:9";
            case "0.67":
                return "2:3";
            case "0.56":
                return "9:16";
            default:
                return "16:9";
        }
    }

    public static Vector3 Random(Vector3 minInclusive, Vector3 maxInclusive)
    {
        return new Vector3(
            UnityEngine.Random.Range(minInclusive.x, maxInclusive.x),
            UnityEngine.Random.Range(minInclusive.y, maxInclusive.y),
            UnityEngine.Random.Range(minInclusive.z, maxInclusive.z)
        );
    }

    public static Vector3 RotateVectorAroundAxis(this Vector3 vector, Vector3 axis, float degrees)
    {
        return Quaternion.AngleAxis(degrees, axis) * vector;
    }

    public static Vector3 MoveToPosition(this Vector3 start, Vector3 destination, Vector3 movement)
    {
        float actualDistance = Vector3.Distance(start, destination);
        float incrementDistance = Vector3.Distance(start + movement, destination);
        if (incrementDistance < actualDistance)
        {
            start = start + movement;
        }

        return start;
    }

    public static Quaternion LookTowards(Transform target, Transform towards, float speed)
    {
        Vector3 direction = (towards.position - target.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        return Quaternion.Lerp(target.rotation, lookRotation,
            speed * Time.deltaTime);
    }

    public static bool ContainsLayer(LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    public static Bounds TransformBounds(this Transform _transform, Bounds _localBounds)
    {
        Vector3 center = _transform.TransformPoint(_localBounds.center);

        // transform the local extents' axes
        Vector3 extents = _localBounds.extents;
        Vector3 axisX = _transform.TransformVector(extents.x, 0, 0);
        Vector3 axisY = _transform.TransformVector(0, extents.y, 0);
        Vector3 axisZ = _transform.TransformVector(0, 0, extents.z);

        // sum their absolute value to get the world extents
        extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
        extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
        extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

        return new Bounds { center = center, extents = extents };
    }

    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static Vector3 Multiply(this Vector3 a, float x, float y, float z)
    {
        return new Vector3(a.x * x, a.y * y, a.z * z);
    }

    public static float ClampLerp(float a, float b, float t, float accuracy = Constants.FLOAT_PRECISION)
    {
        float val = Mathf.Lerp(a, b, t);

        if (Mathf.Abs(val - b) <= accuracy)
        {
            val = b;
        }

        return val;
    }

    public static Vector3 ClampLerp(Vector3 a, Vector3 b, float t, float accuracy = Constants.FLOAT_PRECISION)
    {
        Vector3 val = Vector3.Lerp(a, b, t);

        if (Mathf.Abs(val.x - b.x) <= accuracy)
        {
            val.x = b.x;
        }

        if (Mathf.Abs(val.y - b.y) <= accuracy)
        {
            val.y = b.y;
        }

        if (Mathf.Abs(val.z - b.z) <= accuracy)
        {
            val.z = b.z;
        }

        return val;
    }

    public static bool Arrived(this NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathPending) return false;
        if (!navMeshAgent.isOnNavMesh) return false;
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return false;
        if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0f) return false;
        return (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);
    }

    public static string ToHex(this Color color, bool includeAlpha = false) => ToHex((Color32)color, includeAlpha);

    public static string ToHex(this Color32 color, bool includeAlpha = false)
    {
        string hex = color.r.ToString("x2") + color.g.ToString("x2") + color.b.ToString("x2");
        if (includeAlpha)
            hex += color.a.ToString("x2");
        return hex;
    }
}