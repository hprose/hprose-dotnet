using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using Hprose.Common;
using NLog;
using ServiceStack;

namespace hipig
{
    internal class Program
    {
        private static readonly Logger __logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            args = new[] {"-r"}.Union(args).ToArray();
            var result = Parser.Default.ParseArguments<Options>(args);
            var mapResult = result.MapResult(
                options =>
                {
                    var json = options.ToJson();
                    __logger.Trace(json);
                    return BuildProxy(options);
                },
                errors =>
                {
                    __logger.Error(errors);
                    return false;
                });
            if (!mapResult)
            {
                Console.ReadKey(true);
            }
        }

        private static bool BuildProxy(Options options)
        {
            if (string.IsNullOrEmpty(options.OutputFileName))
            {
                const string fileNameExtension = ".cs";
                options.OutputFileName = Path.GetFileNameWithoutExtension(options.Assembly) + fileNameExtension;
            }

            var codeDomProvider = CodeDomProvider.CreateProvider(
                Equals(
                    Path.GetExtension(options.OutputFileName), ".vb")
                    ? "VisualBasic"
                    : "CSharp");

            var interfaces = Assembly
                .LoadFrom(options.Assembly)
                .GetTypes()
                .Where(type => type.IsInterface)
                .ToArray();
            if (options.InterfacesName.Any())
            {
                interfaces = interfaces
                    .Where(type =>
                        options.InterfacesName.Contains(type.Name)
                        || options.InterfacesName.Contains(type.FullName))
                    .ToArray();
            }

            var codeCompileUnit = new CodeCompileUnit();
            var codeNamespace = new CodeNamespace(options.NameSpace);
            codeCompileUnit.Namespaces.Add(codeNamespace);
            foreach (var type in interfaces)
            {
                codeNamespace.Types.Add(ImplementInterface(type, options));
            }

            var codeGeneratorOptions = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };
            using (TextWriter textWriter = new StreamWriter(options.OutputFileName, false, Encoding.UTF8))
            {
                codeDomProvider.GenerateCodeFromNamespace(codeNamespace, textWriter, codeGeneratorOptions);
            }
            return true;
        }

        public static CodeTypeDeclaration ImplementInterface(Type @interface, Options options)
        {
            var proxyName = @interface.Name;
            if (proxyName.Length > 1 && proxyName[0] == 'I' && proxyName[1] >= 'A' && proxyName[1] <= 'Z')
            {
                proxyName = proxyName.Substring(1);
            }
            proxyName += options.ClassTail;

            var proxyClass = new CodeTypeDeclaration(proxyName);
            proxyClass.BaseTypes.Add(typeof (HproseInvocationHandler));
            proxyClass.BaseTypes.Add(@interface);

            const string invoker = nameof(invoker);
            const string ns = nameof(ns);

            var constructor1 = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };
            constructor1.Parameters.Add(new CodeParameterDeclarationExpression(typeof (HproseInvoker), invoker));
            constructor1.Parameters.Add(new CodeParameterDeclarationExpression(typeof (string), ns));
            constructor1.BaseConstructorArgs.Add(new CodeVariableReferenceExpression(invoker));
            constructor1.BaseConstructorArgs.Add(new CodeVariableReferenceExpression(ns));
            proxyClass.Members.Add(constructor1);

            var constructor2 = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };
            constructor2.Parameters.Add(new CodeParameterDeclarationExpression(typeof (HproseInvoker), invoker));
            constructor2.ChainedConstructorArgs.Add(new CodeVariableReferenceExpression(invoker));
            constructor2.ChainedConstructorArgs.Add(new CodePrimitiveExpression(string.Empty));
            proxyClass.Members.Add(constructor2);

            var interfaceTypes = new List<Type>();
            interfaceTypes.AddRange(@interface.GetInterfaces());
            interfaceTypes.Add(@interface);
            var methodHashs = new HashSet<string>();
            foreach (var type in interfaceTypes)
            {
                foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                {
                    var methodHashBuilder = new StringBuilder(methodInfo.Name);
                    var parameterInfos = methodInfo.GetParameters();
                    var parameterTypes = new CodeExpression[parameterInfos.Length];
                    for (var index = 0; index < parameterInfos.Length; index++)
                    {
                        methodHashBuilder.AppendFormat("_{0}", parameterInfos[index].ParameterType.FullName);
                        parameterTypes[index] = new CodeTypeOfExpression(parameterInfos[index].ParameterType);
                    }
                    var methodHash = methodHashBuilder.ToString();
                    if (methodHashs.Contains(methodHash))
                    {
                        continue;
                    }
                    methodHashs.Add(methodHash);

                    var method = new CodeMemberMethod
                    {
                        Name = methodInfo.Name,
                        Attributes = MemberAttributes.Public
                    };
                    foreach (var parameterInfo in parameterInfos)
                    {
                        var codeParameterDeclarationExpression =
                            new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name);
                        if (parameterInfo.IsOut)
                        {
                            codeParameterDeclarationExpression.Direction = FieldDirection.Out;
                        }
                        if (parameterInfo.ParameterType.IsByRef)
                        {
                            codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
                        }
                        method.Parameters.Add(codeParameterDeclarationExpression);
                    }
                    method.ReturnType = new CodeTypeReference(methodInfo.ReturnType);

                    var hproseAttributes = new List<CodeExpression>();
                    foreach (var customAttribute in methodInfo
                        .GetCustomAttributes(true)
                        .Where(attr =>
                            attr is ResultModeAttribute
                            || attr is SimpleModeAttribute
                            || attr is ByRefAttribute
                            || attr is MethodNameAttribute)
                        .OfType<Attribute>())
                    {
                        var resultModeAttribute = customAttribute as ResultModeAttribute;
                        if (null != resultModeAttribute)
                        {
                            hproseAttributes.Add(new CodeObjectCreateExpression(
                                typeof (ResultModeAttribute),
                                new CodePrimitiveExpression(resultModeAttribute.Value)));
                            continue;
                        }
                        var simpleModeAttribute = customAttribute as SimpleModeAttribute;
                        if (null != simpleModeAttribute)
                        {
                            hproseAttributes.Add(new CodeObjectCreateExpression(
                                typeof (SimpleModeAttribute),
                                new CodePrimitiveExpression(simpleModeAttribute.Value)));
                            continue;
                        }
                        var byRefAttribute = customAttribute as ByRefAttribute;
                        if (null != byRefAttribute)
                        {
                            hproseAttributes.Add(new CodeObjectCreateExpression(
                                typeof (ByRefAttribute),
                                new CodePrimitiveExpression(byRefAttribute.Value)));
                            continue;
                        }
                        var methodNameAttribute = customAttribute as MethodNameAttribute;
                        if (null != methodNameAttribute)
                        {
                            hproseAttributes.Add(new CodeObjectCreateExpression(
                                typeof (MethodNameAttribute),
                                new CodePrimitiveExpression(methodNameAttribute.Value)));
                        }
                    }

                    var parameterValues = new CodeExpression[parameterInfos.Length];
                    for (var i = 0; i < parameterInfos.Length; i++)
                    {
                        parameterValues[i] = new CodeVariableReferenceExpression(parameterInfos[i].Name);
                    }

                    CodeExpression[] codeExpressions =
                    {
                        new CodeThisReferenceExpression(),
                        new CodePrimitiveExpression(method.Name),
                        parameterTypes.Any()
                            ? new CodeArrayCreateExpression(typeof (Type[]), parameterTypes)
                            : new CodeArrayCreateExpression(typeof (Type[]), 0),
                        new CodeTypeOfExpression(method.ReturnType),
                        hproseAttributes.Any()
                            ? new CodeArrayCreateExpression(typeof (Attribute[]), hproseAttributes.ToArray())
                            : new CodeArrayCreateExpression(typeof (Attribute[]), 0),
                        parameterValues.Any()
                            ? new CodeArrayCreateExpression(typeof (object[]), parameterValues)
                            : new CodeArrayCreateExpression(typeof (object[]), 0)
                    };
                    if (methodInfo.ReturnType != typeof (void))
                    {
                        method.Statements.Add(
                            new CodeMethodReturnStatement(
                                new CodeCastExpression(
                                    method.ReturnType,
                                    new CodeMethodInvokeExpression(
                                        new CodeThisReferenceExpression(),
                                        nameof(HproseInvocationHandler.Invoke),
                                        codeExpressions
                                        )
                                    )
                                )
                            );
                    }
                    else
                    {
                        method.Statements.Add(
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                nameof(HproseInvocationHandler.Invoke),
                                codeExpressions
                                )
                            );
                    }
                    proxyClass.Members.Add(method);
                }
            }
            return proxyClass;
        }
    }
}