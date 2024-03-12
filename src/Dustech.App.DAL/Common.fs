namespace Dustech.App.DAL

open Npgsql

[<AutoOpen>]
module Common =
     type ConnectionData =
        { Username: string
          Password: string
          Host: string
          Database: string }

     let getPostgresConnectionStringBuilder (cd: ConnectionData) =
        let builder = NpgsqlConnectionStringBuilder()
        builder.Database <- cd.Database
        builder.Timeout <- 10
        //builder.SslMode <- SslMode.Require
        builder.Host <- cd.Host
        builder.Username <- cd.Username
        builder.Password <- cd.Password
        builder.PersistSecurityInfo <- false
        builder