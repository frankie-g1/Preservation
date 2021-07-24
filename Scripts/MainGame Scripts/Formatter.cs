using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formatter
{
    public static int[] stringToValue(string number)
    {
        int[] arr = new int[2];
        if (number.Length > 18) { Debug.Log("Number too Large"); return null; }
        else if (number.Length > 9)
        {
            string pt1 = number.Substring((number.Length - 9));
            string pt2 = number.Substring(0, (number.Length - 9));
            try
            {
                arr[0] = int.Parse(pt1);
                arr[1] = int.Parse(pt2);
                return arr;
            }
            catch (System.FormatException)
            {
                Debug.Log("Invalid number");
                return null;
            }
        }
        else
        {
            string pt1 = number.Substring(0, number.Length);
            try
            {
                arr[0] = int.Parse(pt1);
                arr[1] = 0;
                return arr;
            }
            catch (System.FormatException)
            {
                Debug.Log("Invalid number");
                return null;
            }
        }
    }

    /// <summary>
    /// Creates a string for displaying money and rounds the money and adds suffixes when approaching large amounts.
    /// </summary>
    /// <returns> string with rounded and reformatted money value so that it stays compact</returns>
    public static string moneyString(int[] val)
    {
        int roundedMoney = 0;
        string suffix = "";
        if (val[1] > 9999999)
        {
            roundedMoney = val[1] / 1000000;
            suffix = "Q";
        }
        else if (val[1] > 9999)
        {
            roundedMoney = val[1] / 1000;
            suffix = "T";
        }
        else if (val[1] > 9)
        {
            roundedMoney = val[1];
            suffix = "B";
        }
        else if (val[0] > 9999999 || val[1] > 0)
        {
            if (val[1] > 0)
            {
                roundedMoney = val[0] / 1000000;
                roundedMoney += (1000 * val[1]);
            }
            else
            {
                roundedMoney = val[0] / 1000000;
            }

            suffix = "M";
        }
        else
        {
            roundedMoney = val[0];
        }

        return "$" + roundedMoney.ToString() + suffix;
    }

    public static int[] addValueArrays(int[] arr1, int[] arr2) {
        int[] finalArr = new int[2];
        int[] tempArr2 = new int[2];
        finalArr[0] = arr1[0];
        finalArr[1] = arr1[1];
        tempArr2[0] = arr2[0];
        tempArr2[1] = arr2[1];
        while (finalArr[0] > 999999999) {
            finalArr[0] -= 1000000000;
            if (finalArr[1] + 1 > 999999999)
            {
                finalArr[1] = 999999999;
                finalArr[0] = 999999999;
                return finalArr;
            }
            else {
                finalArr[1] += 1;
            }
        }

        while (tempArr2[0] > 999999999)
        {
            tempArr2[0] -= 1000000000;
            if (finalArr[1] + 1 > 999999999)
            {
                finalArr[1] = 999999999;
                finalArr[0] = 999999999;
                return finalArr;
            }
            else
            {
                finalArr[1] += 1;
            }
        }

        finalArr[0] += tempArr2[0];

        if (finalArr[1] + tempArr2[1] > 999999999)
        {
            finalArr[1] = 999999999;
            finalArr[0] = 999999999;
            return finalArr;
        }
        else {
            finalArr[1] += tempArr2[1];
            return finalArr;
        }
        
        

    }

    public static void resetTree(bool[] tree)
    {
        for (int i = 0; i < tree.Length; i++)
        {
            tree[i] = false;
        }
    }


}
