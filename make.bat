@echo off

if not exist dist mkdir dist
if not exist dist\1.0 mkdir dist\1.0
if not exist dist\1.1 mkdir dist\1.1
if not exist dist\2.0 mkdir dist\2.0
if not exist dist\3.5 mkdir dist\3.5
if not exist dist\3.5\ClientProfile mkdir dist\3.5\ClientProfile
if not exist dist\4.0 mkdir dist\4.0
if not exist dist\4.0\ClientProfile mkdir dist\4.0\ClientProfile
if not exist dist\4.5 mkdir dist\4.5
if not exist dist\4.5\Core mkdir dist\4.5\Core
if not exist dist\4.5.1 mkdir dist\4.5.1
if not exist dist\WindowsPhone mkdir dist\WindowsPhone
if not exist dist\WindowsPhone71 mkdir dist\WindowsPhone71
if not exist dist\WindowsPhone8 mkdir dist\WindowsPhone8
if not exist dist\SilverLight2 mkdir dist\SilverLight2
if not exist dist\SilverLight3 mkdir dist\SilverLight3
if not exist dist\SilverLight4 mkdir dist\SilverLight4
if not exist dist\SilverLight5 mkdir dist\SilverLight5
if not exist dist\CF1.0 mkdir dist\CF1.0
if not exist dist\CF2.0 mkdir dist\CF2.0
if not exist dist\CF3.5 mkdir dist\CF3.5
if not exist dist\Mono mkdir dist\Mono
if not exist dist\Mono2 mkdir dist\Mono2
if not exist dist\Mono4 mkdir dist\Mono4
if not exist dist\Mono4.5 mkdir dist\Mono4.5
if not exist dist\Unity mkdir dist\Unity

set SL2_PATH=C:\Program Files\Microsoft SDKs\Silverlight\v2.0\Reference Assemblies
set SL3_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\Silverlight\v3.0
set SL4_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0
set SL5_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\Silverlight\v5.0
set WP70_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone
set WP71_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone71
set WP80_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\WindowsPhone\v8.0
set CF_PATH=C:\Program Files\Microsoft.NET\SDK\CompactFramework
set MONO_PATH=C:\Program Files\Mono-3.2.3\bin
set UNITY_PATH=C:\Program Files\Unity\Editor\Data\MonoBleedingEdge\bin

if DEFINED ProgramFiles(x86) set SL2_PATH=C:\Program Files (x86)\Microsoft SDKs\Silverlight\v2.0\Reference Assemblies
if DEFINED ProgramFiles(x86) set SL3_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v3.0
if DEFINED ProgramFiles(x86) set SL4_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0
if DEFINED ProgramFiles(x86) set SL5_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v5.0
if DEFINED ProgramFiles(x86) set WP70_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone
if DEFINED ProgramFiles(x86) set WP71_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone71
if DEFINED ProgramFiles(x86) set WP80_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\WindowsPhone\v8.0
if DEFINED ProgramFiles(x86) set CF_PATH=C:\Program Files (x86)\Microsoft.NET\SDK\CompactFramework
if DEFINED ProgramFiles(x86) set MONO_PATH=C:\Program Files (x86)\Mono-3.2.3\bin
if DEFINED ProgramFiles(x86) set UNITY_PATH=C:\Program Files (x86)\Unity\Editor\Data\MonoBleedingEdge\bin

set NUMERICS_SRC=

set NUMERICS_SRC=%NUMERICS_SRC% src\System\Numerics\BigInteger.cs
set NUMERICS_SRC=%NUMERICS_SRC% src\System\Numerics\BigIntegerBuilder.cs
set NUMERICS_SRC=%NUMERICS_SRC% src\System\Numerics\BigNumber.cs
set NUMERICS_SRC=%NUMERICS_SRC% src\System\Numerics\NumericsHelpers.cs
set NUMERICS_SRC=%NUMERICS_SRC% src\System\Numerics\Complex.cs
set NUMERICS_SRC=%NUMERICS_SRC% src\System\Numerics\DoubleUlong.cs

