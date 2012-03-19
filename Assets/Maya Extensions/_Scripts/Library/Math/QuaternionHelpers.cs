using UnityEngine;
using System.Collections;

/// <file>
/// 
/// <author>
/// Adam Mechtley
/// http://adammechtley.com/donations
/// </author>
/// 
/// <copyright>
/// Copyright (c) 2011,  Adam Mechtley.
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without
/// modification, are permitted provided that the following conditions are met:
/// 
/// 1. Redistributions of source code must retain the above copyright notice,
/// this list of conditions and the following disclaimer.
/// 
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
/// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
/// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
/// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
/// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
/// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
/// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
/// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
/// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
/// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
/// POSSIBILITY OF SUCH DAMAGE.
/// </copyright>
/// 
/// <summary>
/// This file contains a class with static methods for working with Quaternions.
/// 
/// NoFlip and Cache interpolation modes are not implemented.
/// </summary>
/// 
/// </file>

/// <summary>
/// an enum for Euler rotation order (corresponds to that in Maya's MEulerAngle class, where composition is right to left and rotation is right-handed)
/// </summary>
public enum EulerRotationOrder { XYZ, YZX, ZXY, XZY, YXZ, ZYX };

/// <summary>
/// an enum to describe a prefered mode of quaternion interpolation
/// </summary>
public enum QuaternionInterpolationMode
{
	NoFlip,		// same as cached
	Average,	// linear average
	Shortest,	// constant velocity, shortest path
	Longest,	// constant velocity, longest path
	Cached,		// uses a cache of rotation values to pick from longest or shortest
	Slime		// use the slime algorithm
}

/// <summary>
/// a struct to store a target and a weight for a quaternion interpolation target
/// </summary>
public struct QuaternionInterpolationTarget
{
	public Quaternion quaternion;
	public float weight;
}

/// <summary>
/// A class of helper functions for working with quaternions
/// </summary>
public static class QuaternionHelpers : System.Object
{
	/// <summary>
	/// Build a Quaternion that where aimVector is pointed forward and upVector is pointed upwards
	/// </summary>
	/// <param name="forward">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <param name="upwards">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <param name="aimVector">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <param name="upVector">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion CustomLookRotation(Vector3 forward, Vector3 upwards, Vector3 aimVector, Vector3 upVector)
	{
		return Quaternion.LookRotation(forward, upwards) * Quaternion.Inverse(Quaternion.LookRotation(aimVector, upVector));
	}
	
	/// <summary>
	/// variable for building Euler angles from a quaternion: each element corresponds to an EulerRotationOrder
	/// </summary>
	private static int[] yawAxes = new int[6]			{   0,   1,   2,   0,   1,   2 };
	/// <summary>
	/// variable for building Euler angles from a quaternion: each element corresponds to an EulerRotationOrder
	/// </summary>
	private static int[] pitchAxes = new int[6]			{   1,   2,   0,   2,   0,   1 };
	/// <summary>
	/// variable for building Euler angles from a quaternion: each element corresponds to an EulerRotationOrder
	/// </summary>
	private static int[] rollAxes =	new int[6]			{   2,   0,   1,   1,   2,   0 };
	/// <summary>
	/// variable for building Euler angles from a quaternion: each element corresponds to an EulerRotationOrder
	/// </summary>
	private static float[] pitchScalars = new float[6]	{ -1f, -1f, -1f,  1f,  1f,  1f };
	/// <summary>
	/// allocation for computation
	/// </summary>
	private static float yawAngle = 0f;
	/// <summary>
	/// allocation for computation
	/// </summary>
	private static float pitchAngle = 0f;
	/// <summary>
	/// allocation for computation
	/// </summary>
	private static float rollAngle = 0f;
	
