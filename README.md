# Array Sorter

![.NET Core](https://github.com/lomholdt/ArraySorter/workflows/.NET%20Core/badge.svg)

## Build

`dotnet tool restore`

> .NET SDK 5.0 is required
>
> If you are using project Tye to build/run you will need .NET SDK 3.1

## Run

`dotnet tye run`

## Test

`dotnet test`

## Architectural Decisions

- Hexagonal Architecture (using Ports and adapters)
- A CQRS implementation using Commands and Queries
- Domain Driven Design

## Notes

- Duration is calculated as the time a job was Enqueued until it was Processed.
