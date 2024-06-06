library(tidyverse)
library(rvest)


# My data context is the sustainability of online fashion

# Useful information to webscrape would be the price of items, type of item, perhaps the quantity available

# I will be scraping a website of a company that sells sustainable clothing @ https://recreateclothing.co.nz/ 
# The specifc URL I will use is https://recreateclothing.co.nz/collections/shop-all 
# I have checked the Robots.txt file and this URL is allowed to be scraped. This URL should allow me to gather 
# relevant data such as price


url <- "https://recreateclothing.co.nz/collections/shop-all"

page <- read_html(url) 

# The price of each item of clothing 
money <- page %>%
  html_elements(".money") %>%
  html_text2()

clothes <- page %>%
  html_elements(".product-thumbnail__title") %>%
  html_text2()

heading <- page %>%
  html_element("h2") %>%
  html_text2()

paragraph <- page %>%
  html_element("p") %>%
  html_text2()

heading
paragraph
money
clothes
