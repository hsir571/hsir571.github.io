/*
 * ee209_final.c
 *
 * Created: 10/10/2021 4:42:07 PM
 * Author : Anthony
 */ 

#define F_CPU 2000000UL
#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdint.h>
#include <util/delay.h> 
#include <math.h>
#include <stdio.h>

//Header files
#include "uart0.h"
#include "adc.h"
#include "timer0.h"
#include "display.h"
#include "timer2.h"

//User macros
#define SAMPLE_SIZE 80
#define BAUD_RATE 9600
#define ADC_VOLTAGE_RESOLUTION 0.0048828125
#define VOLTAGE_SIG_COND_GAIN 0.0454016298
#define CURRENT_SIG_COND_GAIN 2.074074074
#define SIG_COND_OFFSET 2.1

//Function prototypes
void calculate_output();
void calculate_missing_data();

//Global variables for signal readings
volatile uint16_t voltage_adc_value[2*SAMPLE_SIZE];
volatile uint16_t current_adc_value[2*SAMPLE_SIZE];

//Outputs
float current_rms = 0;
float peak_voltage = 0;
float power = 0; 

//Flags to check status of output
static uint8_t data_ready_flag = 0;
static uint8_t calculations_complete_flag = 0;

static FILE usart_stdout = FDEV_SETUP_STREAM(uart_printf, NULL, _FDEV_SETUP_WRITE);

//Main program
int main(void)
{	
	stdout = &usart_stdout; 
	
	//Initialise peripherals
	uart_init(BAUD_RATE);
	adc_init();
	timer0_init();
	//timer1_init();
	timer2_init();
	display_init();
	 
   while (1) {	 
	   
		data_ready_flag = 0;
		calculations_complete_flag = 0;
		
		//If PD2 is high, wait for it to become low and then high
		if (PIND & (1 << PD2)) {
			while (PIND & (1 << PD2)) {
				;
			}
			while (~PIND & (1 << PD2)) {
				;
			}
				
		//If PD2 is low, wait for it to become high
		} else {
			while (~PIND & (1 << PD2)) {
				;
			}
		}
		
		adc_enable();
		
		while (!data_ready_flag) {
			;
		}
		
		calculate_output();
		
		while (!calculations_complete_flag) {
			;
		}
		
		//Print output via UART
		printf("Peak Voltage = %.2fV \r\n", peak_voltage);
		printf("RMS Current = %.2fA \r\n", current_rms);
		printf("Power = %.2fW\r\n \r\n", power);
	
		_delay_ms(1000); 

    }
}

void calculate_output() {

	uint16_t peak_voltage_adc_value = 0;
	float current_value = 0 ; 
	float voltage_value = 0 ;
	float current_sum = 0 ;
	float power_sum = 0; 
	
	for(uint8_t i = 1; i < 2*SAMPLE_SIZE - 1;i++){
		
		//Determine the peak voltage
		if (voltage_adc_value[i] > peak_voltage_adc_value) {
			peak_voltage_adc_value = voltage_adc_value[i];
		}
		
		//Convert ADC values to voltages/currents
		current_value = ((current_adc_value[i] * ADC_VOLTAGE_RESOLUTION)- SIG_COND_OFFSET) / CURRENT_SIG_COND_GAIN;
		voltage_value = ((voltage_adc_value[i]* ADC_VOLTAGE_RESOLUTION) - SIG_COND_OFFSET) / VOLTAGE_SIG_COND_GAIN;
		
		//Sum the current and power values
		current_sum  = current_sum + (current_value * current_value);  
		power_sum = power_sum + (current_value * voltage_value);
	}

	//Calculate peak voltage
	peak_voltage = ((peak_voltage_adc_value * ADC_VOLTAGE_RESOLUTION) - SIG_COND_OFFSET) / VOLTAGE_SIG_COND_GAIN;
	
	//Calculate RMS current
	current_rms = sqrt(current_sum/(2*SAMPLE_SIZE - 2));
	
	//Calculate real power
	power = (power_sum / (2*SAMPLE_SIZE - 2)) * 1.03;
	
	calculations_complete_flag = 1;
}


void calculate_missing_data() {
	
	for (uint8_t i = 0; i < SAMPLE_SIZE - 1; i++) {
		
		voltage_adc_value[2*i + 1] = (voltage_adc_value[2*i] + voltage_adc_value[2*i + 2]) / 2;
		current_adc_value[2*i + 2] = (current_adc_value[2*i + 1] + current_adc_value[2*i + 3]) / 2;
				
	}
	data_ready_flag = 1;
}