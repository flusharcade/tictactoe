// --------------------------------------------------------------------------------------------------
//  <copyright file="IModule.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2014 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    /// <summary>
    /// </summary>
    public interface IModule
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="builder">
        /// </param>
        void Register(IocBuilder builder);

        #endregion
    }
}
