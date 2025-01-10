/*
 * display.c
 *
 * Created: 3/10/2021 4:20:20 PM
 *  Author: Anthony
 */ 

#include <avr/io.h>
#include <stdint.h>

//Header files
#include "uart0.h"
#include "adc.h"
#include "timer0.h"
#include "display.h"
#include "timer2.h"

//Hardware macros
#define  FIRST_DIGIT_ENABLE PORTD &= ~(1 << PD4)
#define  FIRST_DIGIT_DISABLE PORTD |= (1 << PD4)
#define  SECOND_DIGIT_ENABLE PORTD &= ~(1 << PD5)
#define  SECOND_DIGIT_DISABLE PORTD |= (1 << PD5)
#define  THIRD_DIGIT_ENABLE PORTB &= ~(1 << PB4)
#define  THIRD_DIGIT_DISABLE PORTB |= (1 << PB4)
#define  FOURTH_DIGIT_ENABLE PORTD &= ~(1 << PD7)
#define  FOURTH_DIGIT_DISABLE PORTD |= (1 << PD7)

//Array containing which segments to turn on to display a number between 0 to 9
const uint8_t digit_patterns[10] = {0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07, 0x7F, 0x6F};

//Array containing which segments to turn on to display a unit. 
//First element corresponds to volts, second corresponds to amps and third corresponds to watts.
const uint8_t unit_patterns[3] = {0x1C, 0x77, 0x73};
	
//4 characters to be displayed on Ds1 to Ds4
static volatile uint8_t disp_characters[4] = {0,0,0,0};
	
//The current digit (e.g. the 1's, the 10's) of the 4-digit number we're displaying
static volatile uint8_t disp_position = 0;

void display_init(void){
	
	//Configure the shift register pins as outputs
	DDRC |= (1 << PC3) | (1 << PC4) | (1 << PC5);
	
	//Configure the digit enable/disable pins as outputs
	DDRD |= (1 << PD4) | (1 << PD5) | (1 << PD7);
	DDRB |= (1 << PB4);
	
}

//Populate the array ‘disp_characters[]’ by separating the four digits of ‘number’ and then looking up the segment pattern from ‘seg_pattern[]’
void seperate_and_load_characters(uint16_t number, uint8_t decimal_pos, uint8_t unit) {
	
	for (uint8_t i = 1; i < 4; i++) {
		
		//Separate each digit and assign corresponding digit patterns
		uint8_t digit;
		digit = number % 10;
		number = number / 10;
		disp_characters[i] = digit_patterns[digit];
		
		//Place a decimal point on the digit corresponding to decimal_pos
		if (i == decimal_pos) {
			disp_characters[i] = disp_characters[i] | 0b10000000;
		}
	}
	
	//Assign pattern to the unit of the quantity we want to display
	disp_characters[0] = unit_patterns[unit];

}

//Render a single digit from ‘disp_characters[]’ on the display at ‘disp_position’
void send_next_character_to_display(void){
	
	uint8_t digit_to_send = disp_characters[disp_position];
	
	//Ensure SH_CP and SH_ST are both set to logic 0
	PORTC &= ~(1 << PC3);
	PORTC &= ~(1 << PC5);

	for (int8_t i = 7; i > -1; i--) {
			
		//Extract a bit
		uint8_t bit = digit_to_send >> i;
		bit = bit & 0b00000001;
			
			//Send to shift register
			if (bit) {
				PORTC |= (1 << PC4);
				} else {
				PORTC &= ~(1 << PC4);
			}
			
			//Shift in the bit
			PORTC |= (1 << PC3);
			PORTC &= ~(1 << PC3);
			
		}
		
		//Disable all digits
		FIRST_DIGIT_DISABLE;
		SECOND_DIGIT_DISABLE;
		THIRD_DIGIT_DISABLE;
		FOURTH_DIGIT_DISABLE;
		
		//Latch the output
		PORTC |= (1 << PC5);
		PORTC &= ~(1 << PC5);
		
		//Enable the digit that we want to display
		if (disp_position == 0) {
			FOURTH_DIGIT_ENABLE;
		} else if (disp_position == 1) {
			THIRD_DIGIT_ENABLE;
		} else if (disp_position == 2) {
			SECOND_DIGIT_ENABLE;
		} else {
			FIRST_DIGIT_ENABLE;
		}
		
		disp_position++;
		
		if (disp_position > 3) {
			disp_position = 0;
		}
		
}

