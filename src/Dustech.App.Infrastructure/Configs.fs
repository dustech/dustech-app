namespace Dustech.App.Infrastructure


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


module Configs =
    let private empty = ""

    type Client =
        { ClientName: string
          ClientId: string
          ClientSecret: string
          RedirectUri: string
          ResponseType: string
          CallBackPath : string
          Scopes: string seq }

    let private authorizationCode = "authorization_code"
    let private callBackPath = "signin-oidc";
    let private defaultClient =
        { ClientName = empty
          ClientId = empty
          ClientSecret = empty
          RedirectUri = empty
          ResponseType = authorizationCode
          CallBackPath = callBackPath          
          Scopes =
            [ StandardScopes.Profile
              StandardScopes.OpenId ] }


    let private razorPagesWebClientSecret = Hashing.sha256 <| Some "secret"

    let razorPagesWebClient =
        { defaultClient with
            ClientName = "Dustech App Web"
            ClientId = "dustechappwebclient"
            ClientSecret = razorPagesWebClientSecret
            RedirectUri = "https://localhost:7273/" + callBackPath }
