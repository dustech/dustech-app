namespace Dustech.App.DAL

open System
open System.Collections.Immutable
open System.Data
open System.Data.Common
open Dustech.App.Infrastructure.Auth
open Npgsql

module AuthInDatabase =

    let private getAllIdentities (cd: ConnectionData) =
        let builder = getPostgresConnectionStringBuilder cd

        let rec readUserRows (reader: DbDataReader) (users: List<User>) =
            async {
                let! hasRowToRead = reader.ReadAsync() |> Async.AwaitTask

                if hasRowToRead then
                    let! user_id = reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("user_id")) |> Async.AwaitTask

                    let! sub = reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("sub")) |> Async.AwaitTask

                    let! username =
                        reader.GetFieldValueAsync<string>(reader.GetOrdinal("username"))
                        |> Async.AwaitTask

                    let! password =
                        reader.GetFieldValueAsync<string>(reader.GetOrdinal("password"))
                        |> Async.AwaitTask

                    let! active = reader.GetFieldValueAsync<bool>(reader.GetOrdinal("active")) |> Async.AwaitTask

                    let! concurrent_stamp =
                        reader.GetFieldValueAsync<string>(reader.GetOrdinal("concurrent_stamp"))
                        |> Async.AwaitTask

                    let myUser =

                        { Id = user_id
                          Sub = sub
                          Username = username
                          Password = password
                          Active = active
                          ConcurrentStamp = concurrent_stamp }

                    let users = myUser :: users
                    return! readUserRows reader users
                else
                    return users
            }

        let rec readUserClaimRows (reader: DbDataReader) (claims: List<UserClaim>) =
            async {
                let! hasRowToRead = reader.ReadAsync() |> Async.AwaitTask

                if hasRowToRead then
                    let! id =
                        reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("claim_id"))
                        |> Async.AwaitTask

                    let! user_id = reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("user_id")) |> Async.AwaitTask

                    let! claim_type = reader.GetFieldValueAsync<string>(reader.GetOrdinal("type")) |> Async.AwaitTask

                    let! claim_value = reader.GetFieldValueAsync<string>(reader.GetOrdinal("value")) |> Async.AwaitTask

                    let! concurrent_stamp =
                        reader.GetFieldValueAsync<string>(reader.GetOrdinal("concurrent_stamp"))
                        |> Async.AwaitTask

                    let userClaim =
                        { Id = id
                          UserId = user_id
                          Type = claim_type
                          Value = claim_value
                          ConcurrentStamp = concurrent_stamp }

                    let claims = userClaim :: claims
                    return! readUserClaimRows reader claims
                else
                    return claims
            }

        let getAllIdentitiesAsync () =
            async {
                let connection = new NpgsqlConnection(builder.ConnectionString)
                // connection.StateChange.AddHandler connectionStateChange
                // connection.Notice.AddHandler connectionInfoMessage
                let mutable users = List<User>.Empty
                let mutable userClaims = List<UserClaim>.Empty

                try
                    do! Async.AwaitTask(connection.OpenAsync())

                    let command = connection.CreateCommand()
                    command.CommandType <- CommandType.Text

                    command.CommandText <-
                        """
                                SELECT u.user_id, u.sub, u.username, u.password, u.active, u.concurrent_stamp
                                FROM auth."user" u 
                                LIMIT 501;
                                SELECT uc.claim_id, uc.user_id, uc.type, uc.value, uc.concurrent_stamp
                                FROM auth.user_claim as uc 
                                LIMIT 501
                                                                """

                    use! reader = command.ExecuteReaderAsync() |> Async.AwaitTask

                    let! users = readUserRows reader users

                    let! userClaims =
                        async {
                            if reader.NextResultAsync() |> Async.AwaitTask |> Async.RunSynchronously then
                                let! userClaims = readUserClaimRows reader userClaims
                                return userClaims
                            else
                                return userClaims
                        }

                    let storage =
                        users
                        |> Seq.map (fun u ->
                            { User = u
                              UserClaims = userClaims |> Seq.filter (fun c -> c.UserId = u.Id) })

                    do! Async.AwaitTask(connection.CloseAsync())
                    return storage
                with ex ->
                    printfn $"ERROR: %s{ex.Message}"
                    return ImmutableList<Identity>.Empty
            }

        getAllIdentitiesAsync () |> Async.RunSynchronously

    type AuthInPostgres(conData: ConnectionData) =
        let mutable identities: seq<Identity> = Seq.empty

        interface IIdentity with
            member this.GetEnumerator() : System.Collections.Generic.IEnumerator<Identity> = identities.GetEnumerator()

            member this.GetEnumerator() : System.Collections.IEnumerator =
                (this :> seq<Identity>).GetEnumerator() :> System.Collections.IEnumerator

            member this.getIdentities(query) =
                let filterByUsernameAndPassword query (i: Identity) =
                    match query.Password with
                    | None -> true
                    | Some p ->
                        match  query.Username with
                        | None -> false
                        | Some u ->
                            u.ToLower() = i.User.Username.ToLower() && p = i.User.Password

                let filterByUsername query (i: Identity) =                    
                    match  query.Username with
                    | None -> true
                    | Some u ->
                        u.ToLower() = i.User.Username.ToLower()
                
                let filterBySub query (i: Identity) =                    
                    match  query.Sub with
                    | None -> true
                    | Some s ->
                        s.ToLower() = i.User.Sub.ToString().ToLower()
                
                identities <- getAllIdentities conData
                |> Seq.filter (filterByUsernameAndPassword query)
                |> Seq.filter (filterByUsername query)
                |> Seq.filter (filterBySub query)
                            
                identities

    let toAuthInPostgres conData = AuthInPostgres conData
