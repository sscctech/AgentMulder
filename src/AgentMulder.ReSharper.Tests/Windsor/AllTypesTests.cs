using System.Linq;
using AgentMulder.Containers.CastleWindsor;
using AgentMulder.Containers.CastleWindsor.Providers;
using AgentMulder.ReSharper.Domain.Containers;
using AgentMulder.ReSharper.Tests.Windsor.Helpers;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using NUnit.Framework;

namespace AgentMulder.ReSharper.Tests.Windsor
{
    // todo these tests are currently duplicated, because of a bug in R# test runner involving abstract test fixtures and TestCases http://youtrack.jetbrains.com/issue/RSRP-299812

    [TestWindsor]
    public class AllTypesTests : ComponentRegistrationsTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return @"Windsor\TestCases"; }
        }

        protected override IContainerInfo ContainerInfo
        {
            get
            {
                return new WindsorContainerInfo(new[]
                {
                    new AllTypesRegistrationProvider(new BasedOnRegistrationProvider())
                });
            }
        }

        protected override string RelativeTypesPath
        {
            get { return @"..\..\Types"; }
        }

        [TestCase("FromTypesParams")]
        [TestCase("FromTypesNewArray")]
        [TestCase("FromTypesNewList")]
        [TestCase("FromAssemblyTypeOf")]
        [TestCase("FromAssemblyGetExecutingAssembly")]
        public void TestWithEmptyResult(string testName)
        {
            RunTest(testName, registrations => 
                Assert.AreEqual(0, registrations.Count()));
        }

        [TestCase("BasedOn\\FromTypesParamsBasedOnGeneric", new[] { "Foo.cs", "Baz.cs" })]
        [TestCase("BasedOn\\FromTypesNewListBasedOnGeneric", new[] { "Foo.cs", "Baz.cs" })]
        [TestCase("BasedOn\\FromTypesNewArrayBasedOnGeneric", new[] { "Foo.cs", "Baz.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyBasedOnGeneric", new[] { "Foo.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyBasedOnNonGeneric", new[] { "Foo.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyInNamespace", new[] { "InSomeNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyInNamespaceWithSubnamespaces", new[] { "InSomeNamespace.cs", "InSomeOtherNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyInSameNamespaceAsGeneric", new[] { "InSomeNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyInSameNamespaceAsGenericWithSubnamespaces", new[] { "InSomeNamespace.cs", "InSomeOtherNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyInSameNamespaceAsNonGeneric", new[] { "InSomeNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyInSameNamespaceAsNonGenericWithSubnamespaces", new[] { "InSomeNamespace.cs", "InSomeOtherNamespace.cs" })]
        [TestCase("BasedOn\\FromAssemblyTypeOfBasedOn", new[] { "Foo.cs" })]
        [TestCase("BasedOn\\FromAssemblyGetExecutingAssemblyBasedOn", new[] { "Foo.cs" })]
        [TestCase("BasedOn\\FromAssemblyContainingBasedOnGeneric", new[] { "Foo.cs" })]
        [TestCase("BasedOn\\FromAssemblyContainingBasedOnNonGeneric", new[] { "Foo.cs" })]
        [TestCase("BasedOn\\FromTypesParamsPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs" })]
        [TestCase("BasedOn\\FromTypesNewListPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs" })]
        [TestCase("BasedOn\\FromTypesNewArrayPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs" })]
        [TestCase("BasedOn\\FromAssemblyContainingPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs", "FooBar.cs" })]
        [TestCase("BasedOn\\FromAssemblyGetExecutingAssemblyPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs", "FooBar.cs" })]
        [TestCase("BasedOn\\FromAssemblyNamedPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs", "FooBar.cs" })]
        [TestCase("BasedOn\\FromAssemblyTypeOfPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs", "FooBar.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyPick", new[] { "Foo.cs", "Bar.cs", "Baz.cs", "FooBar.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentIsInInamespace", new[] { "InSomeNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentIsInInamespaceWithSubnamespaces", new[] { "InSomeNamespace.cs", "InSomeOtherNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentHasAttributeMethodGroup", new[] { "HaveAttribute.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentIsInSameInamespaceAsGeneric", new[] { "InSomeNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentIsInSameInamespaceAsGenericWithSubnamespaces", new[] { "InSomeNamespace.cs", "InSomeOtherNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentIsInSameInamespaceAsNonGeneric", new[] { "InSomeNamespace.cs" })]
        [TestCase("BasedOn\\FromThisAssemblyWhereComponentIsInSameInamespaceAsNonGenericWithSubnamespaces", new[] { "InSomeNamespace.cs", "InSomeOtherNamespace.cs" })]
        public void TestWithRegistrations(string testName, params string[] fileNames)
        {
            RunTest(testName, registrations =>
            {
                ICSharpFile[] codeFiles = fileNames.Select(GetCodeFile).ToArray();

                Assert.AreEqual(1, registrations.Count());
                foreach (var codeFile in codeFiles)
                {
                    codeFile.ProcessChildren<ITypeDeclaration>(declaration =>
                        Assert.That(registrations.Any((r => r.Registration.IsSatisfiedBy(declaration.DeclaredElement)))));    
                }
            });
        }

        [TestCase("BasedOn\\FromAssemblyContainingBasedOnGeneric", new[] { "IFoo.cs" })]
        public void ExcludeTest(string testName, string[] fileNamesToExclude)
        {
            RunTest(testName, registrations =>
            {
                ICSharpFile[] codeFiles = fileNamesToExclude.Select(GetCodeFile).ToArray();

                CollectionAssert.IsNotEmpty(registrations);
                foreach (var codeFile in codeFiles)
                {
                    codeFile.ProcessChildren<ITypeDeclaration>(declaration =>
                        Assert.That(registrations.All((r => !r.Registration.IsSatisfiedBy(declaration.DeclaredElement)))));
                }
            });
        }
    }
}