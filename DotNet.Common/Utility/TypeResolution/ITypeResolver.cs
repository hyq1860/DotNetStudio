/*****************************************************************
 * Copyright (C) 2005-2006 Newegg Corporation
 * All rights reserved.
 * 
 * Author:   Allen Wang (Allen.G.Wang@newegg.com)
 * Create Date:  06/30/2009 15:12:41
 * Description:
 *
 * RevisionHistory
 * Date         Author               Description
 * 
*****************************************************************/
using System;

namespace DotNet.Common.Utility.TypeResolution
{
    /// <summary>
    /// Resolves a <see cref="System.Type"/> by name.
    /// </summary>
    /// <remarks>
    /// <p>
    /// The rationale behind the creation of this interface is to centralise
    /// the resolution of type names to <see cref="System.Type"/> instances
    /// beyond that offered by the plain vanilla
    /// <see cref="System.Type.GetType(string)"/> method call.
    /// </p>
    /// </remarks>
    public interface ITypeResolver
    {
        /// <summary>
        /// Resolves the supplied <paramref name="typeName"/> to a
        /// <see cref="System.Type"/>
        /// instance.
        /// </summary>
        /// <param name="typeName">
        /// The (possibly partially assembly qualified) name of a
        /// <see cref="System.Type"/>.
        /// </param>
        /// <returns>
        /// A resolved <see cref="System.Type"/> instance.
        /// </returns>
        /// <exception cref="System.TypeLoadException">
        /// If the supplied <paramref name="typeName"/> could not be resolved
        /// to a <see cref="System.Type"/>.
        /// </exception>
        Type Resolve(string typeName);
    }
}