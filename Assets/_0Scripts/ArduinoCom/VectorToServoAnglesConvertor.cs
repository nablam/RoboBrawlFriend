using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class VectorToServoAnglesConvertor 
{
	float _servo_0_BL_default_ANG;
	float _servo_1_TL_default_ANG;

	float _servo_3_BR_default_ANG;
	float _servo_4_TR_default_ANG;

	float _caculated_neutral_svo_0;
	float _caculated_neutral_svo_1;
	float _caculated_neutral_svo_3;
	float _caculated_neutral_svo_4;

	//float _servo_0_BL_default_ANG_fl32;
	//float _servo_1_TL_default_ANG_fl32;

	//float _servo_3_BR_default_ANG_fl32;
	//float _servo_4_TR_default_ANG_fl32;

	//char _side;

	float x;
	float y;
	float midY; //for RightHandServos
	float Dr, Dl;
	float E;

	float Ar, Al, Br, Bl, Cr, Cl;
	float Ir, Il, Jr, Jl, Sr, Sl;

	float ANGLE;
	float angRads;
	float radius;
	string[] MessageArray;                      
	public VectorToServoAnglesConvertor(int arg_DefaultservoAAngle/*120*/, int arg_DefaultservoBAngle/*60*/, float arg_radius )
	{
		//hard set defaultvalues

		//_servo_1_TL_default_ANG = arg_DefaultservoAAngle; _servo_4_TR_default_ANG = arg_DefaultservoAAngle;
		//_servo_0_BL_default_ANG = arg_DefaultservoBAngle; _servo_3_BR_default_ANG = arg_DefaultservoBAngle;


		_servo_1_TL_default_ANG = 120; _servo_4_TR_default_ANG = 60;
		_servo_0_BL_default_ANG = 60; _servo_3_BR_default_ANG = 120;


		//_servo_1_TL_default_ANG_fl32 = _servo_1_TL_default_ANG; _servo_4_TR_default_ANG_fl32 = _servo_4_TR_default_ANG;
		//_servo_0_BL_default_ANG_fl32 = _servo_0_BL_default_ANG; _servo_3_BR_default_ANG_fl32 = _servo_3_BR_default_ANG;




		//_side = arg_side;
		MessageArray = new string[8];


		//                                
		//       |\ .                        
		//       | \   \       Br               
		//       |  \      \                   
		//       |   \         \                  
		//       |    \            \              
		//       |     \              /           
		//     E |      \ Cr         /             
		//       |       \          /              
		//       |        \        /  Ar            
		//       |         \      /                 
		//       |          \    /                  
		//       |________Ir_\Jr/ Sr               
		//             Dr                       
		radius = arg_radius;//was 10
		ANGLE = 90;
		angRads = ANGLE / 360 * 2 * Mathf.PI;

		x = 0;
		y = 0;

		x = Mathf.Cos(angRads) * radius;
		y = Mathf.Sin(angRads) * radius;

		//("S"+Percentage.ToString("D3")
		//var label = someNumber.ToString("F3", CultureInfo.InvariantCulture); // F3 is to get exactly 3 digits beyond a decimal point


		//Arduino was -> Serial.println("x= " + String(x, 6) + "  y=" + String(y, 6));
		string Tempx= x.ToString("F6", CultureInfo.InvariantCulture);
		string Tempy = y.ToString("F6", CultureInfo.InvariantCulture);
		Debug.Log("x= " + Tempx + "  y= " + Tempy);

		midY = 60; //for RightHandServos
		Dr =20f - x;
		Dl = 20f + x;
		E = midY + y;
		Ar = 32;
		Al = 32;
		Br = 48;
		Bl = 48;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));

		//Arduino was -> Serial.println("Cr= " + String(Cr, 6) + "  Cl=" + String(Cl, 6));
		string TempCr = Cr.ToString("F6", CultureInfo.InvariantCulture);
		string TempCl = Cl.ToString("F6", CultureInfo.InvariantCulture);
		Debug.Log("Cr= " + TempCr + "  Cl= " + TempCl);

		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;
		//Arduino was ->	Serial.println("Ir= " + String(Ir, 6) + "  Il=" + String(Il, 6));
		string TempIr = Ir.ToString("F6", CultureInfo.InvariantCulture);
		string TempIl = Il.ToString("F6", CultureInfo.InvariantCulture);
		Debug.Log("Ir= " + TempIr + "  Il= " + TempIl);

		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;
		//Arduino was ->	Serial.println("Jr= " + String(Jr, 6) + "  Jl=" + String(Jl, 6));
		string Tempjr = Jr.ToString("F6", CultureInfo.InvariantCulture);
		string Tempjl = Jl.ToString("F6", CultureInfo.InvariantCulture);
		Debug.Log("jr= " + Tempjr + "  jl= " + Tempjl);

		Sr = 180 - Ir - Jr;
		Sl = Il + Jl;

		//Arduino was ->	Serial.println("Sr= " + String(Sr, 6) + "  Sl=" + String(Sl, 6));
		string TempSr = Sr.ToString("F6", CultureInfo.InvariantCulture);
		string TempSl = Sl.ToString("F6", CultureInfo.InvariantCulture);



		  _caculated_neutral_svo_0= _caculated_neutral_svo_4 = Sr;
		_caculated_neutral_svo_1 = _caculated_neutral_svo_3 = Sl;
		Debug.Log("Sr= " + TempSr + "  Sl= " + TempSl);
	}

	//public int ServoTop_NeutralANG { get => _servo_1_TL_default_ANG; private set => _servo_1_TL_default_ANG = value; }
	//public int ServoBot_NeutralANG { get => _servo_0_BL_default_ANG; private set => _servo_0_BL_default_ANG = value; }
	////public char Side { get => _side; private set => _side = value; }
	//public int Servo_3_BR_default_ANG { get => _servo_3_BR_default_ANG; set => _servo_3_BR_default_ANG = value; }
	//public int Servo_4_TR_default_ANG { get => _servo_4_TR_default_ANG; set => _servo_4_TR_default_ANG = value; }

	public string  Set_messageStringForMyServos(float arg_Langle, bool arg_LsolenoidState, float arg_Rangle, bool arg_RsolenoidState, int arg_command_3_char, int arg_Debug, float arg_xofset_L, float arg_yofsetL, float arg_radiusL, float arg_xofset_R, float arg_yofsetR, float arg_radiusR) {

		if (arg_command_3_char == 911)
		{
			 
				Debug.Log("STAAAPH!!! use 60 120");
				Populate_0_1_2_3_4_5_default_tHand();
				Populate_6_7ComDebug(arg_command_3_char, arg_Debug);
			 


		} else if (arg_command_3_char == 311) { //raw value to selected servo

			int servoTotest = arg_Debug%4; //0 1 2 3
			int indexOfservototest = 0;
			float floatAngleToUse = 0;
			if (servoTotest == 0) {
				indexOfservototest = 0;
				floatAngleToUse = arg_Langle;
			}
			else
			 if (servoTotest == 1)
			{
				indexOfservototest = 1;
				floatAngleToUse = arg_Langle;
			}
			else
			if (servoTotest == 2)
			{
				indexOfservototest = 3;
				floatAngleToUse = arg_Rangle;
			}
			else
			if(servoTotest == 3) {
				indexOfservototest = 4;
				floatAngleToUse = arg_Rangle;
			}
 
			MessageArray[indexOfservototest] = floatAngleToUse.ToString("000");
		}
		else if (arg_command_3_char == 611)
		{ //raw value to selected HAND servocoordination


			Populate_0_1_2_LeftHand(arg_Langle, arg_LsolenoidState, arg_xofset_L, arg_yofsetL, arg_radiusL);
			Populate_3_4_5_RighttHand(arg_Rangle, arg_RsolenoidState, arg_xofset_R, arg_yofsetR, arg_radiusR);
			Populate_6_7ComDebug(arg_command_3_char, arg_Debug);

		}
		else
		{
			Populate_0_1_2_LeftHand(arg_Langle, arg_LsolenoidState, arg_xofset_L, arg_yofsetL, arg_radiusL);
			Populate_3_4_5_RighttHand(arg_Rangle, arg_RsolenoidState, arg_xofset_R, arg_yofsetR, arg_radiusR);
			Populate_6_7ComDebug(arg_command_3_char, arg_Debug);
		}

 

		return string.Concat(MessageArray);
	}

	void Populate_0_1_2_3_4_5_default_tHand(  )
	{
		if (false)
		{
			string temp_S0 = _servo_0_BL_default_ANG.ToString("000");
			string temp_S1 = _servo_1_TL_default_ANG.ToString("000");
			string temp_S3 = _servo_3_BR_default_ANG.ToString("000");
			string temp_S4 = _servo_4_TR_default_ANG.ToString("000");
			string temp_solenoid = "000";

			MessageArray[0] = temp_S0; MessageArray[4] = temp_S4;
			MessageArray[1] = temp_S1; MessageArray[3] = temp_S3;
			MessageArray[2] = temp_solenoid; MessageArray[5] = temp_solenoid;
		}
		else
		{
			string temp_S0 = _caculated_neutral_svo_0.ToString("000");
			string temp_S1 = _caculated_neutral_svo_1.ToString("000");
			string temp_S3 = _caculated_neutral_svo_3.ToString("000");
			string temp_S4 = _caculated_neutral_svo_4.ToString("000");
			string temp_solenoid = "000";

			MessageArray[0] = temp_S0; MessageArray[4] = temp_S4;
			MessageArray[1] = temp_S1; MessageArray[3] = temp_S3;
			MessageArray[2] = temp_solenoid; MessageArray[5] = temp_solenoid;


		}






	}


	void Populate_0_1_2_LeftHand(float arg_Langle, bool arg_LsolenoidState, float argx, float argy, float argradius) {
		ANGLE = arg_Langle;
		angRads = ANGLE / 360 * 2 * Mathf.PI;

		//x = 0;
		//y = 0;
		//x = Mathf.Cos(angRads) * radius;
		//y = Mathf.Sin(angRads) * radius;

		x = 0;
		y = 0;
		if (argradius < 1)
		{
			x = argx*-1;
			y = argy;

		}
		else
		{
			x = Mathf.Cos(angRads) * argradius;
			y = Mathf.Sin(angRads) * argradius;
		}

		midY = 60; //for RightHandServos
		Dr = 20f + x;
		Dl = 20f - x;
		E = midY + y;
		Ar = 32;
		Al = 32;
		Br = 48;
		Bl = 48;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));




		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;

		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;

		Sr = 180 - Ir - Jr;
		Sl = Il + Jl;


		//Arra_LB_LT_RB_RT[0] = int(Sr);
		//Arra_LB_LT_RB_RT[1] = int(Sl);
		string temp_S0 = Sr.ToString("000");
		string temp_S1 = Sl.ToString("000");
		string temp_solenoid = "000";
		if (arg_LsolenoidState == true)
		{
			temp_solenoid = "111";
		}
		MessageArray[0] = temp_S0;
		MessageArray[1] = temp_S1;
		MessageArray[2] = temp_solenoid;
	}

	void Populate_3_4_5_RighttHand(float arg_Rangle, bool arg_RsolenoidState, float argx, float argy, float argradius) {

		ANGLE = 360 - arg_Rangle;
		angRads = ANGLE / 360 * 2 * Mathf.PI;

		x = 0;
		y = 0;
		if (argradius < 1)
		{
			x = argx;
			y = argy;

		}
		else {
			x = Mathf.Cos(angRads) * argradius;
			y = Mathf.Sin(angRads) * argradius;
		}
		midY = 60; //for RightHandServos
		Dr = 20f - x;
		Dl = 20f + x;
		E = midY + y;
		Ar = 32;
		Al = 32;
		Br = 48;
		Bl = 48;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));




		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;

		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;

		Sr = 180 - Ir - Jr;
		Sl = Il + Jl;

		//Arra_LB_LT_RB_RT[2] = int(Sl);
		//Arra_LB_LT_RB_RT[3] = int(Sr);

		string temp_S3 = Sl.ToString("000");
		string temp_S4 = Sr.ToString("000");
		string temp_solenoid = "000";
		if (arg_RsolenoidState == true)
		{
			temp_solenoid = "111";
		}
		MessageArray[3] = temp_S3;
		MessageArray[4] = temp_S4;
		MessageArray[5] = temp_solenoid;
	}

	void Populate_6_7ComDebug(int arg_command_3_char, int arg_Debug) {

		if (arg_command_3_char > 999) {
			arg_command_3_char = 999;
		}
		if (arg_Debug > 999)
		{
			arg_Debug = 999;
		}
		string temp_com3char= arg_command_3_char.ToString("000");
		string temp_debug3char = arg_Debug.ToString("000");
		MessageArray[6] = temp_com3char;
		MessageArray[7] = temp_debug3char;
	}
}


