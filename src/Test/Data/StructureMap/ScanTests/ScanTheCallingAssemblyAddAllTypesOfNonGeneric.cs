﻿using StructureMap;
using TestApplication.Types;

namespace TestApplication.StructureMap.ScanTests
{
    public class ScanTheCallingAssemblyAddAllTypesOfNonGeneric
    {
        public ScanTheCallingAssemblyAddAllTypesOfNonGeneric()
        {
            var container = new Container(x => x.Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.AddAllTypesOf(typeof(ICommon));
            }));
        } 
    }
}