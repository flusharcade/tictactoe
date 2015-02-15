// --------------------------------------------------------------------------------------------------
//  <copyright file="IocContainer.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Custom Inversion of Control container that enables resolving by type and optionally by name.
    /// </summary>
    public sealed class IocContainer : IResolver
    {
        #region Fields

        /// <summary>
        /// List of things to dispose in this container
        /// </summary>
        private readonly List<IDisposable> disposables;

        /// <summary>
        /// Dictionary of factory functions to resolve with keyed by registered name and type
        /// </summary>
        private readonly Dictionary<Tuple<string, Type>, List<Func<IocContainer, object>>> getters;

        /// <summary>
        /// Dictionary of lifetimes
        /// </summary>
        private readonly Dictionary<Tuple<string, Type>, object> lifetimes;

        /// <summary>
        /// The parent IoC container (if any)
        /// </summary>
        private readonly IocContainer parentContainer;

        /// <summary>
        /// The list of singleton objects in the container (for the life of the container) keyed by registered name and type
        /// </summary>
        private readonly Dictionary<Tuple<string, Type>, object> singletons;

        private readonly List<Func<IocContainer, string, Type, object>> fallbacks;

        private readonly object lockObject = new object();
        
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the IocContainer class
        /// </summary>
        /// <param name="parentContainer">The parent container scope which will be resolved from if this container does not have the required type</param>
        /// <param name="registrations">The list of registrations for this container</param>
        /// <param name="fallbacks">The list of fallback functions to call to dynamically register types not found in the container</param>
        internal IocContainer(IocContainer parentContainer, IEnumerable<Registration> registrations, IEnumerable<Func<IocContainer, string, Type, object>> fallbacks)
        {
            this.parentContainer = parentContainer;
            if (parentContainer == null)
            {
                this.singletons = new Dictionary<Tuple<string, Type>, object>();
            }

            this.lifetimes = new Dictionary<Tuple<string, Type>, object>();
            this.getters = new Dictionary<Tuple<string, Type>, List<Func<IocContainer, object>>>();
            this.disposables = new List<IDisposable>();
            this.fallbacks = new List<Func<IocContainer, string, Type, object>>(fallbacks);

            this.Update(registrations);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new root lifetime scope
        /// </summary>
        /// <returns>Returns the <see cref="IocContainer"/> for the lifetime scope</returns>
        public IocContainer BeginLifetimeScope()
        {
            return this.BeginLifetimeScope(null);
        }

        /// <summary>
        /// Creates a new child lifetime scope
        /// </summary>
        /// <param name="builderCallback">The function to build the container (optional)</param>
        /// <returns>Returns the <see cref="IocContainer"/> for the lifetime scope</returns>
        public IocContainer BeginLifetimeScope(Action<IocBuilder> builderCallback)
        {
            var builder = new IocBuilder(this);
            if (builderCallback != null)
            {
                builderCallback(builder);
            }

            return builder.Build();
        }

        /// <summary>
        /// Disposes all resources that were resolved in the container
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }

            this.disposables.Clear();
        }

        /// <summary>
        /// Resolves an instance by name and type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T ResolveNamed<T>(string name)
        {
            return (T)this.ResolveNamed(name, typeof(T));
        }

        /// <summary>
        /// Enumerates the registered types by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<T> Enumerate<T>(string name, Type type)
        {
            var list = new List<T>();
            foreach (var func in this.GetGetter(name, type.GenericTypeArguments[0]))
            {
                list.Add((T)func(this));
            }

            return list;
        }

        /// <summary>
        /// Resolves an instance by name and type (non-generic)
        /// </summary>
        /// <param name="name">The registered name</param>
        /// <param name="type">The registered type</param>
        /// <returns></returns>
        public object ResolveNamed(string name, Type type)
        {
            var result = this.TryResolveNamed(name, type);
            if (result == null)
            {
                throw new InvalidOperationException(string.Format("Could not resolve named type {0}: {1}", name, type));
            }

            return result;
        }

        /// <summary>
        /// Resolves a dependency by type
        /// </summary>
        /// <typeparam name="T">The type of the dependency to be resolved</typeparam>
        /// <returns>Returns a new or pre-existing instance of the requested type.</returns>
        public T Resolve<T>() where T : class
        {
            return (T)this.Resolve(typeof(T));
        }

        /// <summary>
        /// Resolves a dependency by type
        /// </summary>
        /// <param name="type">The type of the dependency to be resolved</param>
        /// <returns>Returns a new or pre-existing instance of the requested type.</returns>
        public object Resolve(Type type)
        {
            return this.ResolveNamed(string.Empty, type);
        }

        public T TryResolve<T>()
        {
            return (T)this.TryResolve(typeof(T));
        }

        public object TryResolve(Type type)
        {
            return this.TryResolveNamed(string.Empty, type);
        }

        public T TryResolveNamed<T>(string name)
        {
            return (T)this.TryResolveNamed(name, typeof(T));
        }

        public object TryResolveNamed(string name, Type type)
        {
            var list = this.GetGetter(name, type);
            if (list.Count > 0)
            {
                return list[0](this);
            }

            var result = this.GetFallback(name, type);
            if (result != null)
            {
                return result;
            }

            if (type.GetTypeInfo().IsGenericType)
            {
                if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) && type.Name.StartsWith("IEnumerable"))
                {
                    // Special... 
                    // TODO: Perf?
                    // At a min, cache the get runtime method?
                    var ret = this.GetType().GetRuntimeMethod("Enumerate", new[] { typeof(string), typeof(Type) }).MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(this, new object[] { name, type });

                    // var ret = this.Enumerate(type.GetGenericArguments()[0]);
                    return ret;
                }

                if (type.Name.StartsWith("Func"))
                {
                    // TODO: This is quite heavy
                    var itemType = type.GenericTypeArguments[0];
                    list = this.GetGetter(name, itemType);
                    if (list.Count > 0)
                    {
                        var item = list[0];
                        var func = Expression.Lambda(typeof(Func<>).MakeGenericType(itemType), Expression.Convert(Expression.Invoke(Expression.Constant(item), Expression.Constant(this)), itemType)).Compile();

                        // Register ourselves for caching, not sure if worth it?
                        /*var builder = new IocBuilder();
                        builder.RegisterInstance(func).As(type);
                        builder.Update(this);*/
                        
                        return func;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Removes all registrations that have a particular name
        /// </summary>
        /// <param name="name">The name of the registrations to remove</param>
        public void RemoveRegistrationsWithName(string name)
        {
            foreach (var key in this.getters.Keys.Where(k => k.Item1 == name).ToList())
            {
                this.getters.Remove(key);
            }
        }

        public void Update(IEnumerable<Registration> registrations)
        {
            Contract.Requires(registrations != null);

            if (registrations == null)
            {
                throw new ArgumentNullException("registrations");
            }

            // TODO: Filter by BindingFlags.Instance | BindingFlags.NonPublic
            var setup = typeof(IocContainer).GetTypeInfo().GetDeclaredMethod("SetupType");
            foreach (var registration in registrations)
            {
                try
                {
                    setup.MakeGenericMethod(registration.RegistrationType).Invoke(this, new object[] { registration });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }

        #endregion

        #region Methods

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", Justification = "Reviewed. Suppression is OK here.")]
        private static Func<IocContainer, T> CreateFunc<T>(object[] injectedParameters)
        {
            var type = typeof(T);

            // Create from expression
            var constructor = type.GetTypeInfo()
                .DeclaredConstructors
                .Where(c => c.IsConstructor)
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            var parameter = Expression.Parameter(typeof(IocContainer), "container");

            var arguments = new List<Expression>();
            foreach (var arg in constructor.GetParameters())
            {
                bool inserted = false;
                foreach (var ip in injectedParameters)
                {
                    if (arg.ParameterType.GetTypeInfo().IsAssignableFrom(ip.GetType().GetTypeInfo()))
                    {
                        arguments.Add(Expression.Constant(ip));
                        inserted = true;
                    }
                }

                if (!inserted)
                {
                    // Call resolve from p
                    arguments.Add(Expression.Call(parameter, "Resolve", new[] { arg.ParameterType }));
                }
            }

            var expression = (Expression)Expression.New(constructor, arguments);            
            return Expression.Lambda<Func<IocContainer, T>>(expression, parameter).Compile();
        }

        private T AfterActivation<T>(T item, IEnumerable<Action<IocContainer, object>> after)
        {
            foreach (var a in after)
            {
                a(this, item);
            }

            return item;
        }

        private object Create<T>(Dictionary<Tuple<string, Type>, object> owner, IList<IDisposable> containerDisposables, string name, Func<IocContainer, T> createFunc, Collection<Action<IocContainer, object>> after = null)
        {
            object value;

            var tuple = new Tuple<string, Type>(name, typeof(T));
            if (!owner.TryGetValue(tuple, out value))
            {
                owner[tuple] = value = createFunc(this);
                if (after != null)
                {
                    this.AfterActivation(value, after);
                }

                var disposable = value as IDisposable;
                if (disposable != null)
                {
                    containerDisposables.Add(disposable);
                }
            }

            return value;
        }

        private object CreateLifetimeScope<T>(string name, Func<IocContainer, T> createFunc, Collection<Action<IocContainer, object>> after)
        {
            lock (this.lockObject)
            {
                return this.Create(this.lifetimes, this.disposables, name, createFunc, after);
            }
        }

        private object CreateInstancePerThisContainer<T>(Dictionary<Tuple<string, Type>, object> owner, IList<IDisposable> containerDisposables, string name, Func<IocContainer, T> createFunc, Collection<Action<IocContainer, object>> after)
        {
            lock (this.lockObject)
            {                
                return this.Create(owner, containerDisposables, name, createFunc, after);
            }
        }

        private object CreateSingleton<T>(string name, Func<IocContainer, T> createFunc, Collection<Action<IocContainer, object>> after)
        {
            lock (this.lockObject)
            {
                var root = this;
                while (root.parentContainer != null)
                {
                    root = root.parentContainer;
                }

                return root.Create(root.singletons, root.disposables, name, createFunc, after);
            }
        }

        private List<Func<IocContainer, object>> GetGetter(string name, Type type)
        {
            List<Func<IocContainer, object>> getter;
            var tuple = new Tuple<string, Type>(name, type);
            if (this.getters.TryGetValue(tuple, out getter))
            {
                return getter;
            }

            if (this.parentContainer == null)
            {
                return new List<Func<IocContainer, object>>();
            }

            return this.parentContainer.GetGetter(name, type);
        }

        private object GetFallback(string name, Type type)
        {
            foreach (var fallback in this.fallbacks)
            {
                var result = fallback(this, name, type);
                if (result != null)
                {
                    return result;
                }
            }

            if (this.parentContainer != null)
            {
                return this.parentContainer.GetFallback(name, type);
            }

            return null;
        }

        private void SetupType<T>(Registration registration)
        {
            var createFunc = (Func<IocContainer, T>)registration.CreateFunc
                             ?? CreateFunc<T>(registration.Parameters.ToArray());

            if (!registration.AsTypes.Any())
            {
                registration.AsTypes.Add(typeof(T));
            }

            var after = registration.ActivationList;
            if (!after.Any())
            {
                after = null;
            }

            foreach (var type in registration.AsTypes)
            {
                var tuple = new Tuple<string, Type>(registration.Name ?? string.Empty, type);

                if (this.getters.ContainsKey(tuple))
                {
                    if (!tuple.Item2.GetTypeInfo().IsInterface)
                    {
                        throw new InvalidOperationException(string.Format("Type {0} registered more than once", type));
                    }
                }

                List<Func<IocContainer, object>> list;
                if (!this.getters.TryGetValue(tuple, out list))
                {
                    this.getters[tuple] = list = new List<Func<IocContainer, object>>();
                }

                switch (registration.Scope)
                {
                    case RegistrationScope.Singleton:
                        if (this.parentContainer != null)
                        {
                            throw new NotSupportedException("Can't register singleton outside parent scope!");
                        }

                        list.Add(container => container.CreateSingleton(tuple.Item1, createFunc, after));
                        break;

                    case RegistrationScope.LifetimeScope:
                        list.Add(container => container.CreateLifetimeScope(tuple.Item1, createFunc, after));
                        break;

                    case RegistrationScope.InstancePerThisContainer:
                        list.Add(container => container.CreateInstancePerThisContainer(this.lifetimes, this.disposables, tuple.Item1, createFunc, after));
                        break;

                    case RegistrationScope.InstancePerDependency:
                        if (after != null)
                        {
                            list.Add(container => container.AfterActivation(createFunc(container), after));
                        }
                        else
                        {
                            list.Add(container => createFunc(container));
                        }

                        break;
                }
            }
        }        

        #endregion
    }
}