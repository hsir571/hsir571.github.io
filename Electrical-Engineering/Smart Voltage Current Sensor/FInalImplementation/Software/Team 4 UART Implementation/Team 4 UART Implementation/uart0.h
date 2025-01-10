/*
 * uart0.h
 *
 * Created: 30/08/2021 5:37:36 pm
 *  Author: logan
 */ 

#include <avr/io.h>
#include <stdint.h>

#ifndef UART0_H_
#define UART0_H_

void usart_init(uint16_t ubrr);
void usart_transmit(uint8_t data);
void transmitFullNumber(uint8_t numberArray[5]);




#endif /* UART0_H_ */