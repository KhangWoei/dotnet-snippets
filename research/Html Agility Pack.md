# HTML Agility Pack
Free an open source library used to parse and manipulate HTML documents in C#.

## Examples
### 1. To load from string

``` 
    var html = """
               <html>
                    some html
               </html> 
               """
    var doc = new HtmlDocument();
    doc.LoadHtml(html);

    ... etc
```

### 2. To load from url
```
    var uri = @"https://html-agility-pack.net/"
    var web =   new HtmlWeb();
    
    var doc = web.Load(uri);

    ...etc
```
