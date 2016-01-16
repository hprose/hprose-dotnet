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

set PRO_PATH=
set NUMERICS_SRC=
set HPROSE_SRC=
set PORTABLE_REFERENCE=
set PORTABLE_PATH=
set HPROSE_INFO=
set CSC=
