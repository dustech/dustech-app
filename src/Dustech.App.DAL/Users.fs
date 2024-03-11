namespace Dustech.App.DAL

open System.Collections.Immutable
open System.Data
open Npgsql

[<AutoOpen>]
module Handlers =
    let connectionStateChange sender (e: StateChangeEventArgs) =
        printfn $"State change from {e.OriginalState} to {e.CurrentState}."

    let connectionInfoMessage sender (e: NpgsqlNoticeEventArgs) = printfn $"Info: {e.Notice}."

module Users =
    type User = { Name: string }
    type ConnectionData = {
        Username:string
        Password:string
        Host:string
        Database:string
    }
    let openCon (cd:ConnectionData)=

        let (builder: NpgsqlConnectionStringBuilder) = NpgsqlConnectionStringBuilder()
        builder.Database <- cd.Database
        builder.Timeout <- 10
        //builder.SslMode <- SslMode.Require
        builder.Host <- cd.Host
        builder.Username <- cd.Username
        builder.Password <- cd.Password
        builder.PersistSecurityInfo <- false


        let asyncConnectAndDisconnect () =
            async {
                let connection = new NpgsqlConnection(builder.ConnectionString)
                connection.StateChange.AddHandler connectionStateChange
                connection.Notice.AddHandler connectionInfoMessage
                let mutable storage = ImmutableList<User>.Empty
                try
                    do! Async.AwaitTask(connection.OpenAsync())

                    let command = connection.CreateCommand()
                    command.CommandType <- CommandType.Text

                    command.CommandText <-
                        """
                                            SELECT t.*
                                            FROM usr."user" t
                                            LIMIT 501
                                        """

                    use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
                    

                    let rec readRows() = async {                            
                            let! hasRowToRead = reader.ReadAsync() |> Async.AwaitTask
                            if hasRowToRead then     
                                let! name = reader.GetFieldValueAsync<string>(reader.GetOrdinal("name")) |> Async.AwaitTask
                                let myUser = { Name = name }
                                storage <- storage.Add(myUser)
                                return! readRows()
                                           
                        }
                    do! readRows()
                    do! Async.AwaitTask(connection.CloseAsync())
                    return storage
                with ex ->
                    printfn $"ERROR: %s{ex.Message}"
                    return storage
            }

        asyncConnectAndDisconnect () |> Async.RunSynchronously
