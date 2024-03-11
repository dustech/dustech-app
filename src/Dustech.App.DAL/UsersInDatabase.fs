namespace Dustech.App.DAL

open System
open System.Collections.Immutable
open System.Data
open Dustech.App.Domain
open Npgsql

// [<AutoOpen>]
// module Handlers =
//     let connectionStateChange sender (e: StateChangeEventArgs) =
//         printfn $"State change from {e.OriginalState} to {e.CurrentState}."
//
//     let connectionInfoMessage sender (e: NpgsqlNoticeEventArgs) = printfn $"Info: {e.Notice}."

module UsersInDatabase =
    type ConnectionData =
        { Username: string
          Password: string
          Host: string
          Database: string }

    let internal getAllUsers (cd: ConnectionData) =

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
                // connection.StateChange.AddHandler connectionStateChange
                // connection.Notice.AddHandler connectionInfoMessage
                let mutable storage = ImmutableList<User>.Empty

                try
                    do! Async.AwaitTask(connection.OpenAsync())

                    let command = connection.CreateCommand()
                    command.CommandType <- CommandType.Text

                    command.CommandText <-
                        """
                                            SELECT t.user_id, t.name, t.last_name, t.quote, t.gender, t.public_quote 
                                            FROM usr."user" t
                                            LIMIT 501
                                        """

                    use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask


                    let rec readRows () =
                        async {
                            let! hasRowToRead = reader.ReadAsync() |> Async.AwaitTask

                            if hasRowToRead then
                                let! user_id =
                                    reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("user_id")) |> Async.AwaitTask

                                let! name =
                                    reader.GetFieldValueAsync<string>(reader.GetOrdinal("name")) |> Async.AwaitTask

                                let! last_name =
                                    reader.GetFieldValueAsync<string>(reader.GetOrdinal("last_name"))
                                    |> Async.AwaitTask
                                
                                let quoteOrdinal = reader.GetOrdinal("quote")
                                let! quoteNull = reader.IsDBNullAsync(quoteOrdinal) |> Async.AwaitTask
                                
                                let! quoteOption =
                                    match not quoteNull with 
                                    | true ->  async {
                                                let! quote = reader.GetFieldValueAsync<string>(quoteOrdinal) |> Async.AwaitTask
                                                return Some quote
                                                }                                                 
                                    | false -> async { return None }
                                    
                                let! gender =
                                    reader.GetFieldValueAsync<string>(reader.GetOrdinal("gender"))
                                    |> Async.AwaitTask
                                    
                                let publicQuoteOrdinal = reader.GetOrdinal("public_quote")
                                let! publicQuoteNull = reader.IsDBNullAsync(publicQuoteOrdinal) |> Async.AwaitTask
                                
                                let! publicQuoteOption =
                                    match not publicQuoteNull with 
                                    | true ->  async {
                                                let! publicQuote = reader.GetFieldValueAsync<string>(publicQuoteOrdinal) |> Async.AwaitTask
                                                return Some publicQuote
                                                }                                                 
                                    | false -> async { return None }
                                    
                                let! gender =
                                    reader.GetFieldValueAsync<string>(reader.GetOrdinal("gender"))
                                    |> Async.AwaitTask

                                let myUser =
                                    {   Id = user_id
                                        Name = name
                                        LastName = last_name
                                        Quote = quoteOption
                                        PublicQuote = publicQuoteOption 
                                        Gender  = stringToGender gender }

                                storage <- storage.Add(myUser)
                                return! readRows ()

                        }

                    do! readRows ()
                    do! Async.AwaitTask(connection.CloseAsync())
                    return storage
                with ex ->
                    printfn $"ERROR: %s{ex.Message}"
                    return storage
            }

        asyncConnectAndDisconnect () |> Async.RunSynchronously

    type UsersInPostgres(conData: ConnectionData) =
        let mutable users: seq<Users.User> = Seq.empty

        interface IUser with
            member this.GetEnumerator() : System.Collections.Generic.IEnumerator<User> = users.GetEnumerator()

            member this.GetEnumerator() : System.Collections.IEnumerator =
                (this :> seq<User>).GetEnumerator() :> System.Collections.IEnumerator

            member this.getUsers(query) =
                let filterByName query (u: User) =
                    match query.Name with
                    | None -> true
                    | Some n -> u.Name = n

                let filterByGender query (u: User) =
                    match query.Gender with
                    | None -> true
                    | Some g -> equal u.Gender g
                    
                let maskPrivateQuote query (u: User) =
                    if query.QueryUserId.Equals u.Id || query.IsAdmin
                    then u
                    else {u with Quote = None }
                    

                users <-
                    getAllUsers (conData)
                    |> Seq.filter (filterByName query)
                    |> Seq.filter (filterByGender query)
                    |> Seq.map (maskPrivateQuote query)

                users

    let toUsersInPostgres conData = UsersInPostgres conData
