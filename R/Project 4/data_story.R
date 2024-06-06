library(magick)

# Step 2: Load your visualizations saved as image files
plot1 <- image_read("plot1.png")
plot2 <- image_read("plot2.png")
plot3 <- image_read("plot3.png")

# Step 3: Create each slide with its content
slide1 <- image_blank(2000, 2000, "#FFFFFF") %>%
  image_annotate( "Analyzing League of Legends Videos", size = 40, color = "#000000", gravity = "center") 

slide2 <- image_blank(2000, 2000, "cyan") %>%
 image_composite(plot1, offset = "+50+50") %>%
 image_annotate("Analyzing videos with 'Faker' in the title or not to see if there is a difference in the view count", size = 40, color = "#000000", gravity = "south")

slide3 <- image_blank(2000, 2000, "cyan") %>%
 image_composite( plot2, offset = "+50+50") %>%
 image_annotate( "Trying to see if there an above average number of comments when 'Faker' is in the title", size = 40, color = "#000000", gravity = "south")

slide4 <- image_blank(2000, 2000, "cyan") %>%
 image_composite(plot3, offset = "+50+50") %>%
 image_annotate( "Do people like longer or shorter videos", size = 40, color = "#000000", gravity = "south")

slide5 <- image_blank(2000, 2000, "cyan") %>%
 image_annotate( "Overall, I learned that League of Legend videos that involve the name 'Faker' have the 
                  potential to get really high views. Looking at the comments of these youtube videos we 
                  can see that on average their seem to be about 402 commments per video. Also by looking 
                  at the likes on videos longer than 10 minutes and shorter than 10 minutes seperately, 
                  we can see that videos longer than 10 minutes seem to be better recieved as 
                  they have more likes.", size = 40, color = "#000000", gravity = "center")

slide_duration <- rep(5, 5) 


animation <- image_join(slide1, slide2, slide3, slide4, slide5) %>%
 image_animate( fps = 1)


image_write(animation, "data_story.gif")