	/// <summary>
	/// Return a quaternion as Euler angles using the supplied rotation order (ZXY corresponds to Quaternion.eulerAngles)
	/// </summary>
	/// <param name="q">
	/// A <see cref="Quaternion"/>
	/// </param>
	/// <param name="order">
	/// A <see cref="EulerRotationOrder"/>
	/// </param>
	/// <returns>
	/// A <see cref="Vector3"/>
	/// </returns>
	public static Vector3 ToEulerAngles(Quaternion q, EulerRotationOrder order)
	{
		FloatMatrix rotationMatrix = new FloatMatrix(new Vector3[3] { q*Vector3.right, q*Vector3.up, q*Vector3.forward });
		
		pitchAngle = Mathf.Rad2Deg*Mathf.Asin(pitchScalars[(int)order]*rotationMatrix[yawAxes[(int)order],rollAxes[(int)order]]);
		if (pitchAngle < 90f)
		{
			if (pitchAngle > -90f)
			{
				yawAngle = Mathf.Rad2Deg*Mathf.Atan2(-pitchScalars[(int)order]*rotationMatrix[pitchAxes[(int)order],rollAxes[(int)order]],rotationMatrix[rollAxes[(int)order],rollAxes[(int)order]]);
				rollAngle = Mathf.Rad2Deg*Mathf.Atan2(-pitchScalars[(int)order]*rotationMatrix[yawAxes[(int)order],pitchAxes[(int)order]],rotationMatrix[yawAxes[(int)order],yawAxes[(int)order]]);
			}
			else
			{
				// non-unique solution
				rollAngle = 0f;
				yawAngle = rollAngle - Mathf.Rad2Deg*Mathf.Atan2(pitchScalars[(int)order]*rotationMatrix[pitchAxes[(int)order],yawAxes[(int)order]],rotationMatrix[pitchAxes[(int)order],pitchAxes[(int)order]]);
			}
		}
		else
		{
			// non-unique solution
			rollAngle = 0f;
			yawAngle = Mathf.Rad2Deg*Mathf.Atan2(pitchScalars[(int)order]*rotationMatrix[pitchAxes[(int)order],yawAxes[(int)order]],rotationMatrix[pitchAxes[(int)order],pitchAxes[(int)order]]) - rollAngle;
		}
		
		
		// pack the angles into a vector
		Vector3 ret = Vector3.zero;
		ret[rollAxes[(int)order]] = rollAngle;
		ret[yawAxes[(int)order]] = yawAngle;
		ret[pitchAxes[(int)order]] = pitchAngle;
		
		// return the result
		return ret;
	}
	
	/// <summary>
	/// variables for building a Quaternion from Euler angles
	/// </summary>
	private static FloatMatrix[] rotationMatrices = new FloatMatrix[3]
	{
		new FloatMatrix(new Vector3[3] { Vector3.right,	Vector3.zero,	Vector3.zero }),
		new FloatMatrix(new Vector3[3] { Vector3.zero,	Vector3.up,		Vector3.zero }),
		new FloatMatrix(new Vector3[3] { Vector3.zero,	Vector3.zero,	Vector3.forward })
	};
	