set HPROSE_SRC=
set HPROSE_SRC=%HPROSE_SRC% src\System\Action.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Func.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\NotImplementedException.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\SerializableAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\MissingMethodException.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Collections\HashMap.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Collections\Generic\HashMap.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\InvalidDataException.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\BlockType.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\CompressionMode.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\DecodeHelper.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\DeflateInput.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\Deflater.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\DeflateStream.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\DeflateStreamAsyncResult.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\FastEncoder.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\FastEncoderStatics.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\FastEncoderWindow.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\GZipDecoder.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\GZIPHeaderState.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\GZipStream.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\HuffmanTree.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\Inflater.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\InflaterState.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\InputBuffer.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\Match.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\MatchState.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\IO\Compression\OutputWindow.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Threading\ReaderWriterLock.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Threading\SynchronizationContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Windows\Forms\WindowsFormsSynchronizationContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\Extension.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseException.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseCallback.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseMethod.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseResultMode.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseInvoker.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseInvocationHandler.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\IHproseFilter.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\InvokeHelper.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\Proxy.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\IInvocationHandler.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\CtorAccessor.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\PropertyAccessor.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\TypeEnum.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\ClassManager.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\ObjectSerializer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\ObjectUnserializer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseFormatter.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseHelper.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseMode.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseReader.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseTags.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseWriter.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\CookieManager.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\HproseClient.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\HproseHttpClient.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseService.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseServiceEvent.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpService.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerService.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerServer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\AssemblyInfo.cs

set HPROSE_INFO= src\AssemblyInfo.cs

