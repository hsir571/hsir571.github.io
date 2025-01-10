/*
 * main.c
 *
 * Created: 30/09/2022 12:21:22 am
 * Author : Team 1
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdint.h>
#include <string.h>

#include "adc.h"
#include "timer0.h"
#include "calculate.h"
#include "uart.h"

#define PIN_OUT(p) DDRB |= (1 << (p))
#define PIN_TOGGLE(p) PORTB ^= (1 << (p))

#define BUFFER_SIZE 8					
#define CONVERT_CONSTANT 3.367					//found by 500/1024 * 1/Gvs. Gvs = 0.145, adc value is 10bits and reference is 5V

uint16_t V_ref = 500;							//in E-02 units, initialized to 500 cause thats the minimum 
volatile uint16_t ADC_reading = 0;				//volatile cause inside interrupt, used to store 
uint16_t V_out = 0;								//calculate from ADC_reading outside of the interrupt

volatile char rx_buffer[BUFFER_SIZE];	
volatile char tx_buffer[BUFFER_SIZE];
volatile uint8_t rx_byte_pos = 0;
volatile uint8_t tx_byte_pos = 0;
volatile uint8_t rx_complete_flag = 1; 
volatile uint8_t tx_complete_flag = 0;


uint16_t received_voltage = 0;			//stores user inputted voltage as uint16 variable and returned to be used in calculations

uint8_t v1 = 0;							//stores each digit of the voltage to store in the transmit array 
uint8_t v2 = 0;
uint8_t v3 = 0;
uint8_t v4 = 0;

uint16_t slowdown_counter = 0;			//counter for slowing down the transmit 













ISR (ADC_vect){
	ADC_reading = ADC;					//reads ADC
	TIFR1 |= 1 << TOV1;					//reset timer1 overflow
	PIN_TOGGLE(PINB5);

}	


ISR(USART_RX_vect) {
	rx_buffer[rx_byte_pos] = UDR0;        //Store received character to next position of array
	rx_byte_pos++;                        //Increment buffer position
	rx_complete_flag = 0;                 //Indicate message not ready to read
	
	if(rx_buffer[rx_byte_pos-1] == '\r'){	//Check if a carriage return entered indicating end of message
		rx_complete_flag = 1;               //Indicate message ready to read
		rx_byte_pos = 0;                    //Reset position in array
	}
	else if(rx_byte_pos > (BUFFER_SIZE - 1)){
		rx_byte_pos = 0;                    //If buffer full discard message and reset buffer
	}
	
}



ISR(USART_TX_vect) {
	UDR0 = tx_buffer[tx_byte_pos];
	tx_buffer[tx_byte_pos] = '\0';	//once value is transmitted clear it so the transmit doesn't repeatable send the same value
	tx_byte_pos++;
	tx_complete_flag = 0;
	
	if(UDR0 == '\r') {
		tx_complete_flag = 1;
		tx_byte_pos = 0;
	}
	else if (tx_byte_pos > (BUFFER_SIZE-1)) {
		tx_byte_pos = 0; 
	}
	
}






void load_message(void) {
	

	
	tx_byte_pos = 0;
	tx_buffer[0] = 86;			//V
	tx_buffer[1] = 111;			//o
	tx_buffer[2] = 117;			//u
	tx_buffer[3] = 116;			//t
	tx_buffer[4] = 61;			//=
	tx_buffer[5] = '\r';

	while (tx_complete_flag != 1){
		;
	}

}






void load_voltage(uint16_t output) {
			
														
		v1 = output/1000;								
		v2 = (output - (v1*1000))/100; 
		v3 = (output - (v1*1000) - (v2*100))/10;	
		v4 = output - (v1*1000) - (v2*100) - (v3*10);



		tx_byte_pos = 0;		//force the byte_pos to 0 so the transmit properly sends in the right order
		tx_buffer[0] = 8;
		tx_buffer[1] = v1 + 48; //plus 48 for ascii
		tx_buffer[2] = v2 + 48;
		tx_buffer[3] = v3 + 48;
		tx_buffer[4] = v4 + 48;
		tx_buffer[5] = 48;		//to make output in mV
		tx_buffer[6] = '\n';
		tx_buffer[7] = '\r';
	
}




uint16_t receive (uint16_t safety){
	uint8_t i = 0;
	uint8_t value_buffer[4] = {0,0,0,0};	 //stores user inputted voltage temporarily in an array

	
	while (rx_buffer[i] != '\r'){
		
		if(rx_buffer[i] < 48){	//48 to 57 are the values for 0-9 in ASCII, if user input a non number then just return the old v_ref
			return safety;
		}else if (rx_buffer[i] > 57){
			return safety;
		}
		
		value_buffer[i] = rx_buffer[i] - 48;	//subtract 48 to extract number from ASCII
		i++;
		
	}
	
	
	if(i == 5){			//if the value more than 10V convert accordingly
		received_voltage = 1000*value_buffer[0] + 100*value_buffer[1] + 10*value_buffer[2] + value_buffer[3];
		
	}else if(i == 4){	//if the value is from 1-10V do this conversion instead
		received_voltage = 100*value_buffer[0] + 10*value_buffer[1] + value_buffer[2];
		
	}else{				//if the value is below 1V then return 5V
		return 500;							
	}
	

	
	if(received_voltage < 500){				//if the found value is less than 5V then return 5V
		return 500;
		
	}else if (received_voltage > 3000){		//same for if the value is above 30V then return 30V
		return 3000;
		
	}
	
	return received_voltage;				
	
}









int main(void) {

	timer0_init();				//Initializing timer0
	timer1_init();				//Initializing timer1
	adc_init();					//Initializing ADC with interrupts
	uart_init(9600);			//Initializing UART0 with interrupts
	sei();						//enable global interrupts

	PIN_OUT(PINB4);				//for testing
	PIN_OUT(PINB5);


	UDR0 = '\0';				//kick start the transmit

	
	while(1){
		
		PIN_TOGGLE(PINB4);

		while (rx_complete_flag != 1){//this code is for proteus
			V_ref = receive(V_ref);			//Read user input for reference voltage feed in current V_ref for when the input is invalid
		}
		
		
		
		V_out = ADC_reading * CONVERT_CONSTANT;		//convert from 10 bit ADC to a 5-30 Voltage 
		pi_controller(V_out, V_ref);				//Do the PI calculations and map to output PWM
		
		
		
		if (slowdown_counter > 1999){
			load_message();
			load_voltage(V_out);		//load the V_out value in the transmit buffer which will trigger the ISR and send 
			slowdown_counter = 0;		
			
		}
		
		slowdown_counter++;
		

	}









}