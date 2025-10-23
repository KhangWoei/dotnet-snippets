# Crawl frontier
Data structure used to store URLs that are eligible for crawling. Should support operations such as queuing URLs, dequeuing URLs, and prioritizing URLs to crawl. 

It should contain logic and policies that a crawler follows when visiting websites. Policies should describe which pages to visit next, priorities for each page to be searched, and how often the page is to be visited. These policies are commonly based on a score, where the score is computed based on a number of different attributes. 

