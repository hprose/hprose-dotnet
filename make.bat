@echo off

del dist /s /q /f

if not exist dist mkdir dist
if not exist dist\Hprose mkdir dist\Hprose
if not exist dist\Hprose.Client mkdir dist\Hprose.Client
if not exist dist\Hprose\Release mkdir dist\Hprose\Release
if not exist dist\Hprose\Debug mkdir dist\Hprose\Debug
if not exist dist\Hprose.Client\Release mkdir dist\Hprose.Client\Release
if not exist dist\Hprose.Client\Debug mkdir dist\Hprose.Client\Debug

if not exist dist\Hprose\Release\net10 mkdir dist\Hprose\Release\net10
if not exist dist\Hprose\Release\net10-cf mkdir dist\Hprose\Release\net10-cf
if not exist dist\Hprose\Release\net11 mkdir dist\Hprose\Release\net11
if not exist dist\Hprose\Release\net20 mkdir dist\Hprose\Release\net20
if not exist dist\Hprose\Release\net20-cf mkdir dist\Hprose\Release\net20-cf
if not exist dist\Hprose\Release\net20-x64 mkdir dist\Hprose\Release\net20-x64
if not exist dist\Hprose\Release\net35 mkdir dist\Hprose\Release\net35
if not exist dist\Hprose\Release\net35-cf mkdir dist\Hprose\Release\net35-cf
if not exist dist\Hprose\Release\net35-client mkdir dist\Hprose\Release\net35-client
if not exist dist\Hprose\Release\net35-x64 mkdir dist\Hprose\Release\net35-x64
if not exist dist\Hprose\Release\net40 mkdir dist\Hprose\Release\net40
if not exist dist\Hprose\Release\net40-client mkdir dist\Hprose\Release\net40-client
if not exist dist\Hprose\Release\net45 mkdir dist\Hprose\Release\net45
if not exist dist\Hprose\Release\net451 mkdir dist\Hprose\Release\net451
if not exist dist\Hprose\Release\net452 mkdir dist\Hprose\Release\net452
if not exist dist\Hprose\Release\net46 mkdir dist\Hprose\Release\net46
if not exist dist\Hprose\Release\netcore45 mkdir dist\Hprose\Release\netcore45
if not exist dist\Hprose\Release\netcore451 mkdir dist\Hprose\Release\netcore451
if not exist dist\Hprose\Release\wp8 mkdir dist\Hprose\Release\wp8
if not exist dist\Hprose\Release\wp81 mkdir dist\Hprose\Release\wp81
if not exist dist\Hprose\Release\wpa81 mkdir dist\Hprose\Release\wpa81
if not exist dist\Hprose\Release\mono mkdir dist\Hprose\Release\mono
if not exist dist\Hprose\Release\mono20 mkdir dist\Hprose\Release\mono20
if not exist dist\Hprose\Release\mono35 mkdir dist\Hprose\Release\mono35
if not exist dist\Hprose\Release\mono40 mkdir dist\Hprose\Release\mono40
if not exist dist\Hprose\Release\mono45 mkdir dist\Hprose\Release\mono45
if not exist dist\Hprose\Release\unity mkdir dist\Hprose\Release\unity
if not exist dist\Hprose\Release\unity-ios mkdir dist\Hprose\Release\unity-ios
if not exist dist\Hprose\Release\unity-web mkdir dist\Hprose\Release\unity-web
if not exist dist\Hprose\Release\MonoAndroid mkdir dist\Hprose\Release\MonoAndroid
if not exist dist\Hprose\Release\MonoMac mkdir dist\Hprose\Release\MonoMac
if not exist dist\Hprose\Release\MonoTouch mkdir dist\Hprose\Release\MonoTouch

