# Mediator Pattern
Basically an event driven architecture where we have an orchestrational unit or class (event bus) that is responsible for routing events to event handlers.

Used to help decouple components through the introduction of a mediator to act as an intermediary between them, each component can be completely oblivious or isolated from other components, simply needs to know where or what requests to send.

This makes component very modular. 

## [Mediatr](https://github.com/jbogard/MediatR/wiki)

Implementation of this pattern in C#.

Utilizes reflection to figure out which event maps to which event handler.

## Purpose it plays in this project:
Wanted to split the queing responsibility away from the main crawling logic as that will be needed when we start to process sites concurrently. 

Want to be able to also split out the policies handling like whether to visit a site, tracking seen root sites ... etc to it's own service.