echo start compile hprose for .NET 1.0
C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705\Csc.exe -out:dist\1.0\Hprose.Client.dll -define:dotNET10;ClientOnly -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705\Csc.exe -out:dist\1.0\Hprose.dll -define:dotNET10 -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 1.1
c:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\Csc.exe -out:dist\1.1\Hprose.Client.dll -define:dotNET11;ClientOnly -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
c:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\Csc.exe -out:dist\1.1\Hprose.dll -define:dotNET11 -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 2.0
c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe -keyfile:HproseKeys.snk -out:dist\2.0\Hprose.Client.dll -define:dotNET2;ClientOnly -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe -keyfile:HproseKeys.snk -out:dist\2.0\Hprose.dll -define:dotNET2 -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 3.5
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\3.5\Hprose.Client.dll -define:dotNET35;ClientOnly -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\3.5\Hprose.dll -define:dotNET35 -filealign:512 -target:library -optimize+ -debug- %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 3.5 ClientProfile
set DOTNET_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v3.5\Profile\Client
if DEFINED ProgramFiles(x86) set DOTNET_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v3.5\Profile\Client
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\3.5\ClientProfile\Hprose.Client.dll -define:dotNET35;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\3.5\ClientProfile\Hprose.dll -define:dotNET35;ClientProfile -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.0
set DOTNET_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0
if DEFINED ProgramFiles(x86) set DOTNET_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.0\Hprose.Client.dll -define:dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.0\Hprose.dll -define:dotNET4; -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.0 ClientProfile
set DOTNET_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client
if DEFINED ProgramFiles(x86) set DOTNET_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.0\ClientProfile\Hprose.Client.dll -define:dotNET4;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.0\ClientProfile\Hprose.dll -define:dotNET4;ClientProfile -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5
set DOTNET_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5
if DEFINED ProgramFiles(x86) set DOTNET_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.5\Hprose.Client.dll -define:dotNET4;dotNET45;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.5\Hprose.dll -define:dotNET4;dotNET45 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5 Windows Store App
set DOTNET_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5
if DEFINED ProgramFiles(x86) set DOTNET_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Collections.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.IO.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Linq.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Net.Requests.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Net.Primitives.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Reflection.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Reflection.Extensions.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Reflection.Primitives.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Extensions.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Numerics.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.Primitives.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Text.Encoding.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Text.Encoding.Extensions.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Threading.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.5\Core\Hprose.Client.dll -define:dotNET4;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.1
set DOTNET_PATH=C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1
if DEFINED ProgramFiles(x86) set DOTNET_PATH=C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.5.1\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\4.5.1\Hprose.dll -define:dotNET4;dotNET45;dotNET451 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug- %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 2.0
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe -keyfile:HproseKeys.snk -out:dist\SilverLight2\Hprose.Client.dll -define:SILVERLIGHT;SL2;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug- %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 3.0
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\SilverLight3\Hprose.Client.dll -define:SILVERLIGHT;SL3;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug- %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 4.0
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\SilverLight4\Hprose.Client.dll -define:SILVERLIGHT;SL4;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug- %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 5.0
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\SilverLight5\Hprose.Client.dll -define:SILVERLIGHT;SL5;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug- %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 7.0
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\WindowsPhone\Hprose.Client.dll -define:WINDOWS_PHONE;WP70;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug- %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 7.1
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\WindowsPhone71\Hprose.Client.dll -define:WINDOWS_PHONE;WP71;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug- %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 8.0
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Runtime.Serialization.dll"
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\Csc.exe -keyfile:HproseKeys.snk -out:dist\WindowsPhone8\Hprose.Client.dll -define:WINDOWS_PHONE;WP80;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug- %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Compact Framework 1.0
set CF_REFERENCE=
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v1.0\WindowsCE\mscorlib.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v1.0\WindowsCE\System.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v1.0\WindowsCE\System.Windows.Forms.dll"
c:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\Csc.exe -out:dist\CF1.0\Hprose.Client.dll -define:Smartphone;dotNETCF10;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug- -unsafe+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Compact Framework 2.0
set CF_REFERENCE=
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v2.0\WindowsCE\mscorlib.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v2.0\WindowsCE\System.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v2.0\WindowsCE\System.Windows.Forms.dll"
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe -keyfile:HproseKeys.snk -out:dist\CF2.0\Hprose.Client.dll -define:Smartphone;dotNETCF20;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug- %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Compact Framework 3.5
set CF_REFERENCE=
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\mscorlib.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\System.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\System.Core.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\System.Windows.Forms.dll"
C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe -keyfile:HproseKeys.snk -out:dist\CF3.5\Hprose.Client.dll -define:Smartphone;dotNETCF35;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug- %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 1.0
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Mono\Hprose.Client.dll -define:dotNET11;MONO;ClientOnly -noconfig -target:library -optimize+ -debug- -reference:System,System.Windows.Forms %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Mono\Hprose.dll -define:dotNET11;MONO -noconfig -target:library -optimize+ -debug- -reference:System,System.Web,System.Windows.Forms %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 2.0
call "%MONO_PATH%\gmcs" -keyfile:HproseKeys.snk -out:dist\Mono2\Hprose.Client.dll -sdk:2 -define:dotNET2;MONO;ClientOnly -noconfig -target:library -optimize+ -debug- -reference:System %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\gmcs" -keyfile:HproseKeys.snk -out:dist\Mono2\Hprose.dll -sdk:2 -define:dotNET2;MONO -noconfig -target:library -optimize+ -debug- -reference:System,System.Web %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 4.0
call "%MONO_PATH%\dmcs" -keyfile:HproseKeys.snk -out:dist\Mono4\Hprose.Client.dll -sdk:4 -define:dotNET4;MONO;ClientOnly -noconfig -target:library -optimize+ -debug- -reference:System,System.Core,System.Runtime.Serialization,System.Numerics %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\dmcs" -keyfile:HproseKeys.snk -out:dist\Mono4\Hprose.dll -sdk:4 -define:dotNET4;MONO -noconfig -target:library -optimize+ -debug- -reference:System,System.Core,System.Runtime.Serialization,System.Web,System.Numerics %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 4.5
call "%MONO_PATH%\dmcs" -keyfile:HproseKeys.snk -out:dist\Mono4.5\Hprose.Client.dll -sdk:4.5 -define:dotNET4;dotNET45;MONO;ClientOnly -noconfig -target:library -optimize+ -debug- -reference:System,System.Core,System.Runtime.Serialization,System.Numerics %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\dmcs" -keyfile:HproseKeys.snk -out:dist\Mono4.5\Hprose.dll -sdk:4.5 -define:dotNET4;dotNET45;MONO -noconfig -target:library -optimize+ -debug- -reference:System,System.Core,System.Runtime.Serialization,System.Web,System.Numerics %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity
call "%UNITY_PATH%\gmcs" -keyfile:HproseKeys.snk -out:dist\Unity\Hprose.Client.dll -sdk:2 -define:dotNET2;MONO;Unity;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -debug- -reference:System %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\gmcs" -keyfile:HproseKeys.snk -out:dist\Unity\Hprose.dll -sdk:2 -define:dotNET2;MONO;Unity;ClientProfile -noconfig -target:library -optimize+ -debug- -reference:System %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set NUMERICS_SRC=
set HPROSE_SRC=
set DOTNET_PATH=
set DOTNET_REFERENCE=
set SL_REFERENCE=
set SL2_PATH=
set SL3_PATH=
set SL4_PATH=
set SL5_PATH=
set WP_REFERENCE=
set WP70_PATH=
set WP71_PATH=
set WP80_PATH=
set MONO_PATH=
set UNITY_PATH=
set CF_REFERENCE=
set CF_PATH=
set HPROSE_INFO=
