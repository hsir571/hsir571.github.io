/*
 * adc.c
 *
 * Created: 11/10/2021 11:55:40 AM
 *  Author: Anthony
 */ 

#include <avr/io.h>
#include <stdint.h>
#include <avr/interrupt.h>

//Header files
#include "uart0.h"
#include "adc.h"
#include "timer0.h"
#include "display.h"
#include "timer2.h"

//User macros
#define SAMPLE_SIZE 80

//Declare global variables from main
extern uint16_t voltage_adc_value[2*SAMPLE_SIZE];
extern uint16_t current_adc_value[2*SAMPLE_SIZE];

//Function in main called from interrupt
extern void calculate_missing_data();

//Interrupt service routine - Occurs on Timer/Counter1 overflow
ISR (ADC_vect) {
	
	//Variable to keep track of the samples
	static uint8_t sample_count = 0;
	
	if (sample_count < SAMPLE_SIZE * 2) {
		
		//Perform ADC conversion, alternating between voltage and current readings
		if (sample_count % 2) {
			
			//Current reading
			ADMUX &= ~(1 << MUX0);
			ADCSRA |= (1 << ADSC);
			current_adc_value[sample_count] = (ADCL << 0) | (ADCH << 8);
			
		} else {
			
			//Voltage reading
			ADMUX |= (1 << MUX0);
			ADCSRA |= (1 << ADSC);
			voltage_adc_value[sample_count] = (ADCL << 0) | (ADCH << 8);
			
		}
		
		//clear Timer0 overflow flag
		TIFR0 |= (1<<TOV0);
		
		sample_count++;
		
	} else {
		
		//Disable ADC
		ADCSRA &= ~(1 << ADATE);
		
		//Disable interrupt
		ADCSRA &= ~(1 << ADIE);
		
		//Reset sample_count for next set of data
		sample_count = 0;
		
		calculate_missing_data();
		
	}
	
}

void adc_init() {
	
	//AVCC set as reference, ADC0 selected initially and results are right adjusted
	ADMUX = 0x00;
	ADMUX |= (1 << REFS0);
	
	//ADC operates in auto trigger mode
	ADCSRA = 0x00;
	ADCSRA |= (1 << ADEN);
	ADCSRA |= (1 << ADSC);
	ADCSRA |= (1 << ADPS0);  //Prescaler = 8
	ADCSRA |= (1 << ADPS1);  //Prescaler = 8
	
	//Auto trigger source is Timer/Counter0 overflow
	ADCSRB = 0x00;
	ADCSRB |= (1 << ADTS2);
}

void adc_enable () {
	
	ADCSRA |= (1 << ADATE);  //Auto trigger
	
	ADCSRA &= ~(1 << ADIF);
	
	//Enable interrupt
	ADCSRA |= (1 << ADIE);
	sei();
	
	ADCSRA |= (1 << ADSC);
}