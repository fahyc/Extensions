using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

public interface Behavior<V>
{
	float Score(V owner);
	void Init(V owner);
	void Run(V owner);
	//string name { get; set; }
}

[System.Serializable]
public class Slide
{
	//static float minMaximum = .001f; 
	public float val;
	public float max;
	public float drift;//the amount this changes every second naturally.
	public Slide(float v, float m)
	{
		val = Mathf.Clamp(v, 0, m);
		max = m;// < minMaximum ? minMaximum : m;
	}
	public float ratio()
	{
		return val / max;
	}

	public Slide norm()
	{
		return new Slide(ratio(), 1);
	}
	public void add(float amount)
	{
		val = Mathf.Clamp(val + amount, 0, max);
	}
	public void jaggedAdd(float amount)
	{
		if (UnityEngine.Random.value < Time.deltaTime)
		{
			add(amount);
		}
	}
	public override string ToString()
	{
		return "[" + val + "/" + max + "]";
	}
	public Slide rootedAdded(Slide other)
	{//Zero if either is zero. Higher than the average if both are high. A reasonable standard add function.
		return (this * other) ^ (this * other);
	}

	public Slide sqrt()
	{//increases the ratio of a Slide on a nice smooth curve.
		return new Slide(Mathf.Sqrt(val), Mathf.Sqrt(max));
	}

	public static Slide operator ^(Slide s1, Slide s2)
	{//psuedo add. is one if either are one. Always higher than the average.
		return -(-s1 * -s2);
	}
	public static Slide operator -(Slide s1)
	{//negation
		return new Slide(s1.max - s1.val,s1.max);
	}
	public static Slide operator +(Slide s1, Slide s2)
	{//average
		return new Slide(s1.val + s2.val, s1.max + s2.max);
	}
	public static Slide operator -(Slide s1, Slide s2)
	{//average with negation
		return s1 + (-s2);
	}
	public static Slide operator *(Slide s1, Slide s2)
	{//this function doesn't make an enormous amount of sense now that I think about it.
		return new Slide(s1.val * s2.val, s1.max * s2.max);
	}
	public static Slide operator *(Slide s1, float f)
	{//weighting function
		return new Slide(s1.val * f, s1.max * f);
	}
	public static Slide operator *(float f, Slide s1)
	{
		return new Slide(f * s1.val, f * s1.max);
	}

	public static Slide operator |(Slide s1, Slide s2)
	{//max function
		float r1 = s1.ratio();
		float r2 = s2.ratio();
		return new Slide(Mathf.Max(r1, r2),1);
	}

	public static Slide operator &(Slide s1, Slide s2)
	{//min function
		float r1 = s1.ratio();
		float r2 = s2.ratio();
		return new Slide(Mathf.Min(r1, r2), 1);
	}

	public static Slide Zero()
	{
		return new Slide(0, 1);
	}
	public static Slide One()
	{
		return new Slide(1, 1);
	}
	public static Slide Exists(Object o)
	{//returns the slide equivelant
		if(o != null)
		{
			return One();
		}
		return Zero();
	}
}

[System.Serializable]
public class UtilityAI<T> {

	public bool debugMode = true;

	public T owner;

	public int curIndex;

	public List<Behavior<T>> behaviors;

	public UtilityAI(Behavior<T>[] behave, T o){
		behaviors = new List<Behavior<T>>(behave);
		owner = o;
	}

	public void Calculate()
	{
		float max = 0;
		int maxIndex = 0;
		if (debugMode)
		{
			Debug.Log("STARTING DECISION WITH " + behaviors.Count + " BEHAVIORS");

		}
		for (int i = 0; i < behaviors.Count; i++)
		{
			float tscore = behaviors[i].Score(owner);
			if (debugMode)
			{
				Debug.Log(behaviors[i].GetType() + ": " + tscore);
			}
			if (tscore > max)
			{
				max = tscore;
				maxIndex = i;
			}
		}
		if(debugMode)
			Debug.Log("Chosen: " + behaviors[maxIndex].GetType());
		if (curIndex != maxIndex)
		{
			behaviors[maxIndex].Init(owner);
			curIndex = maxIndex;
		}
	}

	public void Add(Behavior<T> b)
	{
		if (debugMode)
			Debug.Log("Adding " + b.GetType() + ". behaviors starts at " + behaviors.Count);
		behaviors.Add(b);
		if (debugMode)
			Debug.Log("Adding " + b.GetType() + ". behaviors is now at " + behaviors.Count);
	}

	public void Run()
	{
		behaviors[curIndex].Run(owner);
	}
}