if not exist dist\Hprose\Debug\net10 mkdir dist\Hprose\Debug\net10
if not exist dist\Hprose\Debug\net10-cf mkdir dist\Hprose\Debug\net10-cf
if not exist dist\Hprose\Debug\net11 mkdir dist\Hprose\Debug\net11
if not exist dist\Hprose\Debug\net20 mkdir dist\Hprose\Debug\net20
if not exist dist\Hprose\Debug\net20-cf mkdir dist\Hprose\Debug\net20-cf
if not exist dist\Hprose\Debug\net20-x64 mkdir dist\Hprose\Debug\net20-x64
if not exist dist\Hprose\Debug\net35 mkdir dist\Hprose\Debug\net35
if not exist dist\Hprose\Debug\net35-cf mkdir dist\Hprose\Debug\net35-cf
if not exist dist\Hprose\Debug\net35-client mkdir dist\Hprose\Debug\net35-client
if not exist dist\Hprose\Debug\net35-x64 mkdir dist\Hprose\Debug\net35-x64
if not exist dist\Hprose\Debug\net40 mkdir dist\Hprose\Debug\net40
if not exist dist\Hprose\Debug\net40-client mkdir dist\Hprose\Debug\net40-client
if not exist dist\Hprose\Debug\net45 mkdir dist\Hprose\Debug\net45
if not exist dist\Hprose\Debug\net451 mkdir dist\Hprose\Debug\net451
if not exist dist\Hprose\Debug\net452 mkdir dist\Hprose\Debug\net452
if not exist dist\Hprose\Debug\net46 mkdir dist\Hprose\Debug\net46
if not exist dist\Hprose\Debug\netcore45 mkdir dist\Hprose\Debug\netcore45
if not exist dist\Hprose\Debug\netcore451 mkdir dist\Hprose\Debug\netcore451
if not exist dist\Hprose\Debug\wp8 mkdir dist\Hprose\Debug\wp8
if not exist dist\Hprose\Debug\wp81 mkdir dist\Hprose\Debug\wp81
if not exist dist\Hprose\Debug\wpa81 mkdir dist\Hprose\Debug\wpa81
if not exist dist\Hprose\Debug\mono mkdir dist\Hprose\Debug\mono
if not exist dist\Hprose\Debug\mono20 mkdir dist\Hprose\Debug\mono20
if not exist dist\Hprose\Debug\mono35 mkdir dist\Hprose\Debug\mono35
if not exist dist\Hprose\Debug\mono40 mkdir dist\Hprose\Debug\mono40
if not exist dist\Hprose\Debug\mono45 mkdir dist\Hprose\Debug\mono45
if not exist dist\Hprose\Debug\unity mkdir dist\Hprose\Debug\unity
if not exist dist\Hprose\Debug\unity-ios mkdir dist\Hprose\Debug\unity-ios
if not exist dist\Hprose\Debug\unity-web mkdir dist\Hprose\Debug\unity-web
if not exist dist\Hprose\Debug\MonoAndroid mkdir dist\Hprose\Debug\MonoAndroid
if not exist dist\Hprose\Debug\MonoMac mkdir dist\Hprose\Debug\MonoMac
if not exist dist\Hprose\Debug\MonoTouch mkdir dist\Hprose\Debug\MonoTouch

