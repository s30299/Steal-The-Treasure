using UnityEngine;

public class HelperFunctions
{
    public static Vector3 Vector3toVector2(Vector3 vec3)
    {
        return new(vec3.x, vec3.y);
    }
}
