// --------------------------------------------------------------------------------------------------
//  <copyright file="RegistrationScope.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2012 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
namespace BodyshopWindows.Ioc
{
    public enum RegistrationScope
    {
        InstancePerDependency,

        LifetimeScope, 

        InstancePerThisContainer,
        
        Singleton
    }
}