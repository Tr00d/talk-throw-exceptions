module Tests

open Xunit

let incrementValue value = value + 1
let convertToMessage value = $"The value is some {value}!"
let defaultMessage = "The value is none"
let matchResult content =
    match content with
    | None -> defaultMessage
    | Some value -> convertToMessage value

[<Fact>]
let ``F# equivalent!`` () =
    let result = 
        Some(3)
        |> Option.map incrementValue
        |> Option.map incrementValue
        |> Option.map incrementValue
        |> matchResult
    Assert.Equal("The value is some 6!", result)   