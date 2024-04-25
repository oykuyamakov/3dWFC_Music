using UnityEngine;

namespace UnityCommon.Runtime.Extensions
{
	public static class VectorExtensions
	{
		
		public static Vector2 WithX(this Vector2 v, float x)
		{
			v.x = x;
			return v;
		}

		public static Vector2 WithY(this Vector2 v, float y)
		{
			v.y = y;
			return v;
		}


		public static Vector3 WithX(this Vector3 v, float x)
		{
			v.x = x;
			return v;
		}

		public static Vector3 WithY(this Vector3 v, float y)
		{
			v.y = y;
			return v;
		}

		public static Vector3 WithZ(this Vector3 v, float z)
		{
			v.z = z;
			return v;
		}

		public static Vector3 WithXY(this Vector3 v, float x, float y)
		{
			v.x = x;
			v.y = y;
			return v;
		}

		public static Vector3 WithXZ(this Vector3 v, float x, float z)
		{
			v.x = x;
			v.z = z;
			return v;
		}

		public static Vector3 WithYZ(this Vector3 v, float y, float z)
		{
			v.y = y;
			v.z = z;
			return v;
		}

		public static Vector3 WithXRelative(this Vector3 v, float x)
		{
			v.x += x;
			return v;
		}

		public static Vector3 WithYRelative(this Vector3 v, float y)
		{
			v.y += y;
			return v;
		}

		public static Vector3 WithZRelative(this Vector3 v, float z)
		{
			v.z += z;
			return v;
		}

		public static Vector3 WithXYRelative(this Vector3 v, float x, float y)
		{
			v.x += x;
			v.y += y;
			return v;
		}

		public static Vector3 WithYZRelative(this Vector3 v, float y, float z)
		{
			v.y += y;
			v.z += z;
			return v;
		}

		public static Vector3 WithXZRelative(this Vector3 v, float x, float z)
		{
			v.x += x;
			v.z += z;
			return v;
		}


		public static Vector3 WithLength(this Vector3 v, float len)
		{
			return v.normalized * len;
		}

		
		
	}
}
