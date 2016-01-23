@echo off

if not exist dist mkdir dist
if not exist dist\Hprose.Client mkdir dist\Hprose.Client
if not exist dist\Hprose.Client\Release mkdir dist\Hprose.Client\Release
if not exist dist\Hprose.Client\Debug mkdir dist\Hprose.Client\Debug

del dist\Hprose.Client\Release\portable /s /q /f
del dist\Hprose.Client\Debug\portable /s /q /f

if not exist dist\Hprose.Client\Release\portable mkdir dist\Hprose.Client\Release\portable
if not exist dist\Hprose.Client\Release\portable\profile2 mkdir dist\Hprose.Client\Release\portable\profile2
if not exist dist\Hprose.Client\Release\portable\profile3 mkdir dist\Hprose.Client\Release\portable\profile3
if not exist dist\Hprose.Client\Release\portable\profile4 mkdir dist\Hprose.Client\Release\portable\profile4
if not exist dist\Hprose.Client\Release\portable\profile5 mkdir dist\Hprose.Client\Release\portable\profile5
if not exist dist\Hprose.Client\Release\portable\profile6 mkdir dist\Hprose.Client\Release\portable\profile6
if not exist dist\Hprose.Client\Release\portable\profile7 mkdir dist\Hprose.Client\Release\portable\profile7
if not exist dist\Hprose.Client\Release\portable\profile14 mkdir dist\Hprose.Client\Release\portable\profile14
if not exist dist\Hprose.Client\Release\portable\profile18 mkdir dist\Hprose.Client\Release\portable\profile18
if not exist dist\Hprose.Client\Release\portable\profile19 mkdir dist\Hprose.Client\Release\portable\profile19
if not exist dist\Hprose.Client\Release\portable\profile23 mkdir dist\Hprose.Client\Release\portable\profile23
if not exist dist\Hprose.Client\Release\portable\profile24 mkdir dist\Hprose.Client\Release\portable\profile24
if not exist dist\Hprose.Client\Release\portable\profile31 mkdir dist\Hprose.Client\Release\portable\profile31
if not exist dist\Hprose.Client\Release\portable\profile32 mkdir dist\Hprose.Client\Release\portable\profile32
if not exist dist\Hprose.Client\Release\portable\profile36 mkdir dist\Hprose.Client\Release\portable\profile36
if not exist dist\Hprose.Client\Release\portable\profile37 mkdir dist\Hprose.Client\Release\portable\profile37
if not exist dist\Hprose.Client\Release\portable\profile41 mkdir dist\Hprose.Client\Release\portable\profile41
if not exist dist\Hprose.Client\Release\portable\profile42 mkdir dist\Hprose.Client\Release\portable\profile42
if not exist dist\Hprose.Client\Release\portable\profile44 mkdir dist\Hprose.Client\Release\portable\profile44
if not exist dist\Hprose.Client\Release\portable\profile46 mkdir dist\Hprose.Client\Release\portable\profile46
if not exist dist\Hprose.Client\Release\portable\profile47 mkdir dist\Hprose.Client\Release\portable\profile47
if not exist dist\Hprose.Client\Release\portable\profile49 mkdir dist\Hprose.Client\Release\portable\profile49
if not exist dist\Hprose.Client\Release\portable\profile78 mkdir dist\Hprose.Client\Release\portable\profile78
if not exist dist\Hprose.Client\Release\portable\profile84 mkdir dist\Hprose.Client\Release\portable\profile84
if not exist dist\Hprose.Client\Release\portable\profile88 mkdir dist\Hprose.Client\Release\portable\profile88
if not exist dist\Hprose.Client\Release\portable\profile92 mkdir dist\Hprose.Client\Release\portable\profile92
if not exist dist\Hprose.Client\Release\portable\profile95 mkdir dist\Hprose.Client\Release\portable\profile95
if not exist dist\Hprose.Client\Release\portable\profile96 mkdir dist\Hprose.Client\Release\portable\profile96
if not exist dist\Hprose.Client\Release\portable\profile102 mkdir dist\Hprose.Client\Release\portable\profile102
if not exist dist\Hprose.Client\Release\portable\profile104 mkdir dist\Hprose.Client\Release\portable\profile104
if not exist dist\Hprose.Client\Release\portable\profile111 mkdir dist\Hprose.Client\Release\portable\profile111
if not exist dist\Hprose.Client\Release\portable\profile136 mkdir dist\Hprose.Client\Release\portable\profile136
if not exist dist\Hprose.Client\Release\portable\profile143 mkdir dist\Hprose.Client\Release\portable\profile143
if not exist dist\Hprose.Client\Release\portable\profile147 mkdir dist\Hprose.Client\Release\portable\profile147
if not exist dist\Hprose.Client\Release\portable\profile151 mkdir dist\Hprose.Client\Release\portable\profile151
if not exist dist\Hprose.Client\Release\portable\profile154 mkdir dist\Hprose.Client\Release\portable\profile154
if not exist dist\Hprose.Client\Release\portable\profile157 mkdir dist\Hprose.Client\Release\portable\profile157
if not exist dist\Hprose.Client\Release\portable\profile158 mkdir dist\Hprose.Client\Release\portable\profile158
if not exist dist\Hprose.Client\Release\portable\profile225 mkdir dist\Hprose.Client\Release\portable\profile225
if not exist dist\Hprose.Client\Release\portable\profile240 mkdir dist\Hprose.Client\Release\portable\profile240
if not exist dist\Hprose.Client\Release\portable\profile255 mkdir dist\Hprose.Client\Release\portable\profile255
if not exist dist\Hprose.Client\Release\portable\profile259 mkdir dist\Hprose.Client\Release\portable\profile259
if not exist dist\Hprose.Client\Release\portable\profile328 mkdir dist\Hprose.Client\Release\portable\profile328
if not exist dist\Hprose.Client\Release\portable\profile336 mkdir dist\Hprose.Client\Release\portable\profile336
if not exist dist\Hprose.Client\Release\portable\profile344 mkdir dist\Hprose.Client\Release\portable\profile344

