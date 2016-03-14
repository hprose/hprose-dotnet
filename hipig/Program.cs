using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using System.CodeDom;
using System.IO;
using Hprose.Common;
using Microsoft.VisualBasic;

namespace Hprose {
    class HIPIG {
        public static CodeTypeDeclaration ImplementIt(Type it) {
            string name = it.Name;
            if (name.Length > 1 && name[0] == 'I' && name[1] >= 'A' && name[1] <= 'Z') {
                name = name.Substring(1);
            }
            name += "Impl";
            CodeTypeDeclaration proxyClass = new CodeTypeDeclaration(name);
            proxyClass.BaseTypes.Add(typeof(HproseInvocationHandler));
            proxyClass.BaseTypes.Add(it);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add( new CodeParameterDeclarationExpression(typeof(HproseInvoker), "invoker") );
            constructor.Parameters.Add( new CodeParameterDeclarationExpression(typeof(String), "ns") );
            constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("invoker"));
            constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("ns"));
            proxyClass.Members.Add(constructor);

            CodeConstructor constructor2 = new CodeConstructor();
            constructor2.Attributes = MemberAttributes.Public;
            constructor2.Parameters.Add( new CodeParameterDeclarationExpression(typeof(HproseInvoker), "invoker") );
            constructor2.ChainedConstructorArgs.Add(new CodeVariableReferenceExpression("invoker"));
            constructor2.ChainedConstructorArgs.Add(new CodePrimitiveExpression(""));
            proxyClass.Members.Add(constructor2);

