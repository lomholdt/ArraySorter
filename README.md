# Array Sorter

![.NET Core](https://github.com/lomholdt/ArraySorter/workflows/.NET%20Core/badge.svg)

## Build

> .NET SDK 5.0 is required
>
> If you are using project Tye to build/run you will need .NET SDK 3.1

`dotnet build`

`dotnet tool restore`

## Run

`dotnet tye run`

or

`dotnet run --project src/ArraySorter.Api`

## Test

`dotnet test`

## Architectural Decisions

- Hexagonal Architecture ([Ports and adapters](assets/hexagonal-fig-4-4-v-vernon.png))
- A CQRS implementation using Commands and Queries
- Domain Driven Design

## Notes

- Duration is calculated as the time a job spends while being processed (meaning the time it took to sort the array)
- Restarting the application will clear the in memory database.
- Read models are using the repository for sake of simplicity
