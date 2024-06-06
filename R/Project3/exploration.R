library(tidyverse)
library(magick)
library(jsonlite)

url1 <- "https://stat.auckland.ac.nz/~fergusson/stats220_S124/zoom_data/participants5.csv"
url2 <- "https://stat.auckland.ac.nz/~fergusson/stats220_S124/zoom_data/participants1.csv"

data1 <- read_csv(url1)
data2 <- read_csv(url2)

students <- data1$private_name %>%
  unique()

all_data <- bind_rows(data1, data2)

all_data$duration_minutes %>%
  mean() %>%
  round(digits = 1)

grouped_data <- all_data %>%
  group_by(date_lecture, in_waiting_room) %>%
  summarise(num = n())