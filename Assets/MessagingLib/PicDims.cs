using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicDims  
{
    string _name;
    string _resolutionStr;
    int _width;
    int _hight;
    int _id;

    ResolutionsTypes _restype;
    VideoCapturerNames _myEnumName;
    public VideoCapturerNames getMyEnumName() { return this._myEnumName; }
    public string Name { get => _name; set => _name = value; }
    //public string Resolution {

    //   private get=> _Resolution; 
    //    set
    //    {
    //            _Resolution = value;
    //            // if (hasInitDone) Initialize(); we will handle initialization explicitly
    //    }

    //}
    public int ID { get => _id; private set => _id = value; }
    public int Width { get => _width; private set => _width = value; }
    public int Hight { get => _hight; private set => _hight = value; }

    public PicDims(int argid, VideoCapturerNames argEnumName, ResolutionsTypes argrestype )
    {
        this._id = argid;
        this._myEnumName = argEnumName;
        Convert_EnumName_ToName();
        this._restype = argrestype;
        _resolutionStr = _restype.ToString().Remove(0, 1);

        GenerateIntegerDims(_resolutionStr);
    }
    char[] delimiterChars = { 'x',' ', ',', '.', ':', '\t' };
    public PicDims()
    {
        this._id = -1;
    }
    public void ResetMyValues(int argid, VideoCapturerNames argEnumName, ResolutionsTypes argrestype)
    {
        this._id = argid;
        this._myEnumName = argEnumName;
        Convert_EnumName_ToName();
        this._restype = argrestype;
        _resolutionStr = _restype.ToString().Remove(0, 1);

        GenerateIntegerDims(_resolutionStr);
    }

    void GenerateIntegerDims(string argResx) {
        string[] words = argResx.Split(delimiterChars);
        if (words.Length != 2) {
            return;
        }

        _width=Int32.Parse(words[0]);
        _hight= Int32.Parse(words[1]);
    }
    public string getmynameandreso() {

        return _name + " " + _resolutionStr;
    }

    public override string ToString()
    {
        return "Dim: " + _id + " " + _name + " w." + _width + " h."+ _hight+ "  resstr" +_resolutionStr;
    }


    void Convert_EnumName_ToName() {

        // X  -
        // x  ,
        // _ " "

        //Q (
        //Z )

        string inputstr = _myEnumName.ToString();
        inputstr = inputstr.Replace('X', '-');
        inputstr = inputstr.Replace('x', ',');
        inputstr = inputstr.Replace('_', ' ');
        inputstr = inputstr.Replace('Q', '(');
        inputstr = inputstr.Replace('Z', ')');

        _name = inputstr;
    }


  
}
