namespace Dustech.App.Infrastructure

open Microsoft.Extensions.Configuration
open Microsoft.VisualBasic.CompilerServices


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

module IdpConfigurationParser =
    
    let internal webAppInternalUri = "http://webapp:5002"
    let internal authInternalUri = "http://idp:5001/"
    let internal webAppHttpsExternalUri = "https://app.dustech.io"    
    let internal authHttpsExternalUri = "https://auth.dustech.io"
    type IdpConfiguration = { Proxied: bool }

    let toBool (value: string) =
        match value with
        | null -> raise (IncompleteInitialization())
        | _ ->
            match value.ToLower() with
            | "true" -> true
            | "false" -> false
            | _ -> raise (IncompleteInitialization())

    let parseIdpConfiguration (configSection: IConfigurationSection) =
        { Proxied = configSection["Proxied"] |> toBool }

module Hijacker =
    open Microsoft.AspNetCore.Authentication.OpenIdConnect
    let log message = 
        printfn $"%s{message}" 
    let replaceUri (uri:string) = 
        log $"Try to substitute: {uri}"
        let newUri = uri
                        .Replace(IdpConfigurationParser.webAppInternalUri, IdpConfigurationParser.webAppHttpsExternalUri)
                        .Replace(IdpConfigurationParser.authInternalUri, IdpConfigurationParser.authHttpsExternalUri )
        log $"URI result: {newUri}"
        newUri
    
    let Hijack (context: RedirectContext) =
        async {
            let maybePostLogoutRedirectUri =
                Option.ofObj context.ProtocolMessage.PostLogoutRedirectUri

            match maybePostLogoutRedirectUri with
            | Some uri ->
                context.ProtocolMessage.PostLogoutRedirectUri <-
                    replaceUri uri
            | _ -> ()

            let maybeIssuerAddress = Option.ofObj context.ProtocolMessage.IssuerAddress

            match maybeIssuerAddress with
            | Some address ->
                context.ProtocolMessage.IssuerAddress <-
                    replaceUri address
            | _ -> ()

            let maybeRedirectUri = Option.ofObj context.ProtocolMessage.RedirectUri

            match maybeRedirectUri with
            | Some redirectUri ->
                context.ProtocolMessage.RedirectUri <-
                    replaceUri redirectUri                        
            | _ -> ()

            return ()
        }
        |> Async.StartAsTask

module Config =
    let private empty = ""

    type Client =
        { Authority: string
          CallBackPath: string
          ClientName: string
          ClientId: string
          ClientSecret: string
          HashedClientSecret: string
          RedirectUris: string []
          ResponseType: string
          SignOutCallBackPath: string
          SignedOutCallbackPaths: string []
          Scopes: string seq }

    let private authorizationCode = "code"


    let private defaultClient =
        { Authority = empty
          CallBackPath = empty
          ClientName = empty
          ClientId = empty
          ClientSecret = empty
          HashedClientSecret = empty
          RedirectUris = Array.empty
          ResponseType = authorizationCode
          SignOutCallBackPath = empty
          SignedOutCallbackPaths = Array.empty
          Scopes =
            [ StandardScopes.Profile
              StandardScopes.OpenId ] }

    let private callBackPath = "/signin-oidc"
    let private signOutCallBackPath = "/signout-callback-oidc"
   

    let razorPagesWebClient =
        { defaultClient with
            Authority = IdpConfigurationParser.authInternalUri
            CallBackPath = callBackPath
            ClientName = "Dustech.Io"
            ClientId = "dustechappwebclient"
            ClientSecret = "secret"
            HashedClientSecret = Hashing.sha256 <| Some "secret"
            RedirectUris =
                [| $"{IdpConfigurationParser.webAppInternalUri}{callBackPath}"
                   $"{IdpConfigurationParser.webAppHttpsExternalUri}{callBackPath}" |]
            SignOutCallBackPath = signOutCallBackPath
            SignedOutCallbackPaths =
                [| $"{IdpConfigurationParser.webAppInternalUri}{signOutCallBackPath}"
                   $"{IdpConfigurationParser.webAppHttpsExternalUri}{signOutCallBackPath}" |] }