if not exist dist\Hprose.Client\Release\net10 mkdir dist\Hprose.Client\Release\net10
if not exist dist\Hprose.Client\Release\net10-cf mkdir dist\Hprose.Client\Release\net10-cf
if not exist dist\Hprose.Client\Release\net11 mkdir dist\Hprose.Client\Release\net11
if not exist dist\Hprose.Client\Release\net20 mkdir dist\Hprose.Client\Release\net20
if not exist dist\Hprose.Client\Release\net20-cf mkdir dist\Hprose.Client\Release\net20-cf
if not exist dist\Hprose.Client\Release\net20-x64 mkdir dist\Hprose.Client\Release\net20-x64
if not exist dist\Hprose.Client\Release\net35 mkdir dist\Hprose.Client\Release\net35
if not exist dist\Hprose.Client\Release\net35-cf mkdir dist\Hprose.Client\Release\net35-cf
if not exist dist\Hprose.Client\Release\net35-client mkdir dist\Hprose.Client\Release\net35-client
if not exist dist\Hprose.Client\Release\net35-x64 mkdir dist\Hprose.Client\Release\net35-x64
if not exist dist\Hprose.Client\Release\net40 mkdir dist\Hprose.Client\Release\net40
if not exist dist\Hprose.Client\Release\net40-client mkdir dist\Hprose.Client\Release\net40-client
if not exist dist\Hprose.Client\Release\net45 mkdir dist\Hprose.Client\Release\net45
if not exist dist\Hprose.Client\Release\net451 mkdir dist\Hprose.Client\Release\net451
if not exist dist\Hprose.Client\Release\net452 mkdir dist\Hprose.Client\Release\net452
if not exist dist\Hprose.Client\Release\net46 mkdir dist\Hprose.Client\Release\net46
if not exist dist\Hprose.Client\Release\netcore45 mkdir dist\Hprose.Client\Release\netcore45
if not exist dist\Hprose.Client\Release\netcore451 mkdir dist\Hprose.Client\Release\netcore451
if not exist dist\Hprose.Client\Release\sl3-wp mkdir dist\Hprose.Client\Release\sl3-wp
if not exist dist\Hprose.Client\Release\sl4-wp71 mkdir dist\Hprose.Client\Release\sl4-wp71
if not exist dist\Hprose.Client\Release\wp8 mkdir dist\Hprose.Client\Release\wp8
if not exist dist\Hprose.Client\Release\wp81 mkdir dist\Hprose.Client\Release\wp81
if not exist dist\Hprose.Client\Release\wpa81 mkdir dist\Hprose.Client\Release\wpa81
if not exist dist\Hprose.Client\Release\sl2 mkdir dist\Hprose.Client\Release\sl2
if not exist dist\Hprose.Client\Release\sl30 mkdir dist\Hprose.Client\Release\sl30
if not exist dist\Hprose.Client\Release\sl40 mkdir dist\Hprose.Client\Release\sl40
if not exist dist\Hprose.Client\Release\sl50 mkdir dist\Hprose.Client\Release\sl50
if not exist dist\Hprose.Client\Release\mono mkdir dist\Hprose.Client\Release\mono
if not exist dist\Hprose.Client\Release\mono20 mkdir dist\Hprose.Client\Release\mono20
if not exist dist\Hprose.Client\Release\mono35 mkdir dist\Hprose.Client\Release\mono35
if not exist dist\Hprose.Client\Release\mono40 mkdir dist\Hprose.Client\Release\mono40
if not exist dist\Hprose.Client\Release\mono45 mkdir dist\Hprose.Client\Release\mono45
if not exist dist\Hprose.Client\Release\unity mkdir dist\Hprose.Client\Release\unity
if not exist dist\Hprose.Client\Release\unity-ios mkdir dist\Hprose.Client\Release\unity-ios
if not exist dist\Hprose.Client\Release\unity-web mkdir dist\Hprose.Client\Release\unity-web
if not exist dist\Hprose.Client\Release\MonoAndroid mkdir dist\Hprose.Client\Release\MonoAndroid
if not exist dist\Hprose.Client\Release\MonoMac mkdir dist\Hprose.Client\Release\MonoMac
if not exist dist\Hprose.Client\Release\MonoTouch mkdir dist\Hprose.Client\Release\MonoTouch

