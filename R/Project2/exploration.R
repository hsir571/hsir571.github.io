#Import library 
library(tidyverse)

#Read CSV and rename variables 
learning_data <- read_csv("https://docs.google.com/spreadsheets/d/e/2PACX-1vRflaqCqWh8BudP1_9jJPUOHfxltZwpccLdl3UCFQyzK0rnb0hZ_xkGA8NayfcOQbdyIP3kjKfHKsD6/pub?gid=489714701&single=true&output=csv") %>%
  rename("Visits" = 2, "Time(minutes)" = 3, "Fitness_Level" = 4, "Enjoyment_Level" = 5, "Exercise_Experience" = 6)


#Making Some plots 

#plot of number of visits to the gym with blue bars
ggplot(data = learning_data) +
  geom_bar(aes(x = Visits), fill="blue")


#Splitting the exercises into their own rows 
learning_data_edit <- learning_data %>%
  separate_rows(Exercise_Experience, sep = ",")

#plotting the different types of exercises people have done 
ggplot(data = learning_data_edit) +
  geom_bar(aes(x = Exercise_Experience), fill="yellow")