if not exist dist\Hprose.Client\Debug\portable mkdir dist\Hprose.Client\Debug\portable
if not exist dist\Hprose.Client\Debug\portable\profile2 mkdir dist\Hprose.Client\Debug\portable\profile2
if not exist dist\Hprose.Client\Debug\portable\profile3 mkdir dist\Hprose.Client\Debug\portable\profile3
if not exist dist\Hprose.Client\Debug\portable\profile4 mkdir dist\Hprose.Client\Debug\portable\profile4
if not exist dist\Hprose.Client\Debug\portable\profile5 mkdir dist\Hprose.Client\Debug\portable\profile5
if not exist dist\Hprose.Client\Debug\portable\profile6 mkdir dist\Hprose.Client\Debug\portable\profile6
if not exist dist\Hprose.Client\Debug\portable\profile7 mkdir dist\Hprose.Client\Debug\portable\profile7
if not exist dist\Hprose.Client\Debug\portable\profile14 mkdir dist\Hprose.Client\Debug\portable\profile14
if not exist dist\Hprose.Client\Debug\portable\profile18 mkdir dist\Hprose.Client\Debug\portable\profile18
if not exist dist\Hprose.Client\Debug\portable\profile19 mkdir dist\Hprose.Client\Debug\portable\profile19
if not exist dist\Hprose.Client\Debug\portable\profile23 mkdir dist\Hprose.Client\Debug\portable\profile23
if not exist dist\Hprose.Client\Debug\portable\profile24 mkdir dist\Hprose.Client\Debug\portable\profile24
if not exist dist\Hprose.Client\Debug\portable\profile31 mkdir dist\Hprose.Client\Debug\portable\profile31
if not exist dist\Hprose.Client\Debug\portable\profile32 mkdir dist\Hprose.Client\Debug\portable\profile32
if not exist dist\Hprose.Client\Debug\portable\profile36 mkdir dist\Hprose.Client\Debug\portable\profile36
if not exist dist\Hprose.Client\Debug\portable\profile37 mkdir dist\Hprose.Client\Debug\portable\profile37
if not exist dist\Hprose.Client\Debug\portable\profile41 mkdir dist\Hprose.Client\Debug\portable\profile41
if not exist dist\Hprose.Client\Debug\portable\profile42 mkdir dist\Hprose.Client\Debug\portable\profile42
if not exist dist\Hprose.Client\Debug\portable\profile44 mkdir dist\Hprose.Client\Debug\portable\profile44
if not exist dist\Hprose.Client\Debug\portable\profile46 mkdir dist\Hprose.Client\Debug\portable\profile46
if not exist dist\Hprose.Client\Debug\portable\profile47 mkdir dist\Hprose.Client\Debug\portable\profile47
if not exist dist\Hprose.Client\Debug\portable\profile49 mkdir dist\Hprose.Client\Debug\portable\profile49
if not exist dist\Hprose.Client\Debug\portable\profile78 mkdir dist\Hprose.Client\Debug\portable\profile78
if not exist dist\Hprose.Client\Debug\portable\profile84 mkdir dist\Hprose.Client\Debug\portable\profile84
if not exist dist\Hprose.Client\Debug\portable\profile88 mkdir dist\Hprose.Client\Debug\portable\profile88
if not exist dist\Hprose.Client\Debug\portable\profile92 mkdir dist\Hprose.Client\Debug\portable\profile92
if not exist dist\Hprose.Client\Debug\portable\profile95 mkdir dist\Hprose.Client\Debug\portable\profile95
if not exist dist\Hprose.Client\Debug\portable\profile96 mkdir dist\Hprose.Client\Debug\portable\profile96
if not exist dist\Hprose.Client\Debug\portable\profile102 mkdir dist\Hprose.Client\Debug\portable\profile102
if not exist dist\Hprose.Client\Debug\portable\profile104 mkdir dist\Hprose.Client\Debug\portable\profile104
if not exist dist\Hprose.Client\Debug\portable\profile111 mkdir dist\Hprose.Client\Debug\portable\profile111
if not exist dist\Hprose.Client\Debug\portable\profile136 mkdir dist\Hprose.Client\Debug\portable\profile136
if not exist dist\Hprose.Client\Debug\portable\profile143 mkdir dist\Hprose.Client\Debug\portable\profile143
if not exist dist\Hprose.Client\Debug\portable\profile147 mkdir dist\Hprose.Client\Debug\portable\profile147
if not exist dist\Hprose.Client\Debug\portable\profile151 mkdir dist\Hprose.Client\Debug\portable\profile151
if not exist dist\Hprose.Client\Debug\portable\profile154 mkdir dist\Hprose.Client\Debug\portable\profile154
if not exist dist\Hprose.Client\Debug\portable\profile157 mkdir dist\Hprose.Client\Debug\portable\profile157
if not exist dist\Hprose.Client\Debug\portable\profile158 mkdir dist\Hprose.Client\Debug\portable\profile158
if not exist dist\Hprose.Client\Debug\portable\profile225 mkdir dist\Hprose.Client\Debug\portable\profile225
if not exist dist\Hprose.Client\Debug\portable\profile240 mkdir dist\Hprose.Client\Debug\portable\profile240
if not exist dist\Hprose.Client\Debug\portable\profile255 mkdir dist\Hprose.Client\Debug\portable\profile255
if not exist dist\Hprose.Client\Debug\portable\profile259 mkdir dist\Hprose.Client\Debug\portable\profile259
if not exist dist\Hprose.Client\Debug\portable\profile328 mkdir dist\Hprose.Client\Debug\portable\profile328
if not exist dist\Hprose.Client\Debug\portable\profile336 mkdir dist\Hprose.Client\Debug\portable\profile336
if not exist dist\Hprose.Client\Debug\portable\profile344 mkdir dist\Hprose.Client\Debug\portable\profile344

