
open System
open Dustech.App.DAL



let pwd = Environment.GetEnvironmentVariable("PSLpwd")
let usr = Environment.GetEnvironmentVariable("PSLusr")
let host = Environment.GetEnvironmentVariable("PSLhost")
let database = Environment.GetEnvironmentVariable("PSLdatabase")
let storage = Users.openCon {
        Username = usr
        Password = pwd
        Host = host
        Database = database 
    }


storage.ForEach(fun u -> printfn $"{u.Name}")


