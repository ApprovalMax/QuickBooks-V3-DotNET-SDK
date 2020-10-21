﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Modified for Intuit's Oauth2 implementation
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Intuit.Ipp.OAuth2PlatformClient
{

    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion
    }

    /// <summary>
    /// OidcScopes enum
    /// </summary>
    public enum OidcScopes:int
    {
        /// <summary>
        /// openid
        /// </summary>
        [StringValue("openid")]
        OpenId = 1,

        /// <summary>
        /// email
        /// </summary>
        [StringValue("email")]
        Email = 2,

        /// <summary>
        /// profile
        /// </summary>
        [StringValue("profile")]
        Profile = 3,

        /// <summary>
        /// phone
        /// </summary>
        [StringValue("phone")]
        Phone = 4,

        /// <summary>
        /// address
        /// </summary>
        [StringValue("address")]
        Address = 5,

        /// <summary>
        /// com.intuit.quickbooks.accounting
        /// </summary>
        [StringValue("com.intuit.quickbooks.accounting")]
        Accounting = 6,

        /// <summary>
        /// com.intuit.quickbooks.payment
        /// </summary>
        [StringValue("com.intuit.quickbooks.payment")]
        Payment = 7,

        /// <summary>
        /// com.intuit.quickbooks.payroll
        /// </summary>
        [StringValue("com.intuit.quickbooks.payroll")]
        Payroll = 8,

        /// <summary>
        /// com.intuit.quickbooks.payroll.timetracking
        /// </summary>
        [StringValue("com.intuit.quickbooks.payroll.timetracking")]
        PayrollTimetracking = 9,

        /// <summary>
        /// com.intuit.quickbooks.payroll.benefits
        /// </summary>
        [StringValue("com.intuit.quickbooks.payroll.benefits")]
        PayrollBenefits = 10,

        /// <summary>
        /// intuit_name
        /// </summary>
        [StringValue("intuit_name")]
        IntuitName = 11

    }
}
