library(tidyverse)



youtube_data <- read.csv('https://docs.google.com/spreadsheets/d/e/2PACX-1vQKPXO2ualEZnThidWU7Lvz8xE2xHjO_fsc45OpvO5w0bFEn3paR-h6LsJ2IQnYdwaRvpd5Lfltiqm0/pub?gid=0&single=true&output=csv')


iwd <- youtube_data %>%
  filter(channelName == ('@IWDominatelol'))

mid <- youtube_data %>%
  filter(channelName == ('@midbeast9495'))

faker_data <- youtube_data %>%
  mutate(Faker = ifelse(str_detect(tolower(title), 'faker'),'Is in the Title', 'Is not in the Title'))
  
  
plot_1 <- faker_data %>% 
  ggplot() +
  geom_bar(aes(x = title, y = viewCount, fill = Faker), stat = "identity") +
  labs(title = "View Count for Videos with 'Faker' in Title", x = "Videos", y = "View Count") +
theme(axis.text.x = element_blank(), axis.ticks.x = element_blank()) 

ggsave("plot1.png", plot = plot_1, width = 6, height = 4, dpi = 300)

mean_comments <- youtube_data$commentCount %>%
  mean() 
  
plot_2 <- faker_data %>%
  ggplot() +
  geom_point(aes(y = commentCount, x = videoId, fill = Faker)) +
  labs(title = "Number of comments per video", x = "Videos", y = "Comment Count") +
  theme(axis.text.x = element_blank(), axis.ticks.x = element_blank()) +
  
  geom_hline(yintercept = mean_comments, size = 1, colour = "red") +
  annotate("text", x = Inf, y = 0, label = "Average Number of Comments", hjust = 1, vjust = 1, color = "red") 
  
  
  ggsave("plot2.png", plot = plot_2, width = 6, height = 4, dpi = 300)
  



new_data <- youtube_data %>%
  mutate(ten_minute = ifelse(duration > 600, "Longer", "Shorter")) %>%
  group_by(ten_minute) %>%
  summarise(mean_likes = mean(likeCount))

plot_3 <- new_data %>% ggplot() +
  geom_col(aes(x = ten_minute, y = mean_likes), fill = "cyan") +
  labs(title = "Average Likes for Videos Long or Shorter than 10 Minutes", x = "Longer or Short than 10 Minutes", y = "Average Likes")
  
  ggsave("plot3.png", plot = plot_3, width = 6, height = 4, dpi = 300)

