// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using IdentityModel;
using System.Security.Claims;
using Duende.IdentityServer.Test;

namespace Dustech.IDP;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "9b2a309d-2680-4e77-be37-cdf58261e7a8",
                    Username = "Dustech",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new("role", "Administrator"),
                        new(JwtClaimTypes.GivenName, "Stefano"),
                        new(JwtClaimTypes.FamilyName, "Cerruti"),
                        new(JwtClaimTypes.PreferredUserName, "Dustech")
                    }
                },
                new TestUser
                {
                    SubjectId = "fc8fe991-f341-44e8-a0dc-e22c9d60a26e",
                    Username = "STesla",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new("role", "Normal"),
                        new(JwtClaimTypes.GivenName, "Silvia"),
                        new(JwtClaimTypes.FamilyName, "Tesla"),
                        new(JwtClaimTypes.PreferredUserName, "STesla")

                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Emma",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new("role", "Normal"),
                        new(JwtClaimTypes.GivenName, "Emma"),
                        new(JwtClaimTypes.FamilyName, "Flagg")
                    }
                }
            };
        }
    }
}