            List<Type> its = new List<Type>();
            its.AddRange(it.GetInterfaces());
            its.Add(it);
            foreach (Type t in its) {
                foreach (MethodInfo m in t.GetMethods(BindingFlags.Instance | BindingFlags.Public)) {
                    CodeMemberMethod method = new CodeMemberMethod();
                    method.Name = m.Name;
                    method.Attributes = MemberAttributes.Public;
                    ParameterInfo[] pis = m.GetParameters();
                    foreach (ParameterInfo p in pis) {
                        CodeParameterDeclarationExpression cpde = new CodeParameterDeclarationExpression(p.ParameterType, p.Name);
                        if (p.IsOut) cpde.Direction = FieldDirection.Out;
                        if (p.ParameterType.IsByRef) cpde.Direction = FieldDirection.Ref;
                        method.Parameters.Add(cpde);
                    }
                    method.ReturnType = new CodeTypeReference(m.ReturnType);

                    CodeExpression[] types = new CodeExpression[pis.Length];
                    for (int i = 0; i < pis.Length; i++) {
                        types[i] = new CodeTypeOfExpression(pis[i].ParameterType);
                    }

                    var customAttrs = m.GetCustomAttributes(true);
                    var n = 0;
                    foreach (var attr in customAttrs) {
                        string attrName = attr.GetType().Name;
                        if (attrName == "ResultModeAttribute" ||
                            attrName == "SimpleModeAttribute" ||
                            attrName == "ByRefAttribute" ||
                            attrName == "MethodNameAttribute") {
                            n++;
                        }
                    }
                    CodeExpression[] attrs = new CodeExpression[n];
                    n = 0;
                    foreach (var attr in customAttrs) {
                        string attrName = attr.GetType().Name;
                        if (attrName == "ResultModeAttribute") {
                            attrs[n++] = new CodeObjectCreateExpression(typeof(ResultModeAttribute), new CodeSnippetExpression("HproseResultMode." + attr.GetType().GetProperty("Value").GetValue(attr).ToString()));
                        }
                        else if (attrName == "SimpleModeAttribute") {
                            attrs[n++] = new CodeObjectCreateExpression(typeof(SimpleModeAttribute), new CodePrimitiveExpression(attr.GetType().GetProperty("Value").GetValue(attr)));
                        }
                        else if (attrName == "ByRefAttribute") {
                            attrs[n++] = new CodeObjectCreateExpression(typeof(ByRefAttribute), new CodePrimitiveExpression(attr.GetType().GetProperty("Value").GetValue(attr)));
                        }
                        else if (attrName == "MethodNameAttribute") {
                            attrs[n++] = new CodeObjectCreateExpression(typeof(MethodNameAttribute), new CodePrimitiveExpression(attr.GetType().GetProperty("Value").GetValue(attr)));
                        }
                    }

                    CodeExpression[] args = new CodeExpression[pis.Length];
                    for (int i = 0; i < pis.Length; i++) {
                        args[i] = new CodeVariableReferenceExpression(pis[i].Name);
                    }
                    if (m.ReturnType != typeof(void)) {
                        method.Statements.Add(new CodeMethodReturnStatement(new CodeCastExpression(method.ReturnType, new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "Invoke", new CodeExpression[] {
                            new CodeThisReferenceExpression(),
                            new CodePrimitiveExpression(method.Name),
                            (types.Length == 0) ? new CodeArrayCreateExpression(typeof(Type[]), 0) : new CodeArrayCreateExpression(typeof(Type[]), types),
                            new CodeTypeOfExpression(method.ReturnType),
                            (attrs.Length == 0) ? new CodeArrayCreateExpression(typeof(Attribute[]), 0) : new CodeArrayCreateExpression(typeof(Attribute[]), attrs),
                            (args.Length == 0) ? new CodeArrayCreateExpression(typeof(object[]), 0) : new CodeArrayCreateExpression(typeof(object[]), args)
                        }))));
                    }
                    else {
                        method.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "Invoke", new CodeExpression[] {
                            new CodeThisReferenceExpression(),
                            new CodePrimitiveExpression(method.Name),
                            (types.Length == 0) ? new CodeArrayCreateExpression(typeof(Type[]), 0) : new CodeArrayCreateExpression(typeof(Type[]), types),
                            new CodeTypeOfExpression(method.ReturnType),
                            (attrs.Length == 0) ? new CodeArrayCreateExpression(typeof(Attribute[]), 0) : new CodeArrayCreateExpression(typeof(Attribute[]), attrs),
                            (args.Length == 0) ? new CodeArrayCreateExpression(typeof(object[]), 0) : new CodeArrayCreateExpression(typeof(object[]), args)
                        }));
                    }
                    proxyClass.Members.Add(method);
                }
            }
            return proxyClass;
        }
        public static void Main(string[] args) {

            if (args.Length == 0) {
                Console.WriteLine("Hprose Invocation Proxy Implementation Generator 1.1");
                Console.WriteLine("Copyright (c) 2008-2016 http://hprose.com");
                Console.WriteLine("hipig -i:IExample -r:IExample.dll -o:Example.cs");
                Console.WriteLine("  -i:InterfaceName    Interface name");
                Console.WriteLine("  -o:OutputFileName   Output files");
                Console.WriteLine("  -r:Assembly         Imports metadata from the specified assembly");
                return;
            }
            string reference = "";
            string interfaceName = "";
            string output = "ServiceProxyImpl.cs";

            foreach (string a in args) {
                if (a.StartsWith("-r:")) {
                    reference = a.Substring(a.IndexOf(":") + 1);
                    if (reference[0] == '"') reference = reference.Substring(1);
                    if (reference[reference.Length - 1] == '"') reference = reference.Substring(0, reference.Length - 1);
                }
                if (a.StartsWith("-i:")) {
                    interfaceName = a.Substring(a.IndexOf(":") + 1);
                    if (interfaceName[0] == '"') interfaceName = interfaceName.Substring(1);
                    if (interfaceName[interfaceName.Length - 1] == '"') interfaceName = interfaceName.Substring(0, interfaceName.Length - 1);
                }
                if (a.StartsWith("-o:")) {
                    output = a.Substring(a.IndexOf(":") + 1);
                    if (output[0] == '"') output = output.Substring(1);
                    if (output[output.Length - 1] == '"') output = output.Substring(0, output.Length - 1);
                }
            }
            if (reference == "") {
                Console.WriteLine("You need to specify the assembly.");
                return;
            }

            Type[] types = Assembly.LoadFrom(reference).GetTypes();

            CodeCompileUnit compunit = new CodeCompileUnit();
            CodeNamespace ns = new CodeNamespace(types[0].Namespace);
            compunit.Namespaces.Add(ns);
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.Reflection"));
            ns.Imports.Add(new CodeNamespaceImport("Hprose.Common"));
            foreach (Type t in types) {
                if (t.IsInterface) {
                    if ((interfaceName == "") || (interfaceName == t.Name)) {
                        ns.Types.Add(ImplementIt(t));
                    }
                }
            }
            StringBuilder fileContent = new StringBuilder();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            CodeDomProvider codePrivoder;
            if (output.EndsWith(".cs")) {
                codePrivoder = CodeDomProvider.CreateProvider("CSharp");
            }
            else if (output.EndsWith(".vb")) {
                codePrivoder = CodeDomProvider.CreateProvider("VisualBasic");
            }
            else {
                Console.WriteLine("Output source file must have a .cs or .vb extension");
                return;
            }
            using (StringWriter sw = new StringWriter(fileContent)) {
                codePrivoder.GenerateCodeFromNamespace(ns, sw, options);
            }
            File.WriteAllText(output, fileContent.ToString());
        }
    }
}
