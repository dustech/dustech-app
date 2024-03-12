namespace Dustech.App.Infrastructure

open System

module Auth =

    let newConcurrentStamp = Guid.NewGuid().ToString()

    type User =
        { Id: Guid
          Sub: Guid
          Username: string
          Password: string
          Active: bool
          ConcurrentStamp: string }
        static member instance = {
            Id = Guid.Empty
            Sub = Guid.Empty
            Username = ""
            Password = ""
            Active = false
            ConcurrentStamp = newConcurrentStamp 
        }
    type UserClaim =
        { Id: Guid
          UserId: Guid
          Type: string
          Value: string
          ConcurrentStamp: string }
        static member instance = {
            Id = Guid.Empty
            UserId = Guid.Empty
            Type = ""
            Value = ""
            ConcurrentStamp = newConcurrentStamp 
        }

    type IdentityQuery = {
        Id: Guid Option
        Username : string Option
        Password : string Option
        Sub : string Option
    }
    
    type Identity = {
        User : User
        UserClaims : UserClaim seq
    }
    
    type IIdentity =
        inherit seq<Identity>
        abstract getIdentities: IdentityQuery -> seq<Identity>