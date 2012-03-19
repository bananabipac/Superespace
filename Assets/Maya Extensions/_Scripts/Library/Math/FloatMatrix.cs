using UnityEngine;
using System.Collections;
using System.Text;

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
/// This file contains a basic class for a FloatMatrix of arbitrary order.
/// 
/// This class should be considered a utility only. Because it allows for
/// matrices of arbitrary order, it does little or no validation for many
/// operations. As such, make sure you know what you are doing, or catch
/// exceptions as needed in your own code.
/// </summary>
/// 
/// </file>

/// <summary>
/// A generic class for row-major float matrics
/// </summary>
public class FloatMatrix : System.Object
{
	/// <summary>
	/// the data in the matrix
	/// </summary>
	private float[,] data;
	
	/// <summary>
	/// retrieve row count
	/// </summary>
	public int rows { get { return data.GetUpperBound(0)+1; } }
	/// <summary>
	/// retrieve column count
	/// </summary>
	public int columns { get { return data.GetUpperBound(1)+1; } }
	
	/// <summary>
	/// index accessor
	/// </summary>
	/// <param name="r">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="c">
	/// A <see cref="System.Int32"/>
	/// </param>
	public float this[int r, int c]
	{
		get { return data[r,c]; }
		set { data[r,c] = value; }
	}
	
	/// <summary>
	/// is it a square matrix?
	/// </summary>
	public bool isSquare { get { return rows==columns; } }
	/// <summary>
	/// Throw an exception that the matrix is not square
	/// </summary>
	private void IsNotSquare()
	{
		throw new System.MemberAccessException(
			string.Format(
				"Cannot compute determinant of a non-square matrix. Found {0} x {1}",
				rows, columns
			)
		);
	}
	
	/// <summary>
	/// get the determinant if it is a square matrix
	/// </summary>
	public float determinant
	{
		get
		{
			if (!isSquare) IsNotSquare();
			
			// take shortcuts if it is a small matrix
			switch (rows)
			{
			case 1:
				return this[0,0];
			case 2:
				return (this[0,0]*this[1,1] - this[1,0]*this[0,1]);
			case 3:
				return
					this[0,0]*this[1,1]*this[2,2] + 
					this[0,1]*this[1,2]*this[2,0] + 
					this[0,2]*this[1,0]*this[2,1] - 
					this[0,0]*this[1,2]*this[2,1] - 
					this[0,1]*this[1,0]*this[2,2] - 
					this[0,2]*this[1,1]*this[2,0];
			// otherwise recursively cut up the matrix
			default:
				int i,j,j1,j2;
				float[,] m;
				float det = 0;
				for (j1=0; j1<rows; j1++)
				{
					m = new float[rows-1, rows-1];
					for (i=1; i<rows; i++)
					{
						j2 = 0;
						for (j=0; j<rows; j++)
						{
							if (j == j1) continue;
							m[i-1,j2] = this[i,j];
							j2++;
						}
					}
					FloatMatrix fm = new FloatMatrix(m);
					det += Mathf.Pow(-1f,1f+j1+1f) * this[0,j1] * fm.determinant;
				}
				return det;
			}
		}
	}
	
	/// <summary>
	/// get the transpose (e.g., mxn becomes nxm)
	/// </summary>
	public FloatMatrix transpose
	{
		get
		{
			float[,] t = new float[columns, rows];
			for (int c=0; c<columns; c++)
			{
				for (int r=0; r<rows; r++)
					t[c,r]=this[r,c];
			}
			return new FloatMatrix(t);
		}
	}
	
	/// <summary>
	/// get the trace if it is a square matrix
	/// </summary>
	public float trace
	{
		get
		{
			if (!isSquare) IsNotSquare();
			float t = 0f;
			for (int i=0; i<rows; i++) t+= this[i,i];
			return t;
		}
	}
	
