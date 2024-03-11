namespace Dustech.App.Infrastructure

open System.Globalization

[<AutoOpen>]
module ConsoleLogging =
    let show message = printfn $"%s{message}"
    
module DeLocalizer =        
    let toInvariant<'T> (value:'T) =
        match box value with
        | :? double as doubleValue ->
            doubleValue.ToString("N2", CultureInfo.InvariantCulture)
        | :? int as intValue ->
            intValue.ToString(CultureInfo.InvariantCulture)
        | :? string as stringValue ->
            stringValue 
        | _ ->
            failwith "Not supported type"
            
[<AutoOpen>]
module FunctionOperations =
    let curry f = fun x -> fun y -> f (x, y)
    let uncurry f = fun (x, y) -> (f x) y