# What are Web Crawlers? 

A an automated program that systematically searches websites and downloads content, or discover other websites. It is commonly used in search engines to index websites for search results, SEO tools to analyze websites and backlinks, or generally just to scrape contents for other purposes. 

Crawlers consume resources on visited systems and are often unprompted. This can cause issues, as such there are mechanisms in place that crawlers should adhere to like 'robots.txt' which is a file that dictates which parts of a website should be visited if at all. 

# How to they work?
Crawlers start off from a set of known pages, known as 'seeds'. As the crawler visits the initial seeds, it identifies any hyperlinks and adds them to a list of URLS to visit, the 'crawl frontier'. Depending on the goal of the crawler, it can also scrape and store any relavent content whilst visiting a website.

The 'crawl frontier' is then recursively visited according to a set of policies. 
