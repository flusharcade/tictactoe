// --------------------------------------------------------------------------------------------------
//  <copyright file="Registration.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    /// <summary>
    /// Represents a registration of a type for the ioc container
    /// The registration closely mirrors <c>Autofac's</c> to allow easy switching between the two.
    /// </summary>
    public class Registration
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="Registration"/> class.</summary>
        /// <param name="type">The object type to register.</param>
        /// <param name="createFunc">The creation function used to generate new instances of type. Passed as an object to allow for a non typed Registration class.</param>
        public Registration(Type type, object createFunc)
        {
            this.AsTypes = new Collection<Type>();
            this.ActivationList = new Collection<Action<IocContainer, object>>();
            this.Parameters = new List<object>();
            this.RegistrationType = type;
            this.CreateFunc = createFunc;
        }

        #endregion

        #region Public Properties

        public List<object> Parameters { get; private set; }

        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of types that may be resolved to return this registered types.
        /// AsTypes should all be base classes/interfaces of the created object
        /// </summary>
        public Collection<Type> AsTypes { get; private set; }
        
        /// <summary>
        /// Gets the create function used to generate a new instance of the type.
        /// </summary>
        public object CreateFunc { get; private set; }

        /// <summary>
        /// Gets or sets the scope that objects are created in.
        /// </summary>
        public RegistrationScope Scope { get; set; }

        /// <summary>
        /// Gets the type of the object created by the registration.
        /// The <c>CreateFunc</c> method must return this type.
        /// </summary>
        public Type RegistrationType { get; private set; }

        public Collection<Action<IocContainer, object>> ActivationList { get; private set; }

        #endregion

        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Type is used to identify what to register as")]
        public Registration As<TAs>()
        {
            return this.Named(string.Empty, typeof(TAs));            
        }

        public Registration As(Type type)
        {
            return this.Named(string.Empty, type);            
        }

        public Registration AsSelf()
        {
            return this.Named(string.Empty, this.RegistrationType);
        }

        public Registration AsImplementedInterfaces()
        {
            foreach (var iface in this.RegistrationType.GetTypeInfo().ImplementedInterfaces)
            {
                this.Named(string.Empty, iface);
            }

            return this;
        }

        public Registration InstancePerDependency()
        {
            this.Scope = RegistrationScope.InstancePerDependency;
            return this;
        }

        public Registration InstancePerLifetimeScope()
        {
            this.Scope = RegistrationScope.LifetimeScope;
            return this;
        }

        public Registration InstancePerThisContainer()
        {
            this.Scope = RegistrationScope.InstancePerThisContainer;
            return this;
        }

        public Registration SingleInstance()
        {
            this.Scope = RegistrationScope.Singleton;
            return this;
        }

        public Registration Named(string name, Type type)
        {
            if (this.Name != null && this.Name != name)
            {
                throw new NotSupportedException("A registration must use only one name");
            }

            Contract.Requires(type.GetTypeInfo().IsAssignableFrom(this.RegistrationType.GetTypeInfo()));

            if (!type.GetTypeInfo().IsAssignableFrom(this.RegistrationType.GetTypeInfo()))
            {
                throw new InvalidOperationException(
                    string.Format("Type {0} is not assignable from {1}", type, this.RegistrationType));
            }
            
            this.Name = name;
            this.AsTypes.Add(type);
            return this;
        }

        public Registration Named<TAs>(string name)
        {
            return this.Named(name, typeof(TAs));
        }

        public Registration WithParameters(params object[] parameters)
        {
            this.Parameters.AddRange(parameters);
            return this;
        }

        public Registration AfterActivation<T>(Action<IocContainer, T> action)
        {
            this.ActivationList.Add((c, a) => action(c, (T)a));
            return this;
        }

        #endregion
    }
}