if not exist dist\Hprose.Client\Debug\net10 mkdir dist\Hprose.Client\Debug\net10
if not exist dist\Hprose.Client\Debug\net10-cf mkdir dist\Hprose.Client\Debug\net10-cf
if not exist dist\Hprose.Client\Debug\net11 mkdir dist\Hprose.Client\Debug\net11
if not exist dist\Hprose.Client\Debug\net20 mkdir dist\Hprose.Client\Debug\net20
if not exist dist\Hprose.Client\Debug\net20-cf mkdir dist\Hprose.Client\Debug\net20-cf
if not exist dist\Hprose.Client\Debug\net20-x64 mkdir dist\Hprose.Client\Debug\net20-x64
if not exist dist\Hprose.Client\Debug\net35 mkdir dist\Hprose.Client\Debug\net35
if not exist dist\Hprose.Client\Debug\net35-cf mkdir dist\Hprose.Client\Debug\net35-cf
if not exist dist\Hprose.Client\Debug\net35-client mkdir dist\Hprose.Client\Debug\net35-client
if not exist dist\Hprose.Client\Debug\net35-x64 mkdir dist\Hprose.Client\Debug\net35-x64
if not exist dist\Hprose.Client\Debug\net40 mkdir dist\Hprose.Client\Debug\net40
if not exist dist\Hprose.Client\Debug\net40-client mkdir dist\Hprose.Client\Debug\net40-client
if not exist dist\Hprose.Client\Debug\net45 mkdir dist\Hprose.Client\Debug\net45
if not exist dist\Hprose.Client\Debug\net451 mkdir dist\Hprose.Client\Debug\net451
if not exist dist\Hprose.Client\Debug\net452 mkdir dist\Hprose.Client\Debug\net452
if not exist dist\Hprose.Client\Debug\net46 mkdir dist\Hprose.Client\Debug\net46
if not exist dist\Hprose.Client\Debug\netcore45 mkdir dist\Hprose.Client\Debug\netcore45
if not exist dist\Hprose.Client\Debug\netcore451 mkdir dist\Hprose.Client\Debug\netcore451
if not exist dist\Hprose.Client\Debug\sl3-wp mkdir dist\Hprose.Client\Debug\sl3-wp
if not exist dist\Hprose.Client\Debug\sl4-wp71 mkdir dist\Hprose.Client\Debug\sl4-wp71
if not exist dist\Hprose.Client\Debug\wp8 mkdir dist\Hprose.Client\Debug\wp8
if not exist dist\Hprose.Client\Debug\wp81 mkdir dist\Hprose.Client\Debug\wp81
if not exist dist\Hprose.Client\Debug\wpa81 mkdir dist\Hprose.Client\Debug\wpa81
if not exist dist\Hprose.Client\Debug\sl2 mkdir dist\Hprose.Client\Debug\sl2
if not exist dist\Hprose.Client\Debug\sl30 mkdir dist\Hprose.Client\Debug\sl30
if not exist dist\Hprose.Client\Debug\sl40 mkdir dist\Hprose.Client\Debug\sl40
if not exist dist\Hprose.Client\Debug\sl50 mkdir dist\Hprose.Client\Debug\sl50
if not exist dist\Hprose.Client\Debug\mono mkdir dist\Hprose.Client\Debug\mono
if not exist dist\Hprose.Client\Debug\mono20 mkdir dist\Hprose.Client\Debug\mono20
if not exist dist\Hprose.Client\Debug\mono35 mkdir dist\Hprose.Client\Debug\mono35
if not exist dist\Hprose.Client\Debug\mono40 mkdir dist\Hprose.Client\Debug\mono40
if not exist dist\Hprose.Client\Debug\mono45 mkdir dist\Hprose.Client\Debug\mono45
if not exist dist\Hprose.Client\Debug\unity mkdir dist\Hprose.Client\Debug\unity
if not exist dist\Hprose.Client\Debug\unity-ios mkdir dist\Hprose.Client\Debug\unity-ios
if not exist dist\Hprose.Client\Debug\unity-web mkdir dist\Hprose.Client\Debug\unity-web
if not exist dist\Hprose.Client\Debug\MonoAndroid mkdir dist\Hprose.Client\Debug\MonoAndroid
if not exist dist\Hprose.Client\Debug\MonoMac mkdir dist\Hprose.Client\Debug\MonoMac
if not exist dist\Hprose.Client\Debug\MonoTouch mkdir dist\Hprose.Client\Debug\MonoTouch

set PRO_PATH=C:\Program Files

if DEFINED ProgramFiles(x86) set PRO_PATH=C:\Program Files (x86)

set SL2_PATH=%PRO_PATH%\Microsoft SDKs\Silverlight\v2.0\Reference Assemblies
set SL3_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\Silverlight\v3.0
set SL4_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0
set SL5_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\Silverlight\v5.0
set WP70_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone
set WP71_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone71
set WP80_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\WindowsPhone\v8.0
set WP81_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\WindowsPhone\v8.1
set WPA81_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\WindowsPhoneApp\v8.1
set CF_PATH=%PRO_PATH%\Microsoft.NET\SDK\CompactFramework
set MONO_PATH=%PRO_PATH%\Mono\bin

set UNITY_PATH=C:\Program Files\Unity\Editor\Data\MonoBleedingEdge\bin

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

set CSC=C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705\Csc.exe

echo start compile hprose for .NET 1.0 Release
"%CSC%" -out:dist\Hprose.Client\Release\net10\Hprose.Client.dll -define:dotNET10;ClientOnly -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Release\net10\Hprose.dll -define:dotNET10 -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 1.0 Debug
"%CSC%" -out:dist\Hprose.Client\Debug\net10\Hprose.Client.dll -define:dotNET10;ClientOnly -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Debug\net10\Hprose.dll -define:dotNET10 -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=c:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\Csc.exe

echo start compile hprose for .NET 1.1 Release
"%CSC%" -out:dist\Hprose.Client\Release\net11\Hprose.Client.dll -define:dotNET11;ClientOnly -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Release\net11\Hprose.dll -define:dotNET11 -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 1.1 Debug
"%CSC%" -out:dist\Hprose.Client\Debug\net11\Hprose.Client.dll -define:dotNET11;ClientOnly -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Debug\net11\Hprose.dll -define:dotNET11 -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe

