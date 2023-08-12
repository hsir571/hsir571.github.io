/*
 * GccApplication1.c
 *
 * Created: 22/08/2021 11:36:45 am
 * Author : logan
 */ 

#include "uart0.h"


#define F_CPU 800000UL
#include <avr/io.h>
#include <stdint.h>
#include <util/delay.h>
#include <stdio.h> 
#include <string.h> 
//Define values
#define UBRRVALUE 12
#define TIME_DELAY 1000
#define   PEAKVOLTAGE    14.5
#define   RMSCURRENT   125
#define   POWER         1.60

//Prototype functions
void extract_digits(uint8_t digits[], uint16_t data);
uint8_t convert_ascii(uint8_t data);
void transmitFullNumber(uint8_t numberArray[5]);
void uart_transmit_byte(char byte);
void uart_transmit_array(char* msg);


int main(void)
{	

	usart_init(UBRRVALUE); //Initialize UART


	while (1)
	{
		uint8_t digits[5]; //Initialize array
		uint8_t acsiiArray[5];
		
		uint16_t peakVoltage = (PEAKVOLTAGE * 100); //Multiply number into an integer
		extract_digits(digits, peakVoltage); //Call extract function to get an array of digits
		for (int i = 0; i < 5; i++){ //For every number in the array
			acsiiArray[i] = convert_ascii(digits[i]);//Convert to ascii
		}
		 
		uart_transmit_array("Peak Voltage = "); 
		transmitFullNumber(acsiiArray); //Transmit the whole number
		uart_transmit_array(" V");
		//Transmit new line
		usart_transmit('\r');
		usart_transmit('\n');
		
		uint16_t RMSCurrent = (RMSCURRENT * 100); //Multiply number into an integer
		extract_digits(digits, RMSCurrent); //Call extract function to get an array of digits
		for (int i = 0; i < 5; i++){ //For every number in the array
			acsiiArray[i] = convert_ascii(digits[i]);//Convert to ascii
		}
		uart_transmit_array("RMS Current = ");
		transmitFullNumber(acsiiArray);	//Transmit the whole number
		uart_transmit_array(" A");
		
		//Transmit new line
		usart_transmit('\r');
		usart_transmit('\n');
		
		uint16_t power = (POWER * 100); //Multiply number into an integer
		extract_digits(digits, power); //Call extract function to get an array of digits
		for (int i = 0; i < 5; i++){ //For every number in the array
			acsiiArray[i] = convert_ascii(digits[i]);//Convert to ascii
		}
		uart_transmit_array("Power = ");
		transmitFullNumber(acsiiArray); //Transmit the whole number
		uart_transmit_array(" W"); 
		//Transmit new line
		usart_transmit('\r');
		usart_transmit('\n');
		
		_delay_ms(TIME_DELAY); //Delay for one second
		
		}

}


uint8_t convert_ascii(uint8_t data){
	return (data + 48); // Convert number to acsii decimal representation
}

void extract_digits(uint8_t digits[5], uint16_t data){
	
	digits[0] = data / 10000; //Get 1st digit
	data = data % 10000; //Take only the lower 4 digits
	digits[1] = data / 1000; //Get 2nd digit
	data = data % 1000; //Take only the lower 3 digits
	digits[2] = data / 100; //Get 3rd digit
	data = data % 100; //Take only the lower 2 digits
	digits[3] = data / 10; //Get 4th digit
	digits[4] = data % 10; //Get 5th digit
	
	
	return;
}

void uart_transmit_array(char* msg){
	//Loop through each byte in the array
	for (uint8_t i = 0; i < strlen(msg); i++){
		uart_transmit_byte(msg[i]);                //Transmit each byte using uart_transmit_byte(char byte)
	}
}

void uart_transmit_byte(char byte){
	while ((UCSR0A & 0b00100000) == 0){       //UDRE0 bit is checked to see if it is 0
		;                                         //If UDRE0 bit is not 0, wait until it becomes 0
	}
	//Put the byte to be sent into the UDR0 register
	UDR0 = byte;
}