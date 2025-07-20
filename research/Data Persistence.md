# Data Persistence 

## 1. Relational database - PostgreSQL

References:
- [Trees in SQL](http://www.dbazine.com/oracle/or-articles/tropashko4/)

### 1.1 Adjacency List
Basically creating a self reference relationship, each record points to a parent, if it has one. 

``` SQL
CREATE TABLE Tree (
    id SERIAL PRIMARY KEY,
    name VARCHAR 
)

CREATE TABLE Nodes (
    id SERIAL PRIMARY KEY
    tree_id INTEGER NOT NULL REFERENCES Tree(id),
    parent_id INTEGER REFERENCES Nodes(id),
    path VARCHAR,
    is_terminal BOOLEAN DEFAULT FALSE)
```

### 1.2 Materialized Path
Each record stores the whole path to the root. So each node would store the full path to root.

``` SQL
CREATE TABLE Tree (
    id SERIAL PRIMARY KEY,
    name VARCHAR 
)


CREATE TABLE Nodes (
    id SERIAL PRIMARY KEY
    tree_id INTEGER NOT NULL REFERENCES Tree(id),
    parent_id INTEGER REFERENCES Nodes(id),
    path VARCHAR,
    is_terminal BOOLEAN DEFAULT FALSE,
    path_from_root VARCHAR,
    )
```

#### Benefits
- Faster queries, can just filter on `path_from_root`
- Subsequently can add an index on `path_from_root`

#### Drawbacks
- More storage
- Complex update/insert, will have to update `path` and `path_from_root` and ensure it is in sync (?), could split this out to a different table but will still need to sync it. 

## 2. Graph database - Neo4j or Amazon Neptune

References:
- [What is a graph database](https://neo4j.com/docs/getting-started/graph-database/)
- [Cypher query langauge](https://neo4j.com/docs/getting-started/cypher/)

Database that stores data as `nodes` and `relationships` instead of in tables or documents. 

Nodes are entities in a graph that can be: (Vertices)
- Tagged with labels 
- Hold any number of key value pairs (properties, the equivalent to a column?)
- Be indexed or bound by constraints
- Can have multiple relationships (or none?)

Relationships provide named connection between two nodes: (Edges) (Equivalent to a FK relationship?)
- Must always have a start and end node
- Must have a direction
- Can have properties (?)

Example (in Neo4j):

1. Creating a Node:
```
CREATE (tomHanks:Person:Actor { name: 'Tom Hanks', born: 1956 })
CREATE (forrestGump:Movie { title: 'Forrest Gump', released: 1994 })

CREATE (<labels> <properties>)
```

2. Creating a relationship
```
CREATE (:Person:Actor { name: 'Tom Hanks', born: 1956})-[:ACTED_IN { roles: ['Forrest'] }]->(:Movie { title: 'Forrest Gump', released: 1994 })

CREATE(tomHanks)-[:ACTED_IN { ... }]->(forrestGump)

CREATE (<source_node>)-[<relationship_name>]->(<target_node>)
```
Do I have to reference the whole <labels> <properties> to identify a node each time? 

### 2.1 Trie example
```
CREATE (contoso_tree:Tree { name: 'contoso' })
CREATE (contoso_tree)-[:OWNS]->(root:TrieNode {
        path: '/',
        is_terminal: false
    })

CREATE (root)-[:HAS_CHILD]->(about:TrieNode { path: 'about', is_terminal: true })
```