	/// <summary>
	/// Return a quaternion corresponding to the supplied Euler angles with the given order (ZXY corresponds to Quaternion.eulerAngles)
	/// </summary>
	/// <param name="eulerAngles">
	/// A <see cref="Vector3"/>
	/// </param>
	/// <param name="order">
	/// A <see cref="EulerRotationOrder"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion FromEulerAngles(Vector3 eulerAngles, EulerRotationOrder order)
	{
		// build rotation matrices
		rotationMatrices[0][1,1] = Mathf.Cos(Mathf.Deg2Rad*eulerAngles.x);
		rotationMatrices[0][2,2] =  rotationMatrices[0][1,1];
		rotationMatrices[0][1,2] = Mathf.Sin(Mathf.Deg2Rad*eulerAngles.x);
		rotationMatrices[0][2,1] = -rotationMatrices[0][1,2];
		
		rotationMatrices[1][2,2] = Mathf.Cos(Mathf.Deg2Rad*eulerAngles.y);
		rotationMatrices[1][0,0] =  rotationMatrices[1][2,2];
		rotationMatrices[1][2,0] = Mathf.Sin(Mathf.Deg2Rad*eulerAngles.y);
		rotationMatrices[1][0,2] = -rotationMatrices[1][2,0];
		
		rotationMatrices[2][0,0] = Mathf.Cos(Mathf.Deg2Rad*eulerAngles.z);
		rotationMatrices[2][1,1] =  rotationMatrices[2][0,0];
		rotationMatrices[2][0,1] = Mathf.Sin(Mathf.Deg2Rad*eulerAngles.z);
		rotationMatrices[2][1,0] = -rotationMatrices[2][0,1];
		
		// composite rotation matrices
		FloatMatrix m = rotationMatrices[yawAxes[(int)order]]*rotationMatrices[pitchAxes[(int)order]]*rotationMatrices[rollAxes[(int)order]];
		
        // build quaternion; see Shoemake, Ken (1987) "Quaternion Calculus and Fast Animation"
		float trace = m.trace;
		float root = 0f;
		Vector4 components = Vector4.zero;
		
		if (trace > 0f)
		{
			// |w| > 0.5f, may as well choose w > 0.5f;
			root = Mathf.Sqrt(trace+1f);
			components.w = 0.5f*root;
			root = 0.5f/root;
			components.x = (m[2,1]-m[1,2])*root;
			components.y = (m[0,2]-m[2,0])*root;
			components.z = (m[1,0]-m[0,1])*root;
		}
		else
		{
			// |w| <= 0.5f
			int[] iNext = new int[3] { 1, 2, 0 };
			int i = 0;
			if (m[1,1] > m[0,0]) i = 1;
			if (m[2,2] > m[i,i]) i = 2;
			int j = iNext[i];
			int k = iNext[j];
			
			root = Mathf.Sqrt(m[i,i]-m[j,j]-m[k,k] + 1f);
			components[i] = 0.5f*root;
			root = 0.5f/root;
			components[3] = (m[k,j]-m[j,k])*root;
			components[j] = (m[j,i]+m[i,j])*root;
			components[k] = (m[k,i]+m[i,k])*root;
		}
		// ensure result is left-handed
		return new Quaternion(components.x, components.y, components.z, -components.w);
	}
	
	/// <summary>
	/// Perform a quaternion slerp using the long path
	/// </summary>
	/// <param name="from">
	/// A <see cref="Quaternion"/>
	/// </param>
	/// <param name="to">
	/// A <see cref="Quaternion"/>
	/// </param>
	/// <param name="t">
	/// A <see cref="System.Single"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion SlerpLong(Quaternion from, Quaternion to, float t)
	{
		// calculate angle
		float cosHalfTheta = Quaternion.Dot(from, to);
		
		// if from == to or from == -to then theta == 0 and we can return from
		if (Mathf.Abs(cosHalfTheta) >= 1.0)
		{
			return from;
		}
		else
		{
			// does rotation need to be inverted?
			Quaternion q;
			if (cosHalfTheta > 0f)
			{
				cosHalfTheta *= -1f;
				q = new Quaternion(-from.x, -from.y, -from.z, -from.w);
			}
			else
			{
				q = from;
			}
			
			// calculate temporary values
			float halfTheta = Mathf.Acos(cosHalfTheta);
			float sinHalfTheta = Mathf.Sqrt(1f - cosHalfTheta*cosHalfTheta);
			
			// do linear interpolation
			if (Mathf.Abs(sinHalfTheta) == 0f)
			{
				return new Quaternion(from.x*0.5f + to.x*0.5f, from.y*0.5f + to.y*0.5f, from.z*0.5f + to.z*0.5f, from.w*0.5f + to.w*0.5f);
			}
			
			// do slerp
			else
			{
				float oneOverSinHalfTheta = 1f/sinHalfTheta;
				float ratioA = Mathf.Sin((1f-t)*halfTheta)*oneOverSinHalfTheta;
				float ratioB = Mathf.Sin(t*halfTheta)*oneOverSinHalfTheta;
				// compute the quaternion
				return new Quaternion(q.x*ratioA + to.x*ratioB, q.y*ratioA + to.y*ratioB, q.z*ratioA + to.z*ratioB, q.w*ratioA + to.w*ratioB);
			}
		}
	}
	
