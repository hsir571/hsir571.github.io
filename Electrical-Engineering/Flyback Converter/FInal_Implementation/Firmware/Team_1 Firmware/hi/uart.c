/*
 * uart.c
 *
 * Created: 10/10/2022 1:47:22 PM
 *  Author: Team 1
 */ 

#include <stdint.h>
#include <avr/io.h>

#include "uart.h"


void uart_init(uint16_t BAUD_RATE) {
	
	UCSR0B = (1<<RXCIE0) | (1<<TXCIE0) | (1<<TXEN0) | (1<<RXEN0);	//Enable Transmit/Receive and Transmit/Receive Interrupts
	UCSR0C =  (1<<UCSZ01) | (1<<UCSZ00);							//8N1 no parity, we are only using 7 data bits
	UBRR0 = (1000000 /BAUD_RATE) - 1;								//Set UBRR0 with baud rate 

}