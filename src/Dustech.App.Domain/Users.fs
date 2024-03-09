namespace Dustech.App.Domain
open Dustech.App.Infrastructure

[<AutoOpen>]
module Gender =
    type Gender =
        | Male
        | Female
        | Other
        
    let equal (gender:Gender) (value:string) =
        gender.ToString().ToLowerInvariant() = value.ToLowerInvariant()
        
        

[<AutoOpen>]
module Users =
    
    type User = { Name: string; Gender: Gender }
    
    type UserQuery =
        { Name: string Option
          Gender: string Option }

    type IUser =
        inherit seq<User>

        abstract getUsers: UserQuery -> seq<User>


module ExampleUsers =
    let exampleUsers = seq<User> [
        { Name = "Cannolo"; Gender = Male }
        { Name = "MaryGold"; Gender = Female}
        { Name = "Lilly"; Gender = Female }
        { Name = "Brondu"; Gender = Male }
    ]

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
                | Some g ->
                    equal u.Gender g

            users
            |> Seq.filter (filterByName query)
            |> Seq.filter (filterByGender query)



    static member toUsers users = UsersInMemory users
