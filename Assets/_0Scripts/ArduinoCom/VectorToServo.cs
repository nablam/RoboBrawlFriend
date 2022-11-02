using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class VectorToServo
{


	//                                
	//       |\ .                        
	//       | \   \       Br               
	//       |  \      \                   
	//       |   \         \                  
	//       |    \            \              
	//       |     \              o           
	//     E |      \ Cr         /             
	//       |       \          /              
	//       |        \        /  Ar            
	//       |         \      /                 
	//       |          \    /                  
	//       |________Ir_\Jr/ Sr               
	//             Dr                       






	//            Ymax
	//            |\
	//            | \
	//            |  \
	//            |   \   Br
	//            |    \
	//            |     \
	//            |      \
	//            |       \
	//            |        O
	//            |          \
	//            |           \  Ar
	//            |            \          Ymid _____________________o
	//            |             \             |                     /
	//            |              \            |                  /      Ar
	//            |_______________\           |_______________/
	//                     Dr                        Dr  
	//




	//
	//							   Bl			 xxxxxx|
	//										  xx       |\
	//									xxxxx          | |
	//							   o                   | |
	//				  Al     	xxx                    |  |     Br
	//						xxx                        |   |
	//				/xxxx                              |   \
	//	ANGlimit  <__________|___________|_____________|____\           _ANGlimit_ reffers to the hard servo ange limit before snapping the pla . 
	//					Dl           Dr         Ar		                Use _anglimt_ or 180-_ANGlimit_ depending on dervo L or R 





	//                                          
	//                                               
	//                                                        
	//                               x          |              x               _
	//                               x          |             x                 |
	//                               xx         |            xx                 |   A
	//                                xx        |            x                  |  
	//                                 xxxxxxxxxxxxxxxxxx   x                   |
	//                              xxxxxxx     |       x xxxx                  |       U=A+D
	//                          xxxx      xxx   |      xxx  | xxx              _|
	//                       xxx             xxxxxxxxxxx    |    xxx            |   D
	//                     xx                   |           |      xx           |
	//                    xx                    |           |        xx         |
	//             -----------------------------|-----------|-------------------------
	//                  xx                      |         ymid           x      |
	//                  x                       |                        x      |  D  
	//                  x                       |                        x     _|  
	//                                                                  
	//                                          |_________________________|                       
	//                                               G= A+B
	//                                          
	//                                          


	float Dr, Dl, E;

	float Ar, Al, Br, Bl, Cr, Cl;
	float Ir, Il, Jr, Jl, Sr, Sl;

	float _x_, _y_;
	float _A_, _B_, _D_, _G_, _U_, _ANGlimit_, _Ymid_, _Ymin_, _Ymax_;


	public VectorToServo() {
		InitialSetup();
	//	PrintKinematicValues();
		PrintLimitValues();
	}

	float Find_Ymax(float argD, float argA, float argB) {
		return Mathf.Sin(Mathf.Acos(argD / (argB + argA))) * (argA + argB);
	}
	float Find_Ymin(float argD, float argA, float argB)
	{
		return Mathf.Sin(Mathf.Acos((argB - argD) / argA)) * (argA);
	}

	float Find_AngLimit(float argD, float argA, float argB)
	{
		float a = argA + argB;  //32 +48 = 80
		float b = argD + argD + argA; // 20 +20 +32 = 72
		float c = argB; // 48
		float temp_CosTheta = ((a * a) + (b * b) - (c * c)) / (2 * a * b);
		return Mathf.Acos(temp_CosTheta);
	}

	float Find_Ymid(float argD, float argA, float argB)
	{
		float temp_G = argA + argB;
		float temp_U = argA + argD;
		return ((argB * argB) - (temp_G * temp_G) + (argD * argD) - (temp_U * temp_U)) / ((-2 * argD) - (2 * temp_U));
	}

	float FiletrX(float arg_x, float arg_y, float arg_Ymid, float argD, float argA, float argB) {
		if (arg_y < arg_Ymid) return FilterX_LowerMid(arg_x, arg_y, argD, argA, argB);
		return FilterX_UpperMid(arg_x, arg_y, argD, argA, argB);

	}
	float FilterX_LowerMid(float arg_x, float arg_Y, float argD, float argA, float argB) {
		float temp_G = argA + argB;
		// given the Y value, check the max x 
		float tempMaxXforGivenY = Mathf.Sqrt((temp_G * temp_G) - (arg_Y * arg_Y)) - argD;
		return tempMaxXforGivenY;
	}

	float FilterX_UpperMid(float arg_x, float arg_Y, float argD, float argA, float argB)
	{
		float temp_U = argA + argD;
		// given the Y value, check the max x 
		float tempBsquare = (argB * argB);
		float tempXsquare = (arg_Y * arg_Y);

		float tempMaxXforGivenY =-1* Mathf.Sqrt(tempBsquare - tempXsquare) - temp_U;
		return tempMaxXforGivenY;
	}

	void InitialSetup() {
		_x_ = 0;_y_ = 0;
		_A_ = 32f;_B_ = 48f; _D_ = 20f;
		_G_ = _A_ + _B_;
		_U_ = _A_ + _D_;
		_Ymax_ = Find_Ymax(_D_, _A_, _B_);
		_Ymin_ = Find_Ymin(_D_, _A_, _B_);
		_ANGlimit_= Find_AngLimit(_D_, _A_, _B_);
		_Ymid_= Find_Ymid(_D_, _A_, _B_);
		Set_Hand_Neutral();
	}

	void Set_Hand_Neutral() {

		Dr = Dl = _D_;
		E = _Ymid_;
		Ar =Al = _A_;
		Br = Bl = _B_;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));
		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;
		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;
		Sr = 180 - Ir - Jr;
		Sl = Il + Jl;
	}

	void PrintKinematicValues() {

		
		string TempCr = Cr.ToString("F6", CultureInfo.InvariantCulture);
		string TempCl = Cl.ToString("F6", CultureInfo.InvariantCulture);
		string temp_STR_CRCL= "Cr= " + TempCr + "  Cl= " + TempCl;

		string TempIr = Ir.ToString("F6", CultureInfo.InvariantCulture);
		string TempIl = Il.ToString("F6", CultureInfo.InvariantCulture);
		string temp_STR_IrIl = "Ir= " + TempIr + "  Il= " + TempIl;

		string Tempjr = Jr.ToString("F6", CultureInfo.InvariantCulture);
		string Tempjl = Jl.ToString("F6", CultureInfo.InvariantCulture);
		string temp_STR_jrjl= "jr= " + Tempjr + "  jl= " + Tempjl;

		string TempSr = Sr.ToString("F6", CultureInfo.InvariantCulture);
		string TempSl = Sl.ToString("F6", CultureInfo.InvariantCulture);
		string temp_STR_REVROANG = "Sr= " + TempSr + "  Sl= " + TempSl;

		Debug.Log("**********************************************************");
		Debug.Log(temp_STR_CRCL +  "****"  + temp_STR_IrIl + "****" + temp_STR_jrjl);
		Debug.Log(  "**" + temp_STR_REVROANG + "**");
		Debug.Log("");
	}
	void PrintLimitValues() {
		string Temp_A_ = _A_.ToString("F6", CultureInfo.InvariantCulture);
		string Temp_B_ = _B_.ToString("F6", CultureInfo.InvariantCulture);
		string Temp_D_ = _D_.ToString("F6", CultureInfo.InvariantCulture);
		string Temap_ABD = "_A_= " + Temp_A_ + "  _B_= " + Temp_B_ + " _D_="+  Temp_D_;
		string Temp_G_ = _G_.ToString("F6", CultureInfo.InvariantCulture);
		string Temp_U_ = _U_.ToString("F6", CultureInfo.InvariantCulture);
		string Temp_ANGlimit_ = _ANGlimit_.ToString("F6", CultureInfo.InvariantCulture);
		string Temap_GUANG = "_G_= " + Temp_G_ + "  _U_= " + Temp_U_ + " _ANG_="+ Temp_ANGlimit_;
		string Temp_Ymin_ = _Ymin_.ToString("F6", CultureInfo.InvariantCulture);
		string Temp_Ymid_ = _Ymid_.ToString("F6", CultureInfo.InvariantCulture);
		string Temp_Ymax_ = _Ymax_.ToString("F6", CultureInfo.InvariantCulture);
		string Temap_Ynxd = "_Ymin_= " + Temp_Ymin_ + "  _Ymid_= " + Temp_Ymid_ + " _ymax_=" + Temp_Ymax_;

		Debug.Log("-----------------------------------------------------------");
		Debug.Log(Temap_ABD);
		Debug.Log(Temap_GUANG);
		Debug.Log(Temap_Ynxd);
		Debug.Log("");


	}

	public float GiveMeanXvalueFor(float argY) {

		return FiletrX(1000f, argY,_Ymid_, _D_, _A_, _B_);
	}
}
