// --------------------------------------------------------------------------------------------------
//  <copyright file="Bootstrapper.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2014 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public sealed class Bootstrapper : IDisposable
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// </summary>
        private static Bootstrapper bootstrapperSingleton;

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        private IocContainer container;

        private bool isStarted;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public Bootstrapper()
        {
            this.Modules = new List<IModule>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public static IResolver Container
        {
            get
            {
                return Instance.container;
            }
        }

        /// <summary>
        /// </summary>
        public static Bootstrapper Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return bootstrapperSingleton = bootstrapperSingleton ?? new Bootstrapper();
                }
            }
        }

        /// <summary>
        /// </summary>
        public List<IModule> Modules { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.container.Dispose();
        }

        /// <summary>
        /// </summary>
        public void Start()
        {
            if (this.isStarted)
            {
                return;
            }

            this.isStarted = true;
            var builder = new IocBuilder();
            foreach (var module in this.Modules)
            {
                module.Register(builder);
            }

            this.container = builder.Build();
        }

        #endregion
    }
}
