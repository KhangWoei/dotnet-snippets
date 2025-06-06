# Crawling Policies
These defines the behaviour of the crawler.

## 1. Selection Policy

Describes which page to download. 

### 1.1 Restricting followed links
Using a HTTP HEAD request to determines a web resource's Multipurpose Internet Mail Extension Type (MIME or Media Type like audio, image, message ... etc) and deciding to to download or request the entire resource only if required or desired. 

### 1.2 URL normalization
Standardizing URL to determine if two syntactically different URLs are equivalent so to avoid crawling the same resource more than once. 

### 1.3 Path-ascending crawling

### 1.4 Focused crawling
Crawlers that attempt to download pages that are similar to each other. 

#### 1.4.1 Academic focused
Crawls free-access academic related documents.

#### 1.4.2 Semantic focused
Uses domain ontologies to represent topical maps and link web pages with relavent ontological concepts.

## 2. Re-visit policy 
Crawling is time consuming, and the Web is ever changing, by the time a crawl is complete the pages that has been visited could very well be out of date. There are cost functions available to drive the re-visit policy, common ones are freshness and age. 

Freshness is a measure that indicates whether a local copy is accurate or not.

Age determines how outdated the local copy is. 

*I have no plans on implementing this policy at the moment, will have to revisit this in the future*

## 3. Politeness policy
Crawlers consume resource on a system it is visiting and can have crippling impact of the performance of a site.

The 'robots exclusion protocol' was created to circumvent this. This is a standard for administrators used to indicate which parts of their servers should not be accessed by crawlers, though it does not include suggestions like interval of visits. 

## 4. Parallelization policy
Crawlers that are able to run multiple process in parallel with the goal being to maximize download rate while minimizing the overhead from parallelization and to avoid repeated downloads of the same page. 
