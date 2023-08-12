/*
 * uart0.h
 *
 * Created: 11/10/2021 11:30:18 AM
 *  Author: Anthony
 */ 
#include <stdio.h>

#ifndef UART0_H_
#define UART0_H_

void uart_init(uint16_t);
void uart_transmit(uint8_t);
int uart_printf(char var, FILE *stream);

#endif /* UART0_H_ */