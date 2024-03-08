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