// --------------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Iris.Webservice
{
    /// <summary>
    /// Validation Result.
    /// </summary>
    public class ValidationResult
    {
        public ErrorResult Error { get; set; }

        public CustomerInfo Customer { get; set; }
    }
}