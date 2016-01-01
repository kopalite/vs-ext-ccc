﻿using CodeContracts.Contrib.Helpers;
using CodeContracts.Contrib.Rewriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeContracts.Contrib.Managers
{
    internal class ContractClassToProxyTransformer
    {
        /// <summary>
        /// Transforms code contract class (generated by "Create code contract" command) into contract proxy class,
        /// It is wrapper for interface instance based on proxy pattern and Contract.* statements in code contract class.
        /// Can be used for unit and integration test, injection wrappers - for DI containers etc.
        /// Please find more details in test files in unit tests project.
        /// </summary>
        /// <param name="rootNode">Contract class syntax node to be transformed to contract proxy class.</param>
        /// <param name="interfaceName">Name of the interface for which contract proxy class shoud be created.</param>
        /// <param name="proxyClassName">Name of the new generated contract proxy class.</param>
        /// <returns>C# code of generated contract proxy class.</returns>
        public string TransormToContractProxyClass(SyntaxNode rootNode, string interfaceName, string proxyClassName)
        {
            //Creating "public sealed class <interface-name>_proxy" class declaration.

            var classNode = new ProxyClassDeclarationExtender(proxyClassName, interfaceName).Visit(rootNode);

            //Adding 'using' statement for ContractProxyAttribute.

            classNode = new UsingStatementsExtender(IdentifiersHelper.Namespace_CodeContractsContrib).Visit(classNode);

            //Adding ContractProxyAttribute with interface name as parameter.

            classNode = new ClassDeclarationAttributeExtender(IdentifiersHelper.AttributeName_ContractProxy, interfaceName, true).Visit(classNode);

            //Adding internal field of interface type and constructor that initializes the field with injected value.

            classNode = new ProxyClassConstructorCreator(interfaceName).Visit(classNode);

            //TODO: In method/property declarations: replace Contract.Requires<T>() and Contract.Ensures<T>() statements with if statements that throw Exception(contract message)

            classNode = new ProxyClassContractConditionsModifier(IdentifiersHelper.ProxyContractFieldName).Visit(classNode);

            //Prettifying the code (indents, spaces etc)

            classNode = Formatter.Format(classNode, MSBuildWorkspace.Create());

            return classNode.Str();
        }
    }
}
