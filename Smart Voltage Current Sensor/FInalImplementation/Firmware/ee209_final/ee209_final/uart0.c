/*
 * uart0.c
 *
 * Created: 11/10/2021 11:28:12 AM
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

void uart_init(uint16_t baud_rate) {
	
	//No settings here are used
	UCSR0A = 0x00;
	
	//Set TXEN0 to 1 (enable transmit)
	UCSR0B = 0x00;
	UCSR0B |= (1 << TXEN0);
	
	//Use UART mode with no parity, one stop bit and 8 data bits
	UCSR0C = 0x00;
	UCSR0C |= (1 << UCSZ00);
	UCSR0C |= (1 << UCSZ01);
	
	//Set UBRR as per baud rate formula
	UBRR0 = 2000000 / (16 * (uint32_t)baud_rate) - 1;
	
}

void uart_transmit_byte(uint8_t data){
	
	// Wait until transmit register is empty
	while (~UCSR0A & (1 << UDRE0)) {
		;
	}
	
	//Write to the data register
	UDR0 = data; 

}

int uart_printf(char var, FILE *stream) {
	uart_transmit_byte(var);      //Using our original function to transmit UART data
	return 0;
}