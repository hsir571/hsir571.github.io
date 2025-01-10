/*
 * uart0.c
 *
 * Created: 30/08/2021 5:36:57 pm
 *  Author: logan
 */ 



#include "uart0.h"

#include <avr/io.h>
#include <stdint.h>

void usart_init(uint16_t ubrr){
	
	UCSR0B |= (1 << TXEN0); //Turn transmit function on
	
	UCSR0C = 0x06; // Set transmit character length
	
	UBRR0 = 4;
	
	return;
}

void usart_transmit(uint8_t data){
	
	while (( UCSR0A & (1<<UDRE0)) == 0) {}; // Wait until transmit register is empty
	
	UDR0 = data; // Put transmitted data into register
	
	return;
}

void transmitFullNumber(uint8_t acsiiArray[5]){
	
	for(int i = 0; i < 3; i++){ //For three integer digits
		usart_transmit(acsiiArray[i]); //Transmit digit
	}
	
	usart_transmit(46); //Transmit decimal point
	
	for(int i = 3; i < 5; i++){ //For two decimal places
		usart_transmit(acsiiArray[i]); //Transmit digit
	}

}