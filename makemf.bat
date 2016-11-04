@echo off

del dist\Hprose\Release\netmf /s /q /f
del dist\Hprose\Debug\netmf /s /q /f
del dist\Hprose.Client\Release\netmf /s /q /f
del dist\Hprose.Client\Debug\netmf /s /q /f

if not exist dist mkdir dist
if not exist dist\Hprose mkdir dist\Hprose
if not exist dist\Hprose.Client mkdir dist\Hprose.Client
if not exist dist\Hprose\Release mkdir dist\Hprose\Release
if not exist dist\Hprose\Debug mkdir dist\Hprose\Debug
if not exist dist\Hprose.Client\Release mkdir dist\Hprose.Client\Release
if not exist dist\Hprose.Client\Debug mkdir dist\Hprose.Client\Debug

if not exist dist\Hprose\Release\netmf mkdir dist\Hprose\Release\netmf
if not exist dist\Hprose\Debug\netmf mkdir dist\Hprose\Debug\netmf
if not exist dist\Hprose.Client\Release\netmf mkdir dist\Hprose.Client\Release\netmf
if not exist dist\Hprose.Client\Debug\netmf mkdir dist\Hprose.Client\Debug\netmf

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
set HPROSE_SRC=%HPROSE_SRC% src\System\ArithmeticException.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\OverflowException.cs
set HPROSE_SRC=%HPROSE_SRC% src\System\FormatException.cs
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

set HPROSE_INFO= src\Hprose\Properties\AssemblyInfo.cs

set CSC=%PRO_PATH%\MSBuild\12.0\Bin\Csc.exe

echo start compile hprose for .NET Micro Framework Release
set MF_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETMicroFramework\v4.4
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%MF_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%MF_PATH%\System.IO.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%MF_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%MF_PATH%\System.Http.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%MF_PATH%\Microsoft.SPOT.IO.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%MF_PATH%\Microsoft.SPOT.Native.dll"
"%CSC%" -out:dist\Hprose.Client\Release\netmf\Hprose.Client.dll -define:dotNETMF;ClientOnly -filealign:512 -target:library -noconfig -nowarn:3011 -nostdlib+ -optimize+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Release\netmf\Hprose.dll -define:dotNETMF -filealign:512 -target:library -noconfig -nowarn:3011 -nostdlib+ -optimize+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Micro Framework Debug
"%CSC%" -out:dist\Hprose.Client\Debug\netmf\Hprose.Client.dll -define:dotNETMF;ClientOnly -filealign:512 -target:library -noconfig -nowarn:3011 -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Debug\netmf\Hprose.dll -define:dotNETMF -filealign:512 -target:library -noconfig -nowarn:3011 -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set PRO_PATH=
set NUMERICS_SRC=
set HPROSE_SRC=
set DOTNET_PATH=
set DOTNET_REFERENCE=
set HPROSE_INFO=
set CSC=