// --------------------------------------------------------------------------
// <copyright file="ErrorResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Iris.Webservice
{
    /// <summary>
    /// Error Result.
    /// </summary>
    public class ErrorResult
    {
        public ErrorResult()
        {
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}