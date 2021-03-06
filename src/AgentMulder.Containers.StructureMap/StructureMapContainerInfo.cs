﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using AgentMulder.ReSharper.Domain.Containers;

namespace AgentMulder.Containers.StructureMap
{
    [Export(typeof(IContainerInfo))]
    public class StructureMapContainerInfo : ContainerInfoBase
    {
        public override string ContainerDisplayName
        {
            get { return "StructureMap"; }
        }

        public override IEnumerable<string> ContainerQualifiedNames
        {
            get { yield return "StructureMap"; }
        }

        protected override ComposablePartCatalog GetComponentCatalog()
        {
            return new AssemblyCatalog(Assembly.GetExecutingAssembly());
        }
    }
}