// --------------------------------------------------------------------------------------------------
//  <copyright file="Registration{T}.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    using System;

    public sealed class Registration<T> : Registration
    {
        public Registration(Type type, object createFunc)
            : base(type, createFunc)
        {
        }

        public Registration<T> AfterActivation(Action<IocContainer, T> action)
        {
            this.ActivationList.Add((c, a) => action(c, (T)a));

            return this;
        }
    }    
}