// --------------------------------------------------------------------------------------------------
//  <copyright file="IocBuilder.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;	

    public sealed class IocBuilder
    {
        #region Fields

        private readonly List<Registration> registrations = new List<Registration>();

        private readonly IocContainer rootContainer;

        private readonly List<Func<IocContainer, string, Type, object>> fallbacks = new List<Func<IocContainer, string, Type, object>>();

        #endregion

        #region Constructors and Destructors

        public IocBuilder()
        {
        }

        internal IocBuilder(IocContainer rootContainer)
        {
            this.rootContainer = rootContainer;
        }

        #endregion

        #region Public Methods and Operators

        public void AddFallback(Func<IocContainer, string, Type, object> fallback)
        {
            this.fallbacks.Add(fallback);
        }

        public void AddRegistration(Registration registration)
        {
            this.registrations.Add(registration);
        }

        public Registration AfterActivation<T>(Action<IocContainer, T> action)
        {
            var type = typeof(T);
            return this.registrations.Find(r => r.RegistrationType == type).AfterActivation(action);
        }

        public IocContainer Build()
        {
            return new IocContainer(this.rootContainer, this.registrations, this.fallbacks);
        }
       
        public Registration<T> Register<T>(Func<IocContainer, T> createFunc)
        {
            var registration = new Registration<T>(typeof(T), createFunc);
            this.registrations.Add(registration);
            return registration;
        }

        public Registration<T> RegisterInstance<T>(T value)
        {
            return this.Register(container => value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Type is used to identify what to register")]
        public Registration<T> RegisterType<T>()
        {
            var registration = new Registration<T>(typeof(T), null);
            this.registrations.Add(registration);
            return registration;
        }

        public Registration RegisterType(Type type)
        {
            var registration = new Registration(type, null);
            this.registrations.Add(registration);
            return registration;
        }

        public void Update(IocContainer container)
        {
            Contract.Requires(container != null);

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            container.Update(this.registrations);
        }

        #endregion
    }
}