	/// <summary>
	/// 2x2 identity matrix
	/// </summary>
	public static readonly FloatMatrix identity2x2 = new FloatMatrix(new Vector2[2] { Vector2.right, Vector2.up });
	/// <summary>
	/// 3x3 identity matrix
	/// </summary>
	public static readonly FloatMatrix identity3x3 = new FloatMatrix(new Vector3[3] { Vector3.right, Vector3.up, Vector3.forward });
	/// <summary>
	/// 4x4 identity matrix
	/// </summary>
	public static readonly FloatMatrix _identity4x4 = new FloatMatrix(new Vector4[4] { new Vector4(1f,0f,0f,0f), new Vector4(0f,1f,0f,0f), new Vector4(0f,0f,1f,0f), new Vector4(0f,0f,0f,1f) });
	
	/// <summary>
	/// Construct a new matrix with specified rows and columns
	/// </summary>
	/// <param name="rows">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="columns">
	/// A <see cref="System.Int32"/>
	/// </param>
	public FloatMatrix(int rows, int columns)
	{
		if (rows < 1 || columns < 1) throw new System.ArgumentOutOfRangeException("Must specify positive, non-zero rows and columns.");
		this.data = new float[rows,columns];
	}
	
	/// <summary>
	/// Construct a new matrix from a 2D float array
	/// </summary>
	/// <param name="data">
	/// A <see cref="System.Single[,]"/>
	/// </param>
	public FloatMatrix(float[,] data)
	{
		if (data == null) throw new System.NullReferenceException();
		if (data.GetUpperBound(0) < 1 || data.GetUpperBound(1) < 1) throw new System.ArgumentOutOfRangeException();
		this.data = data;
	}
	
	/// <summary>
	/// Construct a nx1 matrix - each row is an entry
	/// </summary>
	/// <param name="data">
	/// A <see cref="System.Single[]"/>
	/// </param>
	public FloatMatrix(float[] data)
	{
		if (data == null) throw new System.NullReferenceException();
		if (data.Length < 1) throw new System.ArgumentOutOfRangeException();
		this.data = new float[data.Length,1];
		for (int i=0; i<data.Length; i++)
			this.data[i,0] = data[i];
	}
	
	/// <summary>
	/// Construct a nx2 matrix - each row is a Vector2
	/// </summary>
	/// <param name="vectors">
	/// A <see cref="Vector2[]"/>
	/// </param>
	public FloatMatrix(Vector2[] vectors)
	{
		if (vectors == null) throw new System.NullReferenceException();
		if (vectors.Length < 1) throw new System.ArgumentOutOfRangeException();
		this.data = new float[vectors.Length, 2];
		for (int i=0; i<vectors.Length; i++)
		{
			this.data[i,0] = vectors[i].x;
			this.data[i,1] = vectors[i].y;
		}
	}
	
	/// <summary>
	/// Construct a nx3 matrix - each row is a Vector3
	/// </summary>
	/// <param name="vectors">
	/// A <see cref="Vector3[]"/>
	/// </param>
	public FloatMatrix(Vector3[] vectors)
	{
		if (vectors == null) throw new System.NullReferenceException();
		if (vectors.Length < 1) throw new System.ArgumentOutOfRangeException();
		this.data = new float[vectors.Length, 3];
		for (int i=0; i<vectors.Length; i++)
		{
			this.data[i,0] = vectors[i].x;
			this.data[i,1] = vectors[i].y;
			this.data[i,2] = vectors[i].z;
		}
	}
	
	/// <summary>
	/// Construct a nx4 matrix - each row is a Vector4
	/// </summary>
	/// <param name="vectors">
	/// A <see cref="Vector4[]"/>
	/// </param>
	public FloatMatrix(Vector4[] vectors)
	{
		if (vectors == null) throw new System.NullReferenceException();
		if (vectors.Length < 1) throw new System.ArgumentOutOfRangeException();
		this.data = new float[vectors.Length, 4];
		for (int i=0; i<vectors.Length; i++)
		{
			this.data[i,0] = vectors[i].x;
			this.data[i,1] = vectors[i].y;
			this.data[i,2] = vectors[i].z;
			this.data[i,3] = vectors[i].w;
		}
	}
	
