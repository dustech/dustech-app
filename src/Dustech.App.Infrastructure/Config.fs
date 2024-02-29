﻿namespace Dustech.App.Infrastructure

open System.Runtime.Serialization


module StandardScopes =

    /// <summary>REQUIRED. Informs the Authorization Server that the Client is making an OpenID Connect request. If the <c>openid</c> scope value is not present, the behavior is entirely unspecified.</summary>
    let OpenId = "openid"

    /// <summary>OPTIONAL. This scope value requests access to the End-User's default profile Claims, which are: <c>name</c>, <c>family_name</c>, <c>given_name</c>, <c>middle_name</c>, <c>nickname</c>, <c>preferred_username</c>, <c>profile</c>, <c>picture</c>, <c>website</c>, <c>gender</c>, <c>birthdate</c>, <c>zoneinfo</c>, <c>locale</c>, and <c>updated_at</c>.</summary>
    let Profile = "profile"

    /// <summary>OPTIONAL. This scope value requests access to the <c>email</c> and <c>email_verified</c> Claims.</summary>
    let Email = "email"

    /// <summary>OPTIONAL. This scope value requests access to the <c>address</c> Claim.</summary>
    let Address = "address"

    /// <summary>OPTIONAL. This scope value requests access to the <c>phone_number</c> and <c>phone_number_verified</c> Claims.</summary>
    let Phone = "phone"

    /// <summary>This scope value MUST NOT be used with the OpenID Connect Implicit Client Implementer's Guide 1.0. See the OpenID Connect Basic Client Implementer's Guide 1.0 (http://openid.net/specs/openid-connect-implicit-1_0.html#OpenID.Basic) for its usage in that subset of OpenID Connect.</summary>
    let OfflineAccess = "offline_access"


module Config =
    let private empty = ""

    type Client =
        { Authority: string
          InternalDuende: string
          ClientName: string
          ClientId: string
          ClientSecret: string
          HashedClientSecret: string
          RedirectUri: string
          RedirectUri2: string
          ResponseType: string
          CallBackPath: string
          SignedOutCallbackPath: string
          Scopes: string seq }

    let private authorizationCode = "code"
    let private callBackPath = "signin-oidc"

    let private defaultClient =
        { Authority = empty
          InternalDuende = empty
          ClientName = empty
          ClientId = empty
          ClientSecret = empty
          HashedClientSecret = empty
          RedirectUri = empty
          RedirectUri2 = empty
          ResponseType = authorizationCode
          CallBackPath = empty
          SignedOutCallbackPath = empty
          Scopes =
            [ StandardScopes.Profile
              StandardScopes.OpenId ] }

    let authD = "https://auth.dustech.io/"
    let localD = "http://auth.dustech.io/"
    let razorPagesWebClient =
        // { defaultClient with
        //     Authority = "https://localhost:5001/"
        //     ClientName = "Dustech.Io"
        //     ClientId = "dustechappwebclient"
        //     ClientSecret = "secret"
        //     HashedClientSecret = Hashing.sha256 <| Some "secret"
        //     RedirectUri = "https://localhost:7273" + callBackPath
        //     SignedOutCallbackPath = "https://localhost:7273/signout-callback-oidc"
        //     }

        // locale
         { defaultClient with
            Authority = authD
            InternalDuende = authD //"https://localhost:5001/"
            ClientName = "Dustech.Io"
            ClientId = "dustechappwebclient"
            ClientSecret = "secret"
            CallBackPath = "/" + callBackPath 
            HashedClientSecret = Hashing.sha256 <| Some "secret"
            RedirectUri = "https://app.dustech.io/" + callBackPath
            //RedirectUri2 = "https://localhost:5002/" + callBackPath
            //SignedOutCallbackPath = "https://localhost:5002/signout-callback-oidc"
            SignedOutCallbackPath = "https://app.dustech.io/signout-callback-oidc"
        }
        
//         
//         { defaultClient with
//             // Authority = "https://auth.dustech.io"
//             Authority = "http://localhost:5001"
//             InternalDuende = "http://localhost:5001"
//             ClientName = "Dustech.Io"
//             ClientId = "dustechappwebclient"
//             ClientSecret = "secret"
//             HashedClientSecret = Hashing.sha256 <| Some "secret"
//             // RedirectUri = "https://auth.dustech.io" + callBackPath
//             RedirectUri = "http://localhost:5002" + callBackPath
//             SignedOutCallbackPath = "https://app.localhost/signout-callback-oidc" }
// //
// { defaultClient with
//     Authority = "http://host.docker.internal:5001/"
//     ClientName = "Dustech.Io"
//     ClientId = "dustechappwebclient"
//     ClientSecret = "secret"
//     HashedClientSecret = Hashing.sha256 <| Some "secret"
//     RedirectUri = "http://host.docker.internal:5002" + callBackPath
//     SignedOutCallbackPath = "http://host.docker.internal:5002/signout-callback-oidc"
//     }
