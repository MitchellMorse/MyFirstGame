using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class Vector3Extensions
    {
        public static Vector2 CalculateVectorTowards(this Vector3 targetVector, Vector3 collidingVector)
        {
            Vector2 direction = targetVector - collidingVector;
            direction = -direction.normalized;
            return direction;
        }

    }
}