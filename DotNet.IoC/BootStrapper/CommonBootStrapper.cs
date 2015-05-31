﻿using Microsoft.Practices.ServiceLocation;

namespace DotNet.IoC
{
    public abstract class CommonBootStrapper
    {
        public static IServiceLocator ServiceLocator;

        protected CommonBootStrapper()
        {
            ServiceLocator = CreateServiceLocator();
        }

        public abstract IServiceLocator CreateServiceLocator();

        public static T GetInstance<T>()
        {
            return ServiceLocator.GetInstance<T>();
        }
    }
}
