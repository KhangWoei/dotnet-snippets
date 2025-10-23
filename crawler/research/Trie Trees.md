# Trie (Try) Trees
Also known as a prefix tree, efficient at storng and searching strings or sort of heirarchical data like directories or urls. 

Really good at prefix-related operations, typically can search for a string, check if a stored string starts with a prefix, or find all strings with a common prefix in O(n) time, where `n` is the length of the search string. 

Each node represents a single character (?) or a relative path (for a uri) or a directory or file name (for a file system).

E.g. :
    Given: 
        - `/home/user/documents/file.txt`
        - `/home/user/documents/file2.txt`
        - `/home/user/pictures/cat.jpg`
```
               /                         
               │                         
               ▼                         
             /home                       
               │                         
               ▼                         
             /user                       
               │                         
               │                         
/pictures◄─────┴─────►/documents         
    │                       │            
    │                       │            
    │                       │            
    ▼                       │            
/cat.jpg          /file.txt◄┴► /file2.txt
```
## Radix or Compressed Trees
Basically an optimized tree where any node with one children will just be combined. So only create children if there are more than one children available.

## Pseudo code

```
class TrieNode
    Dictionary<key: URL segment, value: Trie Node>() Children;
    boolean IsTerminal;
```
