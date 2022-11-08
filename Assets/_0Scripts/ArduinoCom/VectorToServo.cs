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

	//                     120                      60
	//  ┌──────────────────┐   x                xx    ┌──────────────────┐ 
	//  │                  │xxx                  xxx  │                  │
	//  │      SL         xxx                       xxx    SR            │ 
	//  │              xx│                          xx    [4]TR          │ 
	//  │    [1]TL         │                          │                  │ 
	//  │                  │                          │                  │ 
	//  └──────────────────┘                          └──────────────────┘ 
	//      
	//                      [2]LP            [5]LR
	//
	//  ┌──────────────────┐                          ┌──────────────────┐ 
	//  │                  │                          │                  │ 
	//  │                  │                          │xx   SL           │ 
	//  │      SR       xx │                          xx    [3]BR        │ 
	//  │  [0]BL         xx│                         xx                  │ 
	//  └─────────────────xx                       xxx───────────────────┘ 
	//                     xxx                    xx                       
	//                  60    xx                   120                       the hand side doesnt mater  always SR= 60 SL =120

	e_HandSide _HandSide;
	SvosBiAng _svosBiang;


	//--------------------------------------------------------
	ThunbRelPos _CenterPos;
	//--------------------------------------------------------
	//                                                        
	//     O                             
	//     |                                                  
	//  Dl |          ---                                     
	//     |        /     \                                    
	//     |───────────|──────────                            
	//     |        \ _|_ /                                   
	//  Dr |           |                                      
	//     |           └─────── _Ymid   is the X value        
	//     O                            for _Centerpoint
	//                                  Radius =10 
	//                                                        

	public VectorToServo(e_HandSide argSide)
	{
		_HandSide = argSide;
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

		if (arg_y < _Ymin_) { arg_y = _Ymin_; }
		if (arg_y > _Ymax_) { arg_y = _Ymax_; }

		if (arg_y < arg_Ymid) return FilterX_LowerMid(arg_x, arg_y, argD, argA, argB);
		return FilterX_UpperMid(arg_x, arg_y, argD, argA, argB);

	}
	float FilterX_UpperMid(float arg_x, float arg_Y, float argD, float argA, float argB) {
		float temp_G = argA + argB;
		// given the Y value, check the max x 
		float tempMaxXforGivenY = Mathf.Sqrt((temp_G * temp_G) - (arg_Y * arg_Y)) - argD;
		if (tempMaxXforGivenY < 0.5) tempMaxXforGivenY = 0;
		return tempMaxXforGivenY;
	}

	float FilterX_LowerMid(float arg_x, float arg_Y, float argD, float argA, float argB)
	{
		float temp_U = argA + argD;
		// given the Y value, check the max x 
		float tempBsquare = (argB * argB);
		float tempXsquare = (arg_Y * arg_Y);

		float tempMaxXforGivenY =-1* Mathf.Sqrt(tempBsquare - tempXsquare) + temp_U;
		return tempMaxXforGivenY;
	}

	//       |y=0                        
	//       |______________________________Ymax                   
	//       |                         
	//       |                           
	//       |                             
	//       |                                
	//       |  _______    Br   _______________Ymid              
	//       |\        \________               
	//     E |  \   Cr          \___           
	//       |    \                 o          
	//       |      \             /              
	//       |       \          /   Ar             
	//       |         \      /         _______Ymin
	//       |        Ir_\  / Sr               
	//       |____________Jr_________________  x=0                      
	//             Dr
	void InitialSetup() {
		_x_ = 0;_y_ = 0;
		_A_ = 32f;_B_ = 48f; _D_ = 20f;
		_G_ = _A_ + _B_;
		_U_ = _A_ + _D_;
		_Ymax_ = Find_Ymax(_D_, _A_, _B_);
		_Ymin_ = Find_Ymin(_D_, _A_, _B_);
		_ANGlimit_= Find_AngLimit(_D_, _A_, _B_);
		_Ymid_= Find_Ymid(_D_, _A_, _B_);

		_svosBiang = new SvosBiAng(); //a new one is made using 60 120 
		_CenterPos = new ThunbRelPos(_Ymid_, 0,10f, e_ButtonLocationType.Center);
		Set_Hand_Neutral();
	}
	void Populate_0_1_2_LeftHand(float arg_Langle, bool arg_LsolenoidState)
	{
		//ANGLE = arg_Langle;
		//angRads = ANGLE / 360 * 2 * Mathf.PI;
		//x = 0;
		//y = 0;
		//x = Mathf.Cos(angRads) * radius;
		//y = Mathf.Sin(angRads) * radius;
		//midY = 60; //for RightHandServos
		//Dr = 20f + x;
		//Dl = 20f - x;
		//E = midY + y;
		//Ar = 32;
		//Al = 32;
		//Br = 48;
		//Bl = 48;
		//Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		//Cl = Mathf.Sqrt((E * E) + (Dl * Dl));
		//Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		//Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;
		//Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		//Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;
		//Sr = 180 - Ir - Jr;
		//Sl = Il + Jl;
		////Arra_LB_LT_RB_RT[0] = int(Sr);
		////Arra_LB_LT_RB_RT[1] = int(Sl);
		//string temp_S0 = Sr.ToString("000");
		//string temp_S1 = Sl.ToString("000");
		//string temp_solenoid = "000";
		//if (arg_LsolenoidState == true)
		//{
		//	temp_solenoid = "111";
		//}
		//MessageArray[0] = temp_S0;
		//MessageArray[1] = temp_S1;
		//MessageArray[2] = temp_solenoid;
	}

	void Populate_3_4_5_RighttHand(float arg_Rangle, bool arg_RsolenoidState)
	{
		//ANGLE = 360 - arg_Rangle;
		//angRads = ANGLE / 360 * 2 * Mathf.PI;
		//x = 0;
		//y = 0;
		//x = Mathf.Cos(angRads) * radius;
		//y = Mathf.Sin(angRads) * radius;
		//midY = 60; //for RightHandServos
		//Dr = 20f - x;
		//Dl = 20f + x;
		//E = midY + y;
		//Ar = 32;
		//Al = 32;
		//Br = 48;
		//Bl = 48;
		//Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		//Cl = Mathf.Sqrt((E * E) + (Dl * Dl));
		//Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		//Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;
		//Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		//Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;
		//Sr = 180 - Ir - Jr;
		//Sl = Il + Jl;
		////Arra_LB_LT_RB_RT[2] = int(Sl);
		////Arra_LB_LT_RB_RT[3] = int(Sr);
		//string temp_S3 = Sl.ToString("000");
		//string temp_S4 = Sr.ToString("000");
		//string temp_solenoid = "000";
		//if (arg_RsolenoidState == true)
		//{
		//	temp_solenoid = "111";
		//}
		//MessageArray[3] = temp_S3;
		//MessageArray[4] = temp_S4;
		//MessageArray[5] = temp_solenoid;
	}
	void Populate_6_7ComDebug(int arg_command_3_char, int arg_Debug)
	{
		//if (arg_command_3_char > 999)
		//{
		//	arg_command_3_char = 999;
		//}
		//if (arg_Debug > 999)
		//{
		//	arg_Debug = 999;
		//}
		//string temp_com3char = arg_command_3_char.ToString("000");
		//string temp_debug3char = arg_Debug.ToString("000");
		//MessageArray[6] = temp_com3char;
		//MessageArray[7] = temp_debug3char;
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
		_svosBiang.SR = Sr = 180 - Ir - Jr;
		_svosBiang.SL = Sl = Il + Jl;		 
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

	

	public ThunbRelPos get_CalculatedCenterPos()
	{
		return this._CenterPos;
	}

	//positive x on

	//     O<- svoAxis  LeftHand                                                              O<- svoAxis Righthand
	//     |                                                                                  |
	//  Dl |                                                                                  | Dr
	//     |                                                                                  |
	//     |─── ─── ─── ───┐                                                   ┌── ─── ─── ───|
	//     |               |                                                   |              |
	//  Dr |                                                                                  | Dl
	//     |               I                                                   I              |
	//     O                                                                                  O
	//             Input x=40   (c1) y= -10                          Input x=40   y= -10  
	//
	//             local X=  10    Y= 40                              local Y= 40  X= -10
	//

	// arg_x_0_based_Axis   Is a POSITIVE value , 
	// X=0 for left hand is at the left o----o                  X=0 for Right hand is at the Right most  o----o      

	public SvosBiAng Convert_XY_TO_SvoBiAngs(float arg_x_0_based_Axis, float arg_y) {


		float LocalY = 0f;
		float LocalX_ABS = 0f;
		float LocalX = 0f;
		float SignChanger = 1f;

		LocalY = arg_x_0_based_Axis; // 

		if (_HandSide == e_HandSide.LEFT_hand)
		{
			LocalX = arg_y * -1f;  //case c1
		}
		else
		{
			LocalX = arg_y;
		}
		 

		if (LocalX < 0) SignChanger = -1f;

		LocalX_ABS = Mathf.Abs(LocalX);

		float filtered_x = FiletrX(1000f, LocalY, _Ymid_, _D_, _A_, _B_); //GiveMeanXvalueFor(LocalY); //is a max positix


		if (LocalX_ABS > filtered_x) {
			LocalX = filtered_x * SignChanger;
		}


		if (_HandSide == e_HandSide.LEFT_hand){
			Dr = _D_ + LocalX; Dl = _D_ - LocalX;
		}
		else{
			Dr = _D_ - LocalX; Dl = _D_ + LocalX;
		}

		E = LocalY;

		Ar = Al = _A_;
		Br = Bl = _B_;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));
		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;
		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;
		_svosBiang.SR = Sr = 180 - Ir - Jr;
		_svosBiang.SL = Sl = Il + Jl;

		return this._svosBiang;
	}


	public SvosBiAng Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(Vector3 arg_Direction) {


		 

		return Convert_XY_TO_SvoBiAngs(arg_Direction.x, arg_Direction.y);
	}
}