	/// <summary>
	/// Normalize the weights of an array of interpolation targets to 1.0
	/// </summary>
	/// <param name="targets">
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </returns>
	public static QuaternionInterpolationTarget[] NormalizeTargetWeights(QuaternionInterpolationTarget[] targets)
	{
		float sumOfAllWeights = 0f;
		for (int i=0; i<targets.Length; i++)
			sumOfAllWeights += targets[i].weight;
		float oneOverSum = 1f/sumOfAllWeights;
		for (int i=0; i<targets.Length; i++)
			targets[i].weight *= oneOverSum;
		return targets;
	}
	
	/// <summary>
	/// Do an interpolation using the specified mode
	/// </summary>
	/// <param name="normalizedTargets">
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </param>
	/// <param name="interpType">
	/// A <see cref="QuaternionInterpolationMode"/>
	/// </param>
	/// <param name="cache">
	/// A <see cref="Quaternion[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion Interpolate(QuaternionInterpolationTarget[] normalizedTargets, QuaternionInterpolationMode interpType, ref Quaternion[] cache)
	{
		// pass in denominator
		return Interpolate(normalizedTargets, interpType, ref cache, 1f/normalizedTargets.Length);
	}
	public static Quaternion Interpolate(QuaternionInterpolationTarget[] normalizedTargets, QuaternionInterpolationMode interpType, ref Quaternion[] cache, float oneOverListLength)
	{
		switch (interpType)
		{
		case QuaternionInterpolationMode.Average:
			return InterpolateAverage(normalizedTargets);
		case QuaternionInterpolationMode.Slime:
			return InterpolateSlime(normalizedTargets);
		case QuaternionInterpolationMode.Longest:
			return SequentialSlerp(normalizedTargets, false, ref cache);
		default:
			return SequentialSlerp(normalizedTargets, true, ref cache);
		}
	}
	
	/// <summary>
	/// Perform a sequential slerp ala Maya's orientConstraint node
	/// </summary>
	/// <param name="normalizedTargets">
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </param>
	/// <param name="isShortPath">
	/// A <see cref="System.Boolean"/>
	/// </param>
	/// <param name="cache">
	/// A <see cref="Quaternion[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion SequentialSlerp(QuaternionInterpolationTarget[] normalizedTargets, bool isShortPath, ref Quaternion[] cache)
	{
		// the candidate return value
		Quaternion q = normalizedTargets[0].quaternion;
		
		for (int i=1; i<normalizedTargets.Length; i++)
		{
			if (isShortPath) q = Quaternion.Slerp(q, normalizedTargets[i].quaternion, normalizedTargets[i].weight);
			else q = SlerpLong(q, normalizedTargets[i].quaternion, normalizedTargets[i].weight);
		}
		
		// TODO: if using a caching mode, then do a neighborhood test
		
		// shift the cache and add the new result to the end of it
		for (int i=1; i<cache.Length; i++)
			cache[i-1] = cache[i];
		cache[cache.Length-1] = q;
		
		// return the result
		return q;
	}
	
	/// <summary>
	/// A multiple interpolation algorithm that simply does a sequential nlerp
	/// </summary>
	/// <param name="normalizedTargets">
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion InterpolateAverage(QuaternionInterpolationTarget[] normalizedTargets)
	{
		// the candidate return value
		Quaternion q = normalizedTargets[0].quaternion;
		for (int i=1; i<normalizedTargets.Length; i++)
			 q = Quaternion.Lerp(q, normalizedTargets[i].quaternion, normalizedTargets[i].weight);
		return q;
	}
	
	/// <summary>
	/// Perform Slime interpolation across the targets
	/// Formula: reference*e^(sum(weight*ln(reference.conjugate*target.rotation)))
	/// See p.130, 136: http://alumni.media.mit.edu/~aries/papers/johnson_phd.pdf
	/// </summary>
	/// <param name="targets">
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion InterpolateSlime(QuaternionInterpolationTarget[] targets)
	{
		// call other version of method
		return InterpolateSlime(NormalizeTargetWeights(targets), 1f/targets.Length);
	}
	public static Quaternion InterpolateSlime(QuaternionInterpolationTarget[] normalizedTargets, float oneOverListLength)
	{
		// get the reference quaternion
		Quaternion reference = Mean(normalizedTargets);
		
		// sum the logs of each quaternion in the space of the reference
		Vector4 sumOfWeightedQuaternions = Vector4.zero;
		for (int i=0; i<normalizedTargets.Length; i++)
			sumOfWeightedQuaternions += normalizedTargets[i].weight*Log(Quaternion.Inverse(reference)*normalizedTargets[i].quaternion);
		
		// multiply the reference quaternion by the result
		return reference * Exp(sumOfWeightedQuaternions);
	}
	
