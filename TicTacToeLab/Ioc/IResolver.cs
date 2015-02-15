//--------------------------------------------------------------------------------------------------
// <copyright file="IResolver.cs" company="DNS Technology Pty Ltd.">
//   Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
// </copyright>
//--------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System;

    /// <summary>
    /// A dependency container or service locator able to resolve a dependency by type or by type and parameter argument
    /// </summary>
    /// <remarks>This should only be used in situations when the list of types to be resolved is not known at compile time.</remarks>
    public interface IResolver : IDisposable
    {
        /// <summary>
        /// Resolves a dependency by type
        /// </summary>
        /// <param name="type">The type of the dependency to be resolved</param>
        /// <returns>Returns a new or pre-existing instance of the requested type.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves a dependency by type
        /// </summary>
        /// <typeparam name="T">The type of the dependency to be resolved</typeparam>
        /// <returns>Returns a new or pre-existing instance of the requested type.</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolves a dependency by name
        /// </summary>
        /// <typeparam name="T">The type of the dependency to be resolved</typeparam>
        /// <param name="name">The name of the dependency</param>
        /// <returns>Returns a named instance of something.</returns>
        T ResolveNamed<T>(string name);
    }
}