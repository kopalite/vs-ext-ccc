﻿using CodeContracts.Contrib.Managers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace CodeContracts.Contrib.Test
{
    [TestClass]
    public class InterfaceCCAdapter_Test
    {
        [TestMethod]
        public void GetAddaptedInterfaceForCC_Test()
        {
            //Arrange

            string sourceCode = null;
            using (var streamReader = new StreamReader("IInterfaceInput.txt", Encoding.UTF8))
            {
                sourceCode = streamReader.ReadToEnd();
            }
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var rootNode = syntaxTree.GetRoot();

            //Act

            var adapter = new InterfaceToContractClassAdapter();
            var actual = adapter.AdaptInterfaceForContractClass(rootNode, "IInterfaceInput_Contract");

            //Assert

            var expected = File.ReadAllText("IInterfaceInput_Adapted.txt");
            Assert.AreEqual(expected, actual);
        }
    }
}