	/// <summary>
	/// the first guess to use for power iteration when estimating mean is Vector4.one.normalized
	/// </summary>
	private static readonly FloatMatrix firstGuess = new FloatMatrix(new float[4] { 0.5f, 0.5f, 0.5f, 0.5f });
	
	/// <summary>
	/// Compute the mean of an array of quaternions
	/// </summary>
	/// <param name="targets">
	/// A <see cref="QuaternionInterpolationTarget[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion Mean(QuaternionInterpolationTarget[] targets)
	{
		Quaternion[] samples = new Quaternion[targets.Length];
		for (int i=0; i<targets.Length; i++)
			samples[i] = targets[i].quaternion;
		return Mean (samples);
	}
	/// <summary>
	/// Compute the mean of an array of quaternions
	/// </summary>
	/// <param name="quaternions">
	/// A <see cref="Quaternion[]"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion Mean(Quaternion[] quaternions)
	{
		// get data matrix and its transpose
		FloatMatrix q = new FloatMatrix(quaternions);
		FloatMatrix qt = q.transpose;
		
		// scatter matrix (outer product of data matrix)
		FloatMatrix s = qt*q;
		
		// get eigenvector of maximal eigenvalue using power-iteration method
		FloatMatrix currentGuess = firstGuess;
//		int iterations = 4; // NOTE: one iteration seems to suffice in all test cases so far
//		for (int i=0; i<iterations; ++i)
//		{
			currentGuess *= s;
			currentGuess = currentGuess * (1f/new Vector4(currentGuess[0,0], currentGuess[1,0], currentGuess[2,0], currentGuess[3,0]).magnitude);
//		}
		return new Quaternion(currentGuess[0,0], currentGuess[1,0], currentGuess[2,0], currentGuess[3,0]);
	}
	
	/// <summary>
	/// Compute the exponentiation of a quaternion
	/// </summary>
	/// <param name="q">
	/// A <see cref="Quaternion"/>
	/// </param>
	/// <returns>
	/// A <see cref="Quaternion"/>
	/// </returns>
	public static Quaternion Exp(Quaternion q)
	{
		return Exp(new Vector4(q.x, q.y, q.z, q.w));
	}
	public static Quaternion Exp(Vector4 q)
	{
		float a = new Vector3(q.x, q.y, q.z).magnitude;
		float sina = Mathf.Sin(a);
		
		float x=0f, y=0f, z=0f, w=Mathf.Cos(a);
		if (Mathf.Abs(a) > 0f)
		{
			float oneOverA = 1f/a;
			x = sina*q.x*oneOverA;
			y = sina*q.y*oneOverA;
			z = sina*q.z*oneOverA;
		}
		return new Quaternion(x, y, z, w);
	}
	
	/// <summary>
	/// Compute the log of a quaternion
	/// </summary>
	/// <param name="q">
	/// A <see cref="Quaternion"/>
	/// </param>
	/// <returns>
	/// A <see cref="Vector4"/>
	/// </returns>
	public static Vector4 Log(Quaternion q)
	{
		float a = Mathf.Acos(q.w);
		float sina = Mathf.Sin(a);
		
		float x=0f, y=0f, z=0f, w=0f;
		if (Mathf.Abs(sina) > 0f)
		{
			float oneOverSina = 1f/sina;
			x = a*q.x*oneOverSina;
			y = a*q.y*oneOverSina;
			z = a*q.z*oneOverSina;
		}
		return new Vector4(x, y, z, w);
	}
}