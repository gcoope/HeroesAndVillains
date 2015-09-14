using UnityEngine;
using System.Collections;

namespace GeorgeUtils
{
	public class gGUI : MonoBehaviour {

		/// <summary>
		/// Returns word-wrapped string.
		/// </summary>
		/// <returns>The wrapped string.</returns>
		/// <param name="inputText">Input text.</param>
		/// <param name="lineLength">Line length.</param>
		public static string WordWrapText(string inputText, int lineLength){			
			// Split string by char " "       
			string[] words = inputText.Split(" "[0]);			
			// Prepare result
			string result = "";			
			// Temp line string
			string emptyLine = "";			
			// for each all words        
			foreach(string s in words){
				// Append current word into line
				string tempString = emptyLine + " " + s;				
				// If line length is bigger than lineLength
				if(tempString.Length > lineLength){					
					// Append current line into result
					result += emptyLine + "\n";
					// Remain word append into new line
					emptyLine = s;
				}
				// Append current word into current line
				else {
					emptyLine = tempString;
				}
			}		
			// Append last line into result      
			result += emptyLine;			
			// Remove first " " char
			return result.Substring(1,result.Length-1);
		}

	}
}