set PRO_PATH=C:\Program Files

if DEFINED ProgramFiles(x86) set PRO_PATH=C:\Program Files (x86)

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
set HPROSE_SRC=%HPROSE_SRC% src\System\Runtime\Serialization\DataContractAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Runtime\Serialization\DataMemberAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Threading\ReaderWriterLock.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Threading\SynchronizationContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Threading\Timer.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\Windows\Forms\WindowsFormsSynchronizationContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\Extension.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseException.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseCallback.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseMethod.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseResultMode.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseInvoker.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\HproseInvocationHandler.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\IHproseFilter.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\InvokeHelper.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\MethodNameAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\ResultModeAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\SimpleModeAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Common\ByRefAttribute.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\Proxy.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\IInvocationHandler.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\CtorAccessor.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Reflection\PropertyAccessor.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\TypeEnum.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\ObjectSerializer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\ObjectUnserializer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseClassManager.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseFormatter.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseHelper.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseMode.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseReader.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseTags.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\IO\HproseWriter.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\CookieManager.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\HproseClient.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\HproseClientContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\HproseHttpClient.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Client\HproseTcpClient.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseService.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseServiceEvent.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpService.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerService.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseHttpListenerServer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseTcpListenerContext.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseTcpListenerMethods.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\Server\HproseTcpListenerServer.cs
set HPROSE_SRC=%HPROSE_SRC% src\Hprose\AssemblyInfo.cs

