namespace Dustech.App.Domain

open System
open Dustech.App.Infrastructure

[<AutoOpen>]
module Gender =
    type Gender =
        | Male
        | Female
        | Other
    
    let genderToString = function
    | Male -> "Male"
    | Female -> "Female"
    | Other -> "Other"

    let stringToGender = function
    | "Male" -> Male
    | "Female" -> Female
    | "Other" -> Other
    | _ -> failwith "Invalid gender"
    
    let toLowerInvariant (s:string) = s.ToLowerInvariant()
    let equal (gender: Gender) (value: string) =
        gender
        |> genderToString
        |> toLowerInvariant
        |> curry String.Equals (value.ToLowerInvariant())



[<AutoOpen>]
module Users =

    type User =
        {
          Id : Guid
          Name: string
          LastName: string
          Quote: string Option
          PublicQuote: string Option
          Gender: Gender }

        static member instance =
            {
              Id = Guid.Empty
              Name = ""
              LastName = ""
              Quote = Some ""
              PublicQuote = Some ""
              Gender = Other }


    type UserQuery =
        { Name: string Option
          Gender: string Option
          QueryUserId : Guid
          IsAdmin : bool }

    type IUser =
        inherit seq<User>
        abstract getUsers: UserQuery -> seq<User>


module ExampleUsers =
    let exampleUsers =
        seq<User>
            [ { User.instance with
                  Name = "Cannolo"
                  Gender = Male }
              { User.instance with
                  Name = "MaryGold"
                  Gender = Female }
              { User.instance with
                  Name = "Lilly"
                  Gender = Female }
              { User.instance with
                  Name = "Brondu"
                  Gender = Male } ]

type UsersInMemory(users: seq<User>) =

    interface IUser with

        member _.GetEnumerator() = users.GetEnumerator()

        member this.GetEnumerator() =
            (this :> seq<User>).GetEnumerator() :> System.Collections.IEnumerator

        member this.getUsers(query) =

            let filterByName f (u: User) =
                match f.Name with
                | None -> true
                | Some n -> u.Name = n

            let filterByGender f (u: User) =
                match f.Gender with
                | None -> true
                | Some g -> equal u.Gender g

            users |> Seq.filter (filterByName query) |> Seq.filter (filterByGender query)



    static member toUsers users = UsersInMemory users
