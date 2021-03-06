﻿namespace CodeContracts.Contrib.Helpers
{
    internal class IdentifiersHelper
    {
        public const string CodeContractClassFileSuffix = ".contract";
        public const string CodeContractClassNameSuffix = "_Contract";

        public const string ContractProxyClassFileSuffix = ".proxy";
        public const string ContractProxyClassNameSuffix = "_Proxy";

        public const string Namespace_System_Diagnostics_Contracts = "System.Diagnostics.Contracts";
        public const string Namespace_CodeContractsContrib = "CodeContracts.Contrib";

        public const string AttributeName_ContractClassFor = "ContractClassFor";
        public const string AttributeName_ContractClass = "ContractClass";
        public const string AttributeName_ContractProxy = "ContractProxy";

        public const string ProxyContractFieldName = "_contract";
        public const string ProxyContractCtorParamName = "contract";
        public const string ProxyContractRetVal = "retVal";

        public static string GetCodeContractClassFile(string interfaceName)
        {
            return interfaceName + CodeContractClassFileSuffix;
        }

        public static string GetCodeContractClassName(string interfaceName)
        {
            return interfaceName + CodeContractClassNameSuffix;
        }

        public static string GetContractProxyClassFile(string className)
        {
            return className.Replace(CodeContractClassFileSuffix, ContractProxyClassFileSuffix);
        }

        public static string GetContractProxyClassName(string className)
        {
            return className.Replace(CodeContractClassNameSuffix, ContractProxyClassNameSuffix);
        }
    }
}
