namespace Dustech.App.Domain

[<AutoOpen>]
module Users =
    
    type User = {
        Name : string
        Gender : string
    }    
    
    type UserQuery = {
        Name: string Option
        Gender : string Option
    }
    
    type IUser =
        inherit seq<User>
        
        abstract getUsers: UserQuery -> seq<User>
        
    
type UsersInMemory(users:seq<User>) =
    interface IUser with
      
        member _.GetEnumerator() = users.GetEnumerator()

        member this.GetEnumerator() =
            (this :> seq<User>)
                .GetEnumerator()
            :> System.Collections.IEnumerator

        member this.getUsers(query) =
                            
            let filterByName f (u:User) =
                    match f.Name with
                    | None -> true
                    | Some n -> u.Name = n
                    
            let filterByGender f (u:User) =
                    match f.Gender with
                    | None -> true
                    | Some n -> u.Gender = n
                    
            users
            |> Seq.filter (filterByName query)
            |> Seq.filter (filterByGender query)

   
        
    static member toUsers users = UsersInMemory users 