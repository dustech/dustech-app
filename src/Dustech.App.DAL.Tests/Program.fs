
open System
open Dustech.App.DAL



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


6A946644-6089-4D36-B539-F39D0BB59060