set HPROSE_INFO= src\AssemblyInfo.cs

set CSC=%PRO_PATH%\MSBuild\14.0\Bin\Csc.exe

REM ------------------------------------------------------------------------------------
REM  .NET Portable Framework 4.0
REM ------------------------------------------------------------------------------------

set PORTABLE_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.6\Profile

echo start compile hprose for .NET Portable Profile31 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Globalization.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile31\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile31\Hprose.Client.dll -define:PORTABLE;Profile31;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile31 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile31\Hprose.Client.dll -define:PORTABLE;Profile31;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%


echo start compile hprose for .NET Portable Profile32 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Runtime.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile32\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile32\Hprose.Client.dll -define:PORTABLE;Profile32;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile32 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile32\Hprose.Client.dll -define:PORTABLE;Profile32;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile44 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Runtime.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile44\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile44\Hprose.Client.dll -define:PORTABLE;Profile44;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile44 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile44\Hprose.Client.dll -define:PORTABLE;Profile44;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile84 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Globalization.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile84\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile84\Hprose.Client.dll -define:PORTABLE;Profile84;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile84 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile84\Hprose.Client.dll -define:PORTABLE;Profile84;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile151 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Runtime.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile151\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile151\Hprose.Client.dll -define:PORTABLE;Profile151;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile151 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile151\Hprose.Client.dll -define:PORTABLE;Profile151;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile157 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Globalization.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile157\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile157\Hprose.Client.dll -define:PORTABLE;Profile157;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile157 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile157\Hprose.Client.dll -define:PORTABLE;Profile157;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

REM ------------------------------------------------------------------------------------
REM  .NET Portable Framework 4.5
REM ------------------------------------------------------------------------------------

set PORTABLE_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.5\Profile

echo start compile hprose for .NET Portable Profile7 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Runtime.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile7\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile7\Hprose.Client.dll -define:PORTABLE;Profile7;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile7 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile7\Hprose.Client.dll -define:PORTABLE;Profile7;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile49 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Globalization.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile49\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile49\Hprose.Client.dll -define:PORTABLE;Profile49;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile49 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile49\Hprose.Client.dll -define:PORTABLE;Profile49;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile78 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Globalization.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile78\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile78\Hprose.Client.dll -define:PORTABLE;Profile78;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile78 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile78\Hprose.Client.dll -define:PORTABLE;Profile78;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile111 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Runtime.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile111\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile111\Hprose.Client.dll -define:PORTABLE;Profile111;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile111 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile111\Hprose.Client.dll -define:PORTABLE;Profile111;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile259 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Collections.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Globalization.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.IO.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Linq.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Net.Requests.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Net.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Reflection.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Reflection.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Reflection.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Runtime.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Runtime.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Runtime.Serialization.Primitives.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Text.Encoding.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Text.Encoding.Extensions.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Threading.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile259\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile259\Hprose.Client.dll -define:PORTABLE;Profile259;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile259 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile259\Hprose.Client.dll -define:PORTABLE;Profile259;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

REM ------------------------------------------------------------------------------------
REM  .NET Portable Framework 4.0
REM ------------------------------------------------------------------------------------

set PORTABLE_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.0\Profile