echo start compile hprose for .NET 2.0 Release
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net20\Hprose.Client.dll -define:dotNET2;ClientOnly -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net20\Hprose.dll -define:dotNET2 -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 2.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net20\Hprose.Client.dll -define:dotNET2;ClientOnly -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net20\Hprose.dll -define:dotNET2 -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=c:\WINDOWS\Microsoft.NET\Framework64\v2.0.50727\Csc.exe

echo start compile hprose for .NET 2.0 x64 Release
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net20-x64\Hprose.Client.dll -define:dotNET2;ClientOnly -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net20-x64\Hprose.dll -define:dotNET2 -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 2.0 x64 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net20-x64\Hprose.Client.dll -define:dotNET2;ClientOnly -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net20-x64\Hprose.dll -define:dotNET2 -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe

echo start compile hprose for .NET 3.5 Release
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net35\Hprose.Client.dll -define:dotNET35;ClientOnly -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net35\Hprose.dll -define:dotNET35 -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 3.5 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net35\Hprose.Client.dll -define:dotNET35;ClientOnly -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net35\Hprose.dll -define:dotNET35 -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=C:\WINDOWS\Microsoft.NET\Framework64\v3.5\Csc.exe

echo start compile hprose for .NET 3.5 x64 Release
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net35-x64\Hprose.Client.dll -define:dotNET35;ClientOnly -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net35-x64\Hprose.dll -define:dotNET35 -filealign:512 -target:library -optimize+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 3.5 x64 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net35-x64\Hprose.Client.dll -define:dotNET35;ClientOnly -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net35-x64\Hprose.dll -define:dotNET35 -filealign:512 -target:library -optimize+ -debug+ %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe

echo start compile hprose for .NET 3.5 ClientProfile Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v3.5\Profile\Client
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net35-client\Hprose.Client.dll -define:dotNET35;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net35-client\Hprose.dll -define:dotNET35;ClientProfile -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 3.5 ClientProfile Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net35-client\Hprose.Client.dll -define:dotNET35;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net35-client\Hprose.dll -define:dotNET35;ClientProfile -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=%PRO_PATH%\MSBuild\14.0\Bin\Csc.exe

echo start compile hprose for .NET 4.0 Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net40\Hprose.Client.dll -define:dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net40\Hprose.dll -define:dotNET4 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.0 Debug
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net40\Hprose.Client.dll -define:dotNET4;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net40\Hprose.dll -define:dotNET4 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.0 ClientProfile Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net40-client\Hprose.Client.dll -define:dotNET4;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net40-client\Hprose.dll -define:dotNET4;ClientProfile -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.0 ClientProfile Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net40-client\Hprose.Client.dll -define:dotNET4;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net40-client\Hprose.dll -define:dotNET4;ClientProfile -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5 Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net45\Hprose.Client.dll -define:dotNET4;dotNET45;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %1 %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net45\Hprose.dll -define:dotNET4;dotNET45 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %1 %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5 Debug
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net45\Hprose.Client.dll -define:dotNET4;dotNET45;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net45\Hprose.dll -define:dotNET4;dotNET45 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5 Windows Store App Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5
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
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\netcore45\Hprose.Client.dll -define:dotNET4;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5 Windows Store App Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\netcore45\Hprose.Client.dll -define:dotNET4;dotNET45;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.1 Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net451\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net451\Hprose.dll -define:dotNET4;dotNET45;dotNET451 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.1 Debug
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net451\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net451\Hprose.dll -define:dotNET4;dotNET45;dotNET451 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.1 Windows Store App Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5.1
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
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Threading.Tasks.dll"