/* 
 
	void populateServoangs(float argAng, float argRadius)
	{
		radius = argRadius;
		ANGLE = argAng;
		angRads = ANGLE / 360 * 2 * Mathf.PI;

		x = 0;
		y = 0;

		x = Mathf.Cos(angRads) * radius;
		y = Mathf.Sin(angRads) * radius;

		midY = 50; //for RightHandServos
		Dr = 19.5f + x;
		Dl = 19.5f - x;
		E = midY + y;
		Ar = 32;
		Al = 32;
		Br = 48;
		Bl = 48;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));




		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;

		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;

		Sr = 180 - Ir - Jr;
		Sl = Il + Jl;

		//Arra_LB_LT_RB_RT[2] = int(Sl);
		//Arra_LB_LT_RB_RT[3] = int(Sr);



		//***********************************




		radius = argRadius;
		ANGLE = 360 - argAng;
		angRads = ANGLE / 360 * 2 * Mathf.PI;

		x = 0;
		y = 0;

		x = Mathf.Cos(angRads) * radius;
		y = Mathf.Sin(angRads) * radius;

		midY = 60; //for RightHandServos
		Dr = 19.5f - x;
		Dl = 19.5f + x;
		E = midY + y;
		Ar = 32;
		Al = 32;
		Br = 48;
		Bl = 48;
		Cr = Mathf.Sqrt((E * E) + (Dr * Dr));
		Cl = Mathf.Sqrt((E * E) + (Dl * Dl));




		Ir = Mathf.Acos(Dr / Cr) / 2 / Mathf.PI * 360;
		Il = Mathf.Acos(Dl / Cl) / 2 / Mathf.PI * 360;

		Jr = Mathf.Acos(((Cr * Cr) + (Ar * Ar) - (Br * Br)) / (2 * Ar * Cr)) / 2 / Mathf.PI * 360;
		Jl = Mathf.Acos(((Cl * Cl) + (Al * Al) - (Bl * Bl)) / (2 * Al * Cl)) / 2 / Mathf.PI * 360;

		Sr = 180 - Ir - Jr;
		Sl = Il + Jl;

		//Arra_LB_LT_RB_RT[0] = int(Sr);
		//Arra_LB_LT_RB_RT[1] = int(Sl);



	}
 */