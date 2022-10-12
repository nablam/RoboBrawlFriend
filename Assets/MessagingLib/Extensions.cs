using System;
using UnityEngine;

public static class Extensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        if (!camera.orthographic)
        {
            Debug.Log(string.Format("The camera {0} is not Orthographic!", camera.name), camera);
            return new Bounds();
        }

        var t = camera.transform;
        var x = t.position.x;
        var y = t.position.y;
        var size = camera.orthographicSize * 2;
        var width = size * (float)Screen.width / Screen.height;
        var height = size;

        return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
    }

    public static float RemapPositive(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static float RemapNegPos(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    public static float RemapUnity(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float normal = Mathf.InverseLerp(fromMin, fromMax, from);
        float bValue = Mathf.Lerp(toMin, toMax, normal);

        return bValue;

    }


    public static char[] delimiterChars = { 'x', ' ', ',', '.', ':', '\t' };

    public static int Get_ResolutionType_widthInt(ResolutionsTypes argrestype)
    {
        string _strip_undescore = argrestype.ToString().Remove(0, 1);
        string[] words = _strip_undescore.Split(delimiterChars);
        if (words.Length != 2)
        {
            return 0;
        }

        return Int32.Parse(words[0]);
        //_hight = Int32.Parse(words[1]);
    }

    public static int Get_ResolutionType_HeightInt(ResolutionsTypes argrestype)
    {
        string _strip_undescore = argrestype.ToString().Remove(0, 1);
        string[] words = _strip_undescore.Split(delimiterChars);
        if (words.Length != 2)
        {
            return 0;
        }

        return Int32.Parse(words[1]);
        //_hight = Int32.Parse(words[1]);
    }





   
    public static string Convert_EnumName_ToName(VideoCapturerNames argEnumName)
    {
 
        string inputstr = argEnumName.ToString();
        inputstr = inputstr.Replace('X', '-');
        inputstr = inputstr.Replace('x', ',');
        inputstr = inputstr.Replace('_', ' ');
        inputstr = inputstr.Replace('Q', '(');
        inputstr = inputstr.Replace('Z', ')');

       return inputstr;
    }


}

