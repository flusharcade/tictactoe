// --------------------------------------------------------------------------------------------------
//  <copyright file="GroupRegistration.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Registration of multiple items.
    /// Each registration is configured in the same manner.
    /// </summary>
    public sealed class GroupRegistration
    {
        #region Fields

        /// <summary>
        /// The list of registrations
        /// </summary>
        private readonly List<Registration> registrations;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="GroupRegistration"/> class. </summary>
        /// <param name="registrations">The list of registrations to manage</param>
        public GroupRegistration(IEnumerable<Registration> registrations)
        {
            this.registrations = registrations.ToList();
        }

        #endregion
    }
}