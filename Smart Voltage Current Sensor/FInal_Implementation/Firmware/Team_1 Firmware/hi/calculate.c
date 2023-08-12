/*
 * calculate.c
 *
 * Created: 30/09/2022 12:26:16 am
 *  Author: Team 1
 */ 



#include <stdint.h>
#include <stdbool.h> // for float
#include <avr/io.h>
#include "calculate.h"
#include "timer0.h"


#define VOLTAGE_KP 1		//Kp value used in PI controller to control Vout
#define VOLTAGE_KI 3000     //Ki value used in PI controller to control Vout
#define T_SAMPLE 1/30000    //Sample time of the PI controller execution, measured using proteus
#define PI_LIMIT 6784		//The +/- saturation limits of the PI controller, important for overshoots 


static int16_t Int_out;	//ranges from -PI_LIMIT to +PI_LIMIT
static int16_t PI_out;	//ranges from -PI_LIMIT to +PI_LIMIT
int16_t Prop_out;		//ranges from -642 to 642
int16_t V_err;			//ranges from -642 to 642




int16_t sat_limit_pi(int16_t result){
	if (result > PI_LIMIT){
		return PI_LIMIT;                //If result greater than upper limit return upper limit
	}
	else if (result < -PI_LIMIT){
		return -PI_LIMIT;                //If result smaller than lower limit return lower limit
	}
	else
	return result;
}



void pi_controller(uint16_t V_out, uint16_t V_ref){
	
	V_err = - ( V_ref - V_out );				//Calculating the error voltage
	
											//Rate limiter
	if(V_err > 642){						//value calculated so bit shift of V_err doesn't overflow
		V_err = 642;
	}else if(V_err < -642){
		V_err = -642;
	}
	
	Prop_out = V_err * VOLTAGE_KP;								//Calculating the proportional term													
									
	Int_out = sat_limit_pi(Int_out + ((V_err*51)>>9));			//Calculating the integral term and limiting to within +/-PI_LIMIT
																//bit shift corresponds to *0.1 derived from KI*T_SAMPLE
	PI_out = sat_limit_pi(Prop_out + Int_out);                  //Calculating the PI_out and limiting to within +/-PI_LIMIT 
	
	set_timer0_compare(PI_out);									//mapping the PI_out and setting OCR0B to this value
	
}




