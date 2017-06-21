using UnityEngine;

/// <summary>
/// A set of methods to visualies raycasts
/// </summary>
public class RayArtist
{

    #region DrawRay variations
    public static void DrawRay(Vector3 _origin, Vector3 _direction, float _length, Color _color)
    {
        Vector3 _newDir = new Vector3(_direction.x == 0f ? _origin.x : _origin.x - _length, _direction.y == 0f ? _origin.y : _origin.y - _length, _direction.z == 0f ? _origin.z : _origin.z - _length);
        Debug.DrawLine(_origin, _newDir, _color);
    }

    public static void DrawRay(Ray _ray, float _length, Color _color)
    {
        Vector3 _newDir = new Vector3(_ray.direction.x == 0f ? _ray.origin.x : _ray.origin.x - _length, _ray.direction.y == 0f ? _ray.origin.y : _ray.origin.y - _length, _ray.direction.z == 0f ? _ray.origin.z : _ray.origin.z - _length);
        Debug.DrawLine(_ray.origin, _newDir, _color);
    }
    #endregion

    public static void DrawHitPoints(Ray _ray, Vector3 _hitPoint, Color _color)
    {
        Debug.DrawLine(_ray.origin, _hitPoint, _color);
    }

    public static void DrawHitPoints(Vector3 _origin, Vector3 _hitPoint, Color _color)
    {
        Debug.DrawLine(_origin, _hitPoint, _color);
    }

}
