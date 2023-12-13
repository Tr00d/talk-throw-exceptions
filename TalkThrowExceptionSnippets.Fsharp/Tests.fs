module Tests

open Xunit
open FsUnit.Xunit

let incrementValue value = value + 1
let half value = if value % 2 = 0 then Some(value / 2) else None
let convertToMessage value = $"The value is some {value}!"
let defaultMessage = "The value is none"
let matchResult content =
    match content with
    | None -> defaultMessage
    | Some value -> convertToMessage value

[<Fact>]
let ``F# equivalent!`` () =
    Some(3)
    |> Option.map incrementValue
    |> Option.map incrementValue
    |> Option.map incrementValue
    |> Option.bind half
    |> matchResult
    |> should equal "The value is some 3!"