/*
 * timer0.c
 *
 * Created: 11/10/2021 2:24:42 PM
 *  Author: Anthony
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>

//Header files
#include "uart0.h"
#include "adc.h"
#include "timer0.h"
#include "display.h"
#include "timer2.h"

ISR (TIMER0_OVF_vect) {
	;
}

//Initialise the timer to overflow every 0.1ms to auto trigger the ADC
void timer0_init() {
	
	//Fast PWM with OCR0A as the top
	TCCR0A = 0x00;
	TCCR0A |= (1 << WGM00);
	TCCR0A |= (1 << WGM01);
	TCCR0A |= (1 << WGM02);
	
	//Use a prescaler of 8
	TCCR0B = 0x00;
	TCCR0B |= (1 << CS01);
	
	//Timer top value to get a 0.1 ms delay
	OCR0A = 24;
		
	
}