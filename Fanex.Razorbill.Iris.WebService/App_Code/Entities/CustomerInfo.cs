// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Iris.Webservice
{
    /// <summary>
    /// Customer info.
    /// </summary>
    public class CustomerInfo
    {
        public int RoleId { get; set; }

        public int CustId { get; set; }

        public int OnUserId { get; set; }

        public string Currency { get; set; }

        public string Language { get; set; }

        public string MasterUsername { get; set; }

        public int SubAccountId { get; set; }

        public string Site { get; set; }
    }
}