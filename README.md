# Task Driven Threading

Task Driven Threading is a simple, yet lightweight alternative to 
[Unity's Job System](https://docs.unity3d.com/Manual/JobSystem.html) that lets you execute your code on other threads, 
so your game can run smoothly and without issues. This package includes a wide selection of different ways to set up 
your threads as tasks that can execute your code in any way you want, with a multitude of handy tools to keep track of the 
threads.

## Features

- Multiple interfaces to implement, while still keeping you able to perform inheritance.
  - [Task](Runtime/Task.cs): For your simple threading needs, with or without return typing.
  - [MultiTask](Runtime/MultiTask.cs): For those who have a lot of repeating work, now able to be done in parallel.
    Also includes return typing.
  - [Routine](Runtime/Routine.cs): Keep your thread running with reoccurring work at a specific interval.
- A special [Tracker](Runtime/Tracker.cs) to keep track of any task in progress with handy events that will notify you
  when it is done.
- Task dependency. Create long chains of task in parallel or depended upon each other.
- Timeout control, to combat the worst kind of threads. Specify a time limit for your tasks to keep them from spiraling 
  out of control.

## Prerequisites

Task Driven Threading, like any multithreading solution, needs to be used carefully in a specific scenario with a
logical use case. Usually, the [Unity's Job System](https://docs.unity3d.com/Manual/JobSystem.html) is goog enough for
multithreading, but can be limiting. Use this package only when Unity's Job System isn't working for you and you 
satisfy one of the following one or more of the following requirements:

1. Do you have to run code in the background?
2. Does the code take moderate to very long to execute?
3. Do you have many small pieces of code that, together, take a moderate to very long to execute?

## Getting started