	/// <summary>
	/// Construct a nx4 matrix - each row is a Quaternion
	/// </summary>
	/// <param name="quaternions">
	/// A <see cref="Quaternion[]"/>
	/// </param>
	public FloatMatrix(Quaternion[] quaternions)
	{
		if (quaternions == null) throw new System.NullReferenceException();
		if (quaternions.Length < 1) throw new System.ArgumentOutOfRangeException();
		this.data = new float[quaternions.Length, 4];
		for (int i=0; i<quaternions.Length; i++)
		{
			this.data[i,0] = quaternions[i].x;
			this.data[i,1] = quaternions[i].y;
			this.data[i,2] = quaternions[i].z;
			this.data[i,3] = quaternions[i].w;
		}
	}
	
	/// <summary>
	/// Copy constructor
	/// </summary>
	/// <param name="m">
	/// A <see cref="FloatMatrix"/>
	/// </param>
	public FloatMatrix(FloatMatrix m)
	{
		if (m == null) throw new System.NullReferenceException();
		this.data = new float[m.rows, m.columns];
		for (int i=0; i<m.rows; i++)
			for (int j=0; j<m.columns; j++)
				this.data[i,j] = m[i,j];
	}
	
	/// <summary>
	/// Copy Matrix4x4 into a 4x4 FloatMatrix
	/// </summary>
	/// <param name="m">
	/// A <see cref="Matrix4x4"/>
	/// </param>
	public FloatMatrix(Matrix4x4 m)
	{
		this.data = new float[4,4];
		for (int i=0; i<4; i++)
			for (int j=0; j<4; j++)
				this.data[i,j] = m[i,j];
	}
	
	/// <summary>
	/// Format nicely
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/>
	/// </returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		for (int i=0; i<rows; i++)
		{
			sb.Append("[");
			for (int j=0; j<columns; j++)
			{
				sb.Append(string.Format("{0:0.00000}", data[i,j]));
				if (j<columns-1) sb.Append(" ");
			}
			sb.Append("]");
			if (i<rows-1) sb.Append("\n");
		}
		return sb.ToString();
	}
	
	/// <summary>
	/// Matrix multiplication
	/// </summary>
	/// <param name="m1">
	/// A <see cref="FloatMatrix"/>
	/// </param>
	/// <param name="m2">
	/// A <see cref="FloatMatrix"/>
	/// </param>
	/// <returns>
	/// A <see cref="FloatMatrix"/>
	/// </returns>
	public static FloatMatrix operator *(FloatMatrix m1, FloatMatrix m2) 
	{
		if (m1.columns != m2.rows) throw new System.ArithmeticException("m1 columns must be the same as m2 rows");
		float[,] s = new float[m1.rows,m2.columns];
		for (int m1Row=0; m1Row<m1.rows; m1Row++)
			for (int m2Col=0; m2Col<m2.columns; m2Col++)
				for (int m1Col=0; m1Col<m1.columns; m1Col++)
					s[m1Row,m2Col] += m1[m1Row,m1Col]*m2[m1Col,m2Col];
		return new FloatMatrix(s);
	}
	
	/// <summary>
	/// Multiplication scalar
	/// </summary>
	/// <param name="m">
	/// A <see cref="FloatMatrix"/>
	/// </param>
	/// <param name="s">
	/// A <see cref="System.Single"/>
	/// </param>
	/// <returns>
	/// A <see cref="FloatMatrix"/>
	/// </returns>
	public static FloatMatrix operator *(FloatMatrix m, float s)
	{
		for (int i=0; i<m.rows; i++)
		{
			for (int j=0; j<m.columns; j++)
			{
				m[i,j] *= s;
			}
		}
		return m;
	}
	
	/// <summary>
	/// Division scalar
	/// </summary>
	/// <param name="m">
	/// A <see cref="FloatMatrix"/>
	/// </param>
	/// <param name="s">
	/// A <see cref="System.Single"/>
	/// </param>
	/// <returns>
	/// A <see cref="FloatMatrix"/>
	/// </returns>
	public static FloatMatrix operator /(FloatMatrix m, float s)
	{
		return m*(1f/s);
	}
}