"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\netcore451\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.1 Windows Store App Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\netcore451\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;Core;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.2 Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net452\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;dotNET452;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net452\Hprose.dll -define:dotNET4;dotNET45;dotNET451;dotNET452 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.5.2 Debug
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net452\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;dotNET452;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net452\Hprose.dll -define:dotNET4;dotNET45;dotNET451;dotNET452 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.6 Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net46\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;dotNET452;dotNET46;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net46\Hprose.dll -define:dotNET4;dotNET45;dotNET451;dotNET452;dotNET46 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET 4.6 Debug
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net46\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;dotNET452;dotNET46;ClientOnly -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Web.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net46\Hprose.dll -define:dotNET4;dotNET45;dotNET451;dotNET452;dotNET46 -filealign:512 -target:library -noconfig -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for MonoAndroid Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\MonoAndroid\v1.0
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\MonoAndroid\Hprose.Client.dll -define:dotNET4;dotNET45;MONO;Unity;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\MonoAndroid\Hprose.dll -define:dotNET4;dotNET45;MONO;Unity;ClientProfile -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for MonoAndroid Debug
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\MonoAndroid\v1.0
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\MonoAndroid\Hprose.Client.dll -define:dotNET4;dotNET45;MONO;Unity;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\MonoAndroid\Hprose.dll -define:dotNET4;dotNET45;MONO;Unity;ClientProfile -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for MonoTouch Release
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\MonoTouch\v1.0
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\MonoTouch\Hprose.Client.dll -define:dotNET4;dotNET45;MONO;Unity;Unity_iOS;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\MonoTouch\Hprose.dll -define:dotNET4;dotNET45;MONO;Unity;Unity_iOS;ClientProfile -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for MonoTouch Debug
set DOTNET_PATH=%PRO_PATH%\Reference Assemblies\Microsoft\Framework\MonoTouch\v1.0
set DOTNET_REFERENCE=
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\mscorlib.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Core.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Runtime.Serialization.dll"
set DOTNET_REFERENCE=%DOTNET_REFERENCE% -reference:"%DOTNET_PATH%\System.Numerics.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\MonoTouch\Hprose.Client.dll -define:dotNET4;dotNET45;MONO;Unity;Unity_iOS;ClientProfile;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\MonoTouch\Hprose.dll -define:dotNET4;dotNET45;MONO;Unity;Unity_iOS;ClientProfile -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug+ %DOTNET_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

set CSC=c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe

echo start compile hprose for Silverlight 2.0 Release
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL2_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\sl2\Hprose.Client.dll -define:SILVERLIGHT;SL2;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 2.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\sl2\Hprose.Client.dll -define:SILVERLIGHT;SL2;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe

echo start compile hprose for Silverlight 3.0 Release
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL3_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\sl30\Hprose.Client.dll -define:SILVERLIGHT;SL3;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 3.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\sl30\Hprose.Client.dll -define:SILVERLIGHT;SL3;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 4.0 Release
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL4_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\sl40\Hprose.Client.dll -define:SILVERLIGHT;SL4;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 4.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\sl40\Hprose.Client.dll -define:SILVERLIGHT;SL4;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=%PRO_PATH%\MSBuild\14.0\Bin\Csc.exe

echo start compile hprose for Silverlight 5.0 Release
set SL_REFERENCE=
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\mscorlib.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Core.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Net.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Windows.dll"
set SL_REFERENCE=%SL_REFERENCE% -reference:"%SL5_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\sl50\Hprose.Client.dll -define:SILVERLIGHT;SL5;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Silverlight 5.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\sl50\Hprose.Client.dll -define:SILVERLIGHT;SL5;ClientOnly -filealign:512 -target:library -noconfig -nowarn:1685 -nostdlib+ -optimize+ -debug+ %SL_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 7.0 Release
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP70_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\sl3-wp\Hprose.Client.dll -define:WINDOWS_PHONE;WP70;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 7.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\sl3-wp\Hprose.Client.dll -define:WINDOWS_PHONE;WP70;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 7.1 Release
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP71_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\sl4-wp71\Hprose.Client.dll -define:WINDOWS_PHONE;WP71;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 7.1 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\sl4-wp71\Hprose.Client.dll -define:WINDOWS_PHONE;WP71;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 8.0 Release
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP80_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\wp8\Hprose.Client.dll -define:WINDOWS_PHONE;WP80;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 8.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\wp8\Hprose.Client.dll -define:WINDOWS_PHONE;WP80;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 8.1 Release
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP81_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP81_PATH%\System.Core.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP81_PATH%\System.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP81_PATH%\System.Net.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP81_PATH%\System.Windows.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WP81_PATH%\System.Runtime.Serialization.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\wp81\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;WINDOWS_PHONE;WP81;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone 8.1 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\wp81\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;WINDOWS_PHONE;WP81;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %WP_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone App 8.1 Release
set WP_REFERENCE=
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\mscorlib.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Collections.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.IO.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Linq.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Net.Requests.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Net.Primitives.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Reflection.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Reflection.Extensions.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Reflection.Primitives.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Runtime.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Runtime.Extensions.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Runtime.Numerics.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Runtime.Serialization.Primitives.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Text.Encoding.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Text.Encoding.Extensions.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Threading.dll"
set WP_REFERENCE=%WP_REFERENCE% -reference:"%WPA81_PATH%\System.Threading.Tasks.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\wpa81\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;Core;WP81;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ %WP_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Windows Phone App 8.1 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\wpa81\Hprose.Client.dll -define:dotNET4;dotNET45;dotNET451;Core;WP81;ClientOnly -filealign:512 -target:library -noconfig -nowarn:0444 -nostdlib+ -optimize+ -debug+ %WP_REFERENCE% %HPROSE_SRC% %HPROSE_INFO%