echo start compile hprose for .NET Portable Profile2 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile2\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile2\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile2\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile2\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile2\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile2\Hprose.Client.dll -define:PORTABLE;Profile2;SL4;WP70;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile2 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile2\Hprose.Client.dll -define:PORTABLE;Profile2;SL4;WP70;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile3 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile3\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile3\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile3\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile3\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile3\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile3\Hprose.Client.dll -define:PORTABLE;Profile3;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile3 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile3\Hprose.Client.dll -define:PORTABLE;Profile3;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile4 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile4\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile4\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile4\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile4\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile4\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile4\Hprose.Client.dll -define:PORTABLE;Profile4;SL4;WP70;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile4 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile4\Hprose.Client.dll -define:PORTABLE;Profile4;SL4;WP70;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile5 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile5\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile5\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile5\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile5\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile5\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile5\Hprose.Client.dll -define:PORTABLE;Profile5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile5 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile5\Hprose.Client.dll -define:PORTABLE;Profile5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile6 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile6\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile6\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile6\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile6\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile6\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile6\Hprose.Client.dll -define:PORTABLE;Profile6;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile6 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile6\Hprose.Client.dll -define:PORTABLE;Profile6;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile14 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile14\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile14\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile14\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile14\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile14\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile14\Hprose.Client.dll -define:PORTABLE;Profile14;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile14 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile14\Hprose.Client.dll -define:PORTABLE;Profile14;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile18 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile18\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile18\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile18\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile18\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile18\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile18\Hprose.Client.dll -define:PORTABLE;Profile18;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile18 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile18\Hprose.Client.dll -define:PORTABLE;Profile18;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile19 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile19\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile19\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile19\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile19\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile19\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile19\Hprose.Client.dll -define:PORTABLE;Profile19;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile19 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile19\Hprose.Client.dll -define:PORTABLE;Profile19;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile23 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile23\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile23\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile23\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile23\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile23\System.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile23\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile23\Hprose.Client.dll -define:PORTABLE;Profile23;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile23 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile23\Hprose.Client.dll -define:PORTABLE;Profile23;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile24 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile24\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile24\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile24\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile24\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile24\System.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile24\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile24\Hprose.Client.dll -define:PORTABLE;Profile24;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile24 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile24\Hprose.Client.dll -define:PORTABLE;Profile24;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile36 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile36\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile36\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile36\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile36\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile36\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile36\Hprose.Client.dll -define:PORTABLE;Profile36;SL4;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile36 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile36\Hprose.Client.dll -define:PORTABLE;Profile36;SL4;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile37 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile37\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile37\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile37\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile37\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile37\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile37\Hprose.Client.dll -define:PORTABLE;Profile37;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile37 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile37\Hprose.Client.dll -define:PORTABLE;Profile37;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile41 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile41\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile41\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile41\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile41\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile41\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile41\Hprose.Client.dll -define:PORTABLE;Profile41;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile41 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile41\Hprose.Client.dll -define:PORTABLE;Profile41;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile42 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile42\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile42\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile42\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile42\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile42\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile42\Hprose.Client.dll -define:PORTABLE;Profile42;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile42 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile42\Hprose.Client.dll -define:PORTABLE;Profile42;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile46 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile46\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile46\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile46\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile46\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile46\System.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile46\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile46\Hprose.Client.dll -define:PORTABLE;Profile46;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile46 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile46\Hprose.Client.dll -define:PORTABLE;Profile46;SL4;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile47 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile47\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile47\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile47\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile47\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile47\System.Numerics.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile47\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile47\Hprose.Client.dll -define:PORTABLE;Profile47;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile47 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile47\Hprose.Client.dll -define:PORTABLE;Profile47;SL5;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile88 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile88\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile88\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile88\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile88\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile88\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile88\Hprose.Client.dll -define:PORTABLE;Profile88;SL4;WP71;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile88 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile88\Hprose.Client.dll -define:PORTABLE;Profile88;SL4;WP71;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile92 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile92\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile92\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile92\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile92\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile92\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile92\Hprose.Client.dll -define:PORTABLE;Profile92;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile92 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile92\Hprose.Client.dll -define:PORTABLE;Profile92;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile95 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile95\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile95\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile95\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile95\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile95\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile95\Hprose.Client.dll -define:PORTABLE;Profile95;SL4;WP70;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile95 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile95\Hprose.Client.dll -define:PORTABLE;Profile95;SL4;WP70;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile96 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile96\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile96\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile96\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile96\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile96\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile96\Hprose.Client.dll -define:PORTABLE;Profile96;SL4;WP71;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile96 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile96\Hprose.Client.dll -define:PORTABLE;Profile96;SL4;WP71;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile102 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile102\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile102\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile102\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile102\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile102\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile102\Hprose.Client.dll -define:PORTABLE;Profile102;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile102 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile102\Hprose.Client.dll -define:PORTABLE;Profile102;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile104 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile104\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile104\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile104\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile104\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile104\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile104\Hprose.Client.dll -define:PORTABLE;Profile104;SL4;WP71;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile104 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile104\Hprose.Client.dll -define:PORTABLE;Profile104;SL4;WP71;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile136 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile136\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile136\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile136\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile136\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile136\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile136\Hprose.Client.dll -define:PORTABLE;Profile136;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile136 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile136\Hprose.Client.dll -define:PORTABLE;Profile136;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile143 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile143\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile143\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile143\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile143\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile143\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile143\Hprose.Client.dll -define:PORTABLE;Profile143;SL4;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile143 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile143\Hprose.Client.dll -define:PORTABLE;Profile143;SL4;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile147 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile147\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile147\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile147\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile147\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile147\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile147\Hprose.Client.dll -define:PORTABLE;Profile147;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile147 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile147\Hprose.Client.dll -define:PORTABLE;Profile147;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile154 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile154\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile154\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile154\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile154\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile154\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile154\Hprose.Client.dll -define:PORTABLE;Profile154;SL4;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile154 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile154\Hprose.Client.dll -define:PORTABLE;Profile154;SL4;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile158 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile158\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile158\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile158\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile158\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile158\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile158\Hprose.Client.dll -define:PORTABLE;Profile158;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile158 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile158\Hprose.Client.dll -define:PORTABLE;Profile158;SL5;WP80;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile225 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile225\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile225\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile225\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile225\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile225\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile225\Hprose.Client.dll -define:PORTABLE;Profile225;SL5;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile225 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile225\Hprose.Client.dll -define:PORTABLE;Profile225;SL5;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile240 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile240\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile240\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile240\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile240\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile240\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile240\Hprose.Client.dll -define:PORTABLE;Profile240;SL5;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile240 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile240\Hprose.Client.dll -define:PORTABLE;Profile240;SL5;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile255 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile255\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile255\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile255\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile255\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile255\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile255\Hprose.Client.dll -define:PORTABLE;Profile255;SL5;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile255 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile255\Hprose.Client.dll -define:PORTABLE;Profile255;SL5;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile328 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile328\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile328\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile328\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile328\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile328\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile328\Hprose.Client.dll -define:PORTABLE;Profile328;SL5;WP80;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile328 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile328\Hprose.Client.dll -define:PORTABLE;Profile328;SL5;WP80;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile336 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile336\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile336\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile336\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile336\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile336\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile336\Hprose.Client.dll -define:PORTABLE;Profile336;SL5;WP80;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile336 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile336\Hprose.Client.dll -define:PORTABLE;Profile336;SL5;WP80;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile344 Release
set PORTABLE_REFERENCE=
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile344\mscorlib.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile344\System.Core.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile344\System.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile344\System.Net.dll"
set PORTABLE_REFERENCE=%PORTABLE_REFERENCE% -reference:"%PORTABLE_PATH%\Profile344\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\portable\profile344\Hprose.Client.dll -define:PORTABLE;Profile344;SL5;WP80;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Portable Profile344 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\portable\profile344\Hprose.Client.dll -define:PORTABLE;Profile344;SL5;WP80;WP81;dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %PORTABLE_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%


set PRO_PATH=
set NUMERICS_SRC=
set HPROSE_SRC=
set PORTABLE_REFERENCE=
set PORTABLE_PATH=
set HPROSE_INFO=
set CSC=
