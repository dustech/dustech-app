open System
open Dustech.App.DAL
open Dustech.App.DAL.AuthInDatabase
open Dustech.App.DAL.UsersInDatabase
open Dustech.App.Infrastructure.Auth



let pwd = Environment.GetEnvironmentVariable("PSLpwd")
let usr = Environment.GetEnvironmentVariable("PSLusr")
let host = Environment.GetEnvironmentVariable("PSLhost")
let database = Environment.GetEnvironmentVariable("PSLdatabase")
// let storage = UsersInDatabase.openCon {
//         Username = usr
//         Password = pwd
//         Host = host
//         Database = database
//     }
//
//
// storage.ForEach(fun u -> printfn $"{u.Name}")

let conData: Common.ConnectionData =
    { Username = usr
      Password = pwd
      Host = host
      Database = database }
let authInPostgres = toAuthInPostgres conData
let query = {IdentityQuery.Id = Some Guid.Empty}
let identities = (authInPostgres :> IIdentity).getIdentities query

let veryfyme = true



