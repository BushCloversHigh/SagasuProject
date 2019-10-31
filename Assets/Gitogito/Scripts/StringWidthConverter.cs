using UnityEngine;

public class StringWidthConverter : MonoBehaviour
{
    const int ConvertionConstant = 65248;

    static public string ConvertToFullWidth (string halfWidthStr)
    {
        string fullWidthStr = null;

        for (int i = 0 ; i < halfWidthStr.Length ; i++)
        {
            fullWidthStr += (char)(halfWidthStr[i] + ConvertionConstant);
        }

        return fullWidthStr;
    }
}
