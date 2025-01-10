/*
 * adc.c
 *
 * Created: 30/09/2022 12:24:08 am
 *  Author: Team 1
 */ 


#include <avr/io.h>
#include "adc.h"


//Initialize ADC to auto trigger with timer0 compare match

void adc_init(void){
	ADMUX = 1 << REFS0;																//AVCC set as reference, ADC0 selected and results are right adjusted
	ADCSRA = (1 << ADEN) | (1 << ADSC) | (1 << ADATE) |(1 << ADIE) | (0b110 << ADPS0); //Enable ADC, start conversion, setup auto-trigger, enable interrupt and set pre-scaler to 64 for ADC clock of 250kHz
	ADCSRB = (0b110 << ADTS0);															//Use TC1 overflow (every 0.075ms) as auto- trigger source 
	DIDR0 = 1 << ADC0D;																	//ADC0 buffer disabled
}




