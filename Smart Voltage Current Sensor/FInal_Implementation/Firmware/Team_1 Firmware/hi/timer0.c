/*
 * timer0.c
 *
 * Created: 30/09/2022 12:30:50 am
 *  Author: Team 1
 */ 

#include <stdint.h>
#include <avr/io.h>
#include "timer0.h"

#define PI_LIMIT 6784		//not used here but for information


void timer0_init(void){
	TCCR0A = 2<<COM0B0 | 3<<WGM00;		//Fast PWM mode & Clear on Compare Match with OCR0B 
	TCCR0B = 1<<WGM02 | 1<<CS00;		//Pre-scaler 1
	OCR0A = 159;						//Loading OCR0A with 159 to setup a 100kHz PWM
	DDRD = 1<<PIND5;					//PD5 setup as an output pin of the PWM

}



void timer1_init(void){
	TCCR1A = (1<<WGM11) | (1<<WGM10);					//Fast PWM mode 
	TCCR1B = (1<<WGM13) | (1<<WGM12) | (1<<CS11);       //Pre-scaler 8
	OCR1A = 149;										//Setup a 0.075ms PWM, will not go below this. minimum acquisition + conversion time
	
}



void set_timer0_compare(int16_t unmapped){
	OCR0B = ((unmapped * 3)>>8) + 80;   //converting PI_out results to a value between 0 and 159 to map to the output PWM
	
}
