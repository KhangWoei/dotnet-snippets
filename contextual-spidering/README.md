# ContextSpider
A bidirectional graph traversal algorithm that visits nodes based on contextual priority.

# Algorithm Overview
ContextSpider traverses a bidirectional graph starting from a specified node, visiting each node exactly once in an order determined by connection context.

# Traversal Rules
1. Starting Node
   The starting node is always visited first, bootstrapping the traversal.
2. Visitability
   A node becomes visitable when at least one of its neighbors (in either direction) has been visited.
3. Priority Selection
   When multiple nodes are visitable, the next node is selected by:
    - Primary: Most visited neighbors (sum of visited dependencies and dependents)
    - Secondary: Fewest unvisited neighbors (tie-breaker)
    - Tertiary: Lexicographic order (deterministic tie-breaker)

The algorithm selects the node with the highest priority, visits it, updates the frontier, and repeats until no visitable nodes remain.

# Termination
The traversal terminates when:
1. No more visitable nodes exist in the frontier
2. All reachable nodes from the starting node have been visited

Nodes unreachable from the starting node are never visited.
