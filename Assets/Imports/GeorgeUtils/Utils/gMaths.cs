using UnityEngine;
using System.Collections;

namespace GeorgeUtils
{
	public class gMaths {

		#region Addition
		//	/// <summary>
		//	/// Adds a and b.
		//	/// </summary>
		//	/// <param name="a">The first int.</param>
		//	/// <param name="b">The second int.</param>
		public static int Add(int a, int b){
			return a + b;
		}
		/// <summary>
		/// Adds a and b.
		/// </summary>
		/// <param name="a">The first int.</param>
		/// <param name="b">The second int.</param>
		public static float Add(float a, float b){
			return a + b;
		}
		
		/// <summary>
		/// Adds all specified numbers.
		/// </summary>
		/// <param name="numbers">Numbers.</param>
		public static int Sum(params int[] numbers){
			int x = 0;
			for (int i = 0; i < numbers.Length; i++) {
				x += numbers[i];
			}
			return x;
		}
		/// <summary>
		/// Adds all specified numbers.
		/// </summary>
		/// <param name="numbers">Numbers.</param>
		public static float Sum(params float[] numbers){
			float x = 0;
			for (int i = 0; i < numbers.Length; i++) {
				x += numbers[i];
			}
			return x;
		}
		#endregion
		
		#region Subtraction
		/// <summary>
		/// Subtracts a from b.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static int Subtract(int a, int b){
			return a - b;
		}
		#endregion
		
		#region Multiplication
		/// <summary>
		/// Multiply a and b.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static int Multiply(int a, int b){
			return a * b;
		}
		/// <summary>
		/// Multiply a and b.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static float Multiply(float a, float b){
			return a * b;
		}
		
		/// <summary>
		/// Multiply the specified numbers.
		/// </summary>
		/// <param name="numbers">Numbers.</param>
		public static int Multiply(params int[] numbers){
			int x = 1;
			for (int i = 0; i < numbers.Length; i++) {
				x += x * numbers[i];
			}
			return x;
		}
		
		/// <summary>
		/// Multiply the specified numbers.
		/// </summary>
		/// <param name="numbers">Numbers.</param>
		public static float Multiply(params float[] numbers){
			float x = 1;
			for (int i = 0; i < numbers.Length; i++) {
				x += x * numbers[i];
			}
			return x;
		}
		
		/// <summary>
		/// Squares number.
		/// </summary>
		/// <param name="a">The value.</param>
		public static int Square(int a){
			return a * a;
		}
		/// <summary>
		/// Squares number.
		/// </summary>
		/// <param name="a">The value.</param>
		public static float Square(float a){
			return a * a;
		}
		
		/// <summary>
		/// Set a to the power of p.
		/// </summary>
		/// <param name="a">The value to power.</param>
		/// <param name="p">The power to which value will be increased by.</param>
		public static int Power(int a, int p){
			int x = 1;
			for (int i = 0; i < p; i++) {
				x = x * a;
			}
			return x;
		}
		/// <summary>
		/// Set a to the power of p.
		/// </summary>
		/// <param name="a">The value to power.</param>
		/// <param name="p">The power to which value will be increased by.</param>
		public static float Power(float a, float p){
			float x = 1;
			for (int i = 0; i < p; i++) {
				x = x * a;
			}
			return x;
		}
		#endregion
		
		#region division
		/// <summary>
		/// Divide a and b.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static int Divide(int a, int b){
			return a / b;
		}
		/// <summary>
		/// Divide a and b.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static float Divide(float a, float b){
			return a / b;
		}
		#endregion
		
		#region Percentages
		/// <summary>
		/// Find X percent of Y.
		/// </summary>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static float Percent(float a, float b){
			return (a / 100) * b;
		}
		
		/// <summary>
		/// Returns what percent X of Y is.
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static float PercentOf(float a, float b){
			return (a / b) * 100;
		}
		
		/// <summary>
		/// Perecentage increase from a to b.
		/// </summary>
		/// <returns>The increase.</returns>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static int PercentIncrease(int a, int b){
			return ((b - a) / a) * 100;
		}
		/// <summary>
		/// Perecentage increase from a to b.
		/// </summary>
		/// <returns>The increase.</returns>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		public static float PercentIncrease(float a, float b){
			return ((b - a) / a) * 100;
		}
		#endregion

	}
}
