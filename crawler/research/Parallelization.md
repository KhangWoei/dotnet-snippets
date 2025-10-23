# Parallelization

## 1. Concurrency vs Parallelization

### 1.1 Concurrency
Dealing with multiple things at once but necessarily simultaneously. System creates the illusion of handling multiple tasks simultaneously through interleaving. 

For example, `async/await`is fundamentally about co-ordinating asynchronous operations rather than multiple things simultanesouly. When we `await`, the current thread is released back into the thread pool to handle other work. The `awaited` operation doesn't block, instead it yield control, when the awaited operation is completed it signals a completion through a continuation mechanism. (Usually there's a synchronization context which is listening for the completion notification)

However, if we fire and forget a CPU bound work then we're threading into the territory of parallelism. 

## 1.2 Parallization
Doing multiple things simultaneously. Requires multiple processing units (CPU cores) to execute different tasks at the same time. (CPU bound work)

For example, calling `Parallel.ForEach()` on a collection will cause the runtime to divide the work among available CPU cores. Each core will executes different iterations of the loop simultaneously. Core 1 might process items 1-250, Core 2 handles items 251-500 ... etc

## 2. Task Parallel Library

Tasks represent asynchronous operations that can run concurrently or in parallel.

### 2.1 Creating a task

Starting a task will schedule work to execute on a thread.

``` C#
Task task1 = Task.Run(() => DoWork());
Task task2 = Task.Run(() => DoWork());

Task.WaitAll(task1, task2);
```

### 2.1.1 Creating a deferred task

``` C#
Task task1 = new Task(() => DoWork());

task1.Start();

Task.WaitAll(task1));
```

### 2.2 Parallel.For

Partitions iterations across available core

```
Parallel.For(0, 1000, i => {
    DoSomething(i);
    });

Parallel.ForEach(collection, item => {
    DoSomething(item)
    });
```

## 3. PLINQ

Automatically parallelizes LINQ queries by distributing work across multiple threads

```
var even = Enumerable.Range(1, 100).AsParallel().Where(n => n % 2 == 0).ToList();
```

## 4. Other (TODO)
- Hosted Services
- Background services
- Thread pools
- Timer-based 
- Produce-consumer (Channels)
- Reactive Extneions (Rx.NET)
- TPL Dataflow
