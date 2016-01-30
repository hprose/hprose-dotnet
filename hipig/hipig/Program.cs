using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using System.CodeDom;
using System.IO;
using Hprose.Common;

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

            foreach (MethodInfo m in it.GetMethods()) {
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
                    if (attr is ResultModeAttribute ||
                        attr is SimpleModeAttribute ||
                        attr is ByRefAttribute ||
                        attr is MethodNameAttribute) {
                        n++;
                    }
                }
                CodeExpression[] attrs = new CodeExpression[n];
                n = 0;
                foreach (var attr in customAttrs) {
                    if (attr is ResultModeAttribute) {
                        attrs[n++] = new CodeObjectCreateExpression(typeof(ResultModeAttribute), new CodeSnippetExpression("HproseResultMode." + (attr as ResultModeAttribute).Value));
                    }
                    else if (attr is SimpleModeAttribute) {
                        attrs[n++] = new CodeObjectCreateExpression(typeof(SimpleModeAttribute), new CodePrimitiveExpression((attr as SimpleModeAttribute).Value));
                    }
                    else if (attr is ByRefAttribute) {
                        attrs[n++] = new CodeObjectCreateExpression(typeof(ByRefAttribute), new CodePrimitiveExpression((attr as ByRefAttribute).Value));
                    }
                    else if (attr is MethodNameAttribute) {
                        attrs[n++] = new CodeObjectCreateExpression(typeof(MethodNameAttribute), new CodePrimitiveExpression((attr as MethodNameAttribute).Value));
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
            return proxyClass;
        }
        public static void Main(string[] args) {

            if (args.Length == 0) {
                Console.WriteLine("Hprose Invocation Proxy Implementation Generator 1.0");
                Console.WriteLine("Copyright (c) 2008-2016 http://hprose.com");
                Console.WriteLine("hipig -i:IExample.cs -r:Hprose.Client.dll -o:Example.cs");
                Console.WriteLine("  -input:FILE1[,FILEn]  Input files (short: -i)");
                Console.WriteLine("  -output:FILE          Output files (short: -o)");
                Console.WriteLine("  -reference:A1[,An]    Imports metadata from the specified assembly (short: -r)");
                return;
            }
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");

            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            objCompilerParameters.OutputAssembly = "";

            string[] inputFiles = null;
            string output = "ServiceProxyImpl.cs";

            foreach (string a in args) {
                if (a.StartsWith("-r:") || a.StartsWith("-reference:")) {
                    string references = a.Substring(a.IndexOf(":") + 1);
                    if (references[0] == '"') references = references.Substring(1);
                    if (references[references.Length - 1] == '"') references = references.Substring(0, references.Length - 1);
                    objCompilerParameters.ReferencedAssemblies.AddRange(references.Split(','));
                }
                if (a.StartsWith("-i:") || a.StartsWith("-input:")) {
                    string input = a.Substring(a.IndexOf(":") + 1);
                    if (input[0] == '"') input = input.Substring(1);
                    if (input[input.Length - 1] == '"') input = input.Substring(0, input.Length - 1);
                    inputFiles = input.Split(',');
                }
                if (a.StartsWith("-o:") || a.StartsWith("-output:")) {
                    output = a.Substring(a.IndexOf(":") + 1);
                    if (output[0] == '"') output = output.Substring(1);
                    if (output[output.Length - 1] == '"') output = output.Substring(0, output.Length - 1);
                }
            }
            if (inputFiles == null) {
                inputFiles = new string[] { args[0] };
            }

            CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromFile(objCompilerParameters, inputFiles);
            String outputMessage = "";
            foreach (var item in cr.Output) {
                outputMessage += item + Environment.NewLine;
            }
            Console.WriteLine(outputMessage);
            if (cr.Errors.HasErrors) {
                Console.WriteLine("编译错误：");
                foreach (CompilerError err in cr.Errors) {
                    Console.WriteLine(err.ErrorText);
                }
            }
            else {
                Type[] types = cr.CompiledAssembly.GetTypes();

                CodeCompileUnit compunit = new CodeCompileUnit();
                CodeNamespace ns = new CodeNamespace(types[0].Namespace);
                compunit.Namespaces.Add(ns);
                ns.Imports.Add(new CodeNamespaceImport("System"));
                ns.Imports.Add(new CodeNamespaceImport("System.Reflection"));
                ns.Imports.Add(new CodeNamespaceImport("Hprose.Common"));
                foreach (Type t in types) {
                    if (t.IsInterface) {
                        ns.Types.Add(ImplementIt(t));
                    }
                }
                StringBuilder fileContent = new StringBuilder();
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                using (StringWriter sw = new StringWriter(fileContent)) {
                    objCSharpCodePrivoder.GenerateCodeFromNamespace(ns, sw, options);
                }
                File.WriteAllText(output, fileContent.ToString());

                string[] files = new string[inputFiles.Length + 1];
                inputFiles.CopyTo(files, 0);
                files[files.Length - 1] = output;

                objCompilerParameters.GenerateExecutable = false;
                objCompilerParameters.GenerateInMemory = true;
                objCompilerParameters.OutputAssembly = "";

                cr = objCSharpCodePrivoder.CompileAssemblyFromFile(objCompilerParameters, files);
                foreach (var item in cr.Output) {
                    outputMessage += item + Environment.NewLine;
                }
                Console.WriteLine(outputMessage);
                if (cr.Errors.HasErrors) {
                    Console.WriteLine("编译错误：");
                    foreach (CompilerError err in cr.Errors) {
                        Console.WriteLine(err.ErrorText);
                    }
                }
            }

        }
    }
}
