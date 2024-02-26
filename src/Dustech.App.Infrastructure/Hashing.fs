namespace Dustech.App.Infrastructure

open System
open System.Security.Cryptography
open System.Text

/// <summary>
/// Methods for hashing strings
/// </summary>
module Hashing =
    let private empty = ""

    /// <summary>
    /// Creates a SHA256 hash of the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>A hash</returns>
    let sha256 (input: string Option) =
        match input with
        | None -> empty
        | Some toHash ->
            use sha = SHA256.Create()
            let bytes = Encoding.UTF8.GetBytes toHash
            let hash = sha.ComputeHash bytes
            Convert.ToBase64String hash




//
//
// /// <summary>
// /// Extension methods for hashing strings
// /// </summary>
// public static class HashExtensions
// {
//     /// <summary>
//     /// Creates a SHA256 hash of the specified input.
//     /// </summary>
//     /// <param name="input">The input.</param>
//     /// <returns>A hash</returns>
//     public static string Sha256(this string input)
//     {
//         if (input.IsMissing()) return string.Empty;
//
//         using (var sha = SHA256.Create())
//         {
//             var bytes = Encoding.UTF8.GetBytes(input);
//             var hash = sha.ComputeHash(bytes);
//
//             return Convert.ToBase64String(hash);
//         }
//     }
//
//     /// <summary>
//     /// Creates a SHA256 hash of the specified input.
//     /// </summary>
//     /// <param name="input">The input.</param>
//     /// <returns>A hash.</returns>
//     public static byte[] Sha256(this byte[] input)
//     {
//         if (input == null)
//         {
//             return null;
//         }
//
//         using (var sha = SHA256.Create())
//         {
//             return sha.ComputeHash(input);
//         }
//     }
//
//     /// <summary>
//     /// Creates a SHA512 hash of the specified input.
//     /// </summary>
//     /// <param name="input">The input.</param>
//     /// <returns>A hash</returns>
//     public static string Sha512(this string input)
//     {
//         if (input.IsMissing()) return string.Empty;
//
//         using (var sha = SHA512.Create())
//         {
//             var bytes = Encoding.UTF8.GetBytes(input);
//             var hash = sha.ComputeHash(bytes);
//
//             return Convert.ToBase64String(hash);
//         }
//     }
// }