set CSC=c:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\Csc.exe

echo start compile hprose for .NET Compact Framework 1.0 Release
set CF_REFERENCE=
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v1.0\WindowsCE\mscorlib.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v1.0\WindowsCE\System.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v1.0\WindowsCE\System.Windows.Forms.dll"
"%CSC%" -out:dist\Hprose.Client\Release\net10-cf\Hprose.Client.dll -define:Smartphone;dotNETCF10;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -unsafe+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Release\net10-cf\Hprose.dll -define:Smartphone;dotNETCF10 -noconfig -nostdlib -filealign:512 -target:library -optimize+ -unsafe+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Compact Framework 1.0 Debug
"%CSC%" -out:dist\Hprose.Client\Debug\net10-cf\Hprose.Client.dll -define:Smartphone;dotNETCF10;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug+ -unsafe+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -out:dist\Hprose\Debug\net10-cf\Hprose.dll -define:Smartphone;dotNETCF10 -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug+ -unsafe+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Csc.exe

echo start compile hprose for .NET Compact Framework 2.0 Release
set CF_REFERENCE=
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v2.0\WindowsCE\mscorlib.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v2.0\WindowsCE\System.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v2.0\WindowsCE\System.Windows.Forms.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net20-cf\Hprose.Client.dll -define:Smartphone;dotNETCF20;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net20-cf\Hprose.dll -define:Smartphone;dotNETCF20 -noconfig -nostdlib -filealign:512 -target:library -optimize+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Compact Framework 2.0 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net20-cf\Hprose.Client.dll -define:Smartphone;dotNETCF20;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net20-cf\Hprose.dll -define:Smartphone;dotNETCF20 -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

set CSC=C:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe

echo start compile hprose for .NET Compact Framework 3.5 Release
set CF_REFERENCE=
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\mscorlib.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\System.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\System.Core.dll"
set CF_REFERENCE=%CF_REFERENCE% -reference:"%CF_PATH%\v3.5\WindowsCE\System.Windows.Forms.dll"
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\net35-cf\Hprose.Client.dll -define:Smartphone;dotNETCF35;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\net35-cf\Hprose.dll -define:Smartphone;dotNETCF35 -noconfig -nostdlib -filealign:512 -target:library -optimize+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for .NET Compact Framework 3.5 Debug
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\net35-cf\Hprose.Client.dll -define:Smartphone;dotNETCF35;ClientOnly -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
"%CSC%" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\net35-cf\Hprose.dll -define:Smartphone;dotNETCF35 -noconfig -nostdlib -filealign:512 -target:library -optimize+ -debug+ %CF_REFERENCE% %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 1.0 Release
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\mono\Hprose.Client.dll -define:dotNET11;MONO;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Windows.Forms %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\mono\Hprose.dll -define:dotNET11;MONO -noconfig -target:library -optimize+ -reference:System,System.Web,System.Windows.Forms %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 1.0 Debug
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\mono\Hprose.Client.dll -define:dotNET11;MONO;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Windows.Forms %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\mono\Hprose.dll -define:dotNET11;MONO -noconfig -target:library -optimize+ -debug+ -reference:System,System.Web,System.Windows.Forms %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 2.0 Release
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\mono20\Hprose.Client.dll -sdk:2 -define:dotNET2;MONO;ClientOnly -noconfig -target:library -optimize+ -reference:System %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\mono20\Hprose.dll -sdk:2 -define:dotNET2;MONO -noconfig -target:library -optimize+ -reference:System,System.Web %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 2.0 Debug
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\mono20\Hprose.Client.dll -sdk:2 -define:dotNET2;MONO;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\mono20\Hprose.dll -sdk:2 -define:dotNET2;MONO -noconfig -target:library -optimize+ -debug+ -reference:System,System.Web %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 3.5 Release
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\mono35\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\mono35\Hprose.dll -sdk:2 -define:dotNET35;MONO -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization,System.Web %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 3.5 Debug
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\mono35\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\mono35\Hprose.dll -sdk:2 -define:dotNET35;MONO -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization,System.Web %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 4.0 Release
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\mono40\Hprose.Client.dll -sdk:4 -define:dotNET4;MONO;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization,System.Numerics %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\mono40\Hprose.dll -sdk:4 -define:dotNET4;MONO -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization,System.Web,System.Numerics %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 4.0 Debug
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\mono40\Hprose.Client.dll -sdk:4 -define:dotNET4;MONO;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization,System.Numerics %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\mono40\Hprose.dll -sdk:4 -define:dotNET4;MONO -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization,System.Web,System.Numerics %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 4.5 Release
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\mono45\Hprose.Client.dll -sdk:4.5 -define:dotNET4;dotNET45;MONO;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization,System.Numerics %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\mono45\Hprose.dll -sdk:4.5 -define:dotNET4;dotNET45;MONO -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization,System.Web,System.Numerics %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for mono 4.5 Debug
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\mono45\Hprose.Client.dll -sdk:4.5 -define:dotNET4;dotNET45;MONO;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization,System.Numerics %HPROSE_SRC% %HPROSE_INFO%
call "%MONO_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\mono45\Hprose.dll -sdk:4.5 -define:dotNET4;dotNET45;MONO -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization,System.Web,System.Numerics %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity Release
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\unity\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;Unity;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\unity\Hprose.dll -sdk:2 -define:dotNET35;MONO;Unity;ClientProfile -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity Debug
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\unity\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;Unity;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\unity\Hprose.dll -sdk:2 -define:dotNET35;MONO;Unity;ClientProfile -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity iOS Release
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\unity-ios\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\unity-ios\Hprose.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;ClientProfile -noconfig -target:library -optimize+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity iOS Debug
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\unity-ios\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\unity-ios\Hprose.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;ClientProfile -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core,System.Runtime.Serialization %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity Web Player Release
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Release\unity-web\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;UNITY_WEBPLAYER;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -reference:System,System.Core %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Release\unity-web\Hprose.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;UNITY_WEBPLAYER;ClientProfile -noconfig -target:library -optimize+ -reference:System,System.Core %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

