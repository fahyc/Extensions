using UnityEngine;
using System.Collections;

namespace ExtensionMethods
{
	public static class MyExtensions
	{
		public static Vector2 xy(this Vector3 vec)
		{
			return new Vector2(vec.x, vec.y);
		}
		public static Vector3 append(this Vector2 vec, float num)
		{
			return new Vector3(vec.x, vec.y, num);
		}
		public static T[] slowAdded<T>(this T[] array, T obj)
		{
			//returns a copy of the array with obj at the end.
			T[] output = new T[array.Length + 1];
			for (int i = 0; i < array.Length; i++) {
				output[i] = array[i];
			}
			output[output.Length - 1] = obj;
			return output;
		}
		public static T[] cleaned<T>(this T[] array)
		{
			//returns a copy of the array with any null values removed. 
			int count = 0;
			for(int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					count++;
				}
			}
			T[] output = new T[array.Length - count];
			count = 0;
			for(int i = 0; i < array.Length; i++)
			{
				if(array[i] == null)
				{
					count++;
				}
				else
				{
					output[i - count] = array[i];
				}
			}
			return output;
		}
		public static void print<T>(this T[] array)
		{
			string output = "[";
			for(int i = 0; i < array.Length; i++)
			{
				output += array[i].ToString() + ",";
			}
			output += "]";
			Debug.Log(output);
		}
		
	}
	public static class H
	{
		public static float UnityAtan(float x, float y)
		{//converts an xy vector into an angle
			return Mathf.Rad2Deg * Mathf.Atan2(y, x) - 90;
		}
		public static float UnityAtan(Vector2 vec)
		{//converts a vector into an angle. 
			return UnityAtan(vec.x, vec.y);
		}
		public static float aDif(float angle1, float angle2)
		{//angular difference. This version taken from stackoverflow to ensure correctness. 
			angle1 = angle1 * Mathf.Deg2Rad;
			angle2 = angle2 * Mathf.Deg2Rad;
			return Mathf.Atan2(Mathf.Sin(angle1 - angle2), Mathf.Cos(angle1 - angle2)) * Mathf.Rad2Deg;
		}
	}
}