/*
 * timer2.c
 *
 * Created: 16/10/2021 12:43:20 PM
 *  Author: Anthony
 */ 

//Header files
#include "uart0.h"
#include "adc.h"
#include "timer0.h"
#include "display.h"
#include "timer2.h"

#include <avr/io.h>
#include <avr/interrupt.h>

extern float peak_voltage;
extern float current_rms;
extern float power;

volatile uint16_t overflow_count = 0;

ISR(TIMER2_COMPA_vect) {
	
	send_next_character_to_display();

	//Output via seven segment display
	if (overflow_count == 0) {
		seperate_and_load_characters((uint16_t)(10 *peak_voltage), 2, 0);
	} else if (overflow_count == 100) {
		seperate_and_load_characters((uint16_t)(100 *current_rms), 3, 1);
	} else if (overflow_count == 200) {
		seperate_and_load_characters((uint16_t)(100 *power), 3, 2);
	}
	
	overflow_count++;
	
	if (overflow_count >= 300) {
		overflow_count = 0;
	}

}



void timer2_init() {

	//Select CTC mode with OCR2A as the top
	TCCR2A = 0x00;
	TCCR2A |= (1 << WGM21);
	
	//Use a prescaler of 256
	TCCR2B = 0x00;
	TCCR2B |= (1 << CS21);
	TCCR2B |= (1 << CS22);

	//Timer top value to get a 10ms delay
	OCR2A = 78;
	
	//Interrupt enable
	TIMSK2 |= (1 << OCIE2A);

}