echo start compile hprose for Unity Web Player Debug
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose.Client\Debug\unity-web\Hprose.Client.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;UNITY_WEBPLAYER;ClientProfile;ClientOnly -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%
call "%UNITY_PATH%\mcs" -keyfile:HproseKeys.snk -out:dist\Hprose\Debug\unity-web\Hprose.dll -sdk:2 -define:dotNET35;MONO;Unity;Unity_iOS;UNITY_WEBPLAYER;ClientProfile -noconfig -target:library -optimize+ -debug+ -reference:System,System.Core %NUMERICS_SRC% %HPROSE_SRC% %HPROSE_INFO%

rem copy dist\Hprose.Client\Release\unity\* dist\Hprose.Client\Release\MonoAndroid\*
rem copy dist\Hprose\Release\unity\* dist\Hprose\Release\MonoAndroid\*

rem copy dist\Hprose.Client\Debug\unity\* dist\Hprose.Client\Debug\MonoAndroid\*
rem copy dist\Hprose\Debug\unity\* dist\Hprose\Debug\MonoAndroid\*

rem copy dist\Hprose.Client\Release\unity\* dist\Hprose.Client\Release\MonoMac\*
rem copy dist\Hprose\Release\unity\* dist\Hprose\Release\MonoMac\*

rem copy dist\Hprose.Client\Debug\unity\* dist\Hprose.Client\Debug\MonoMac\*
rem copy dist\Hprose\Debug\unity\* dist\Hprose\Debug\MonoMac\*

rem copy dist\Hprose.Client\Release\unity-ios\* dist\Hprose.Client\Release\MonoTouch\*
rem copy dist\Hprose\Release\unity-ios\* dist\Hprose\Release\MonoTouch\*

rem copy dist\Hprose.Client\Debug\unity-ios\* dist\Hprose.Client\Debug\MonoTouch\*
rem copy dist\Hprose\Debug\unity-ios\* dist\Hprose\Debug\MonoTouch\*

set PRO_PATH=
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
set WP81_PATH=
set WPA81_PATH=
set MONO_PATH=
set UNITY_PATH=
set CF_REFERENCE=
set CF_PATH=
set HPROSE_INFO=
set CSC=
