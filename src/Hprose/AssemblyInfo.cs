namespace Hprose {
    public class AssemblyInfo {
        public const string Version = "1.5";
    #if ClientOnly
        public const string Name = "Hprose Client";
        public const string FileName = "Hprose.Client.dll";
    #else
        public const string Name = "Hprose";
        public const string FileName = "Hprose.dll";
    #endif
    #if MONO
        #if (Unity && dotNET35)
            public const string dotNET_Version = "3.5.0.0";
            #if UNITY_WEBPLAYER
                public const string dotNET_Name = "Unity Web Player";
            #elif Unity_iOS
                public const string dotNET_Name = "Unity iOS";
            #else
                public const string dotNET_Name = "Unity";
            #endif
        #elif dotNET45
            public const string dotNET_Version = "4.0.30319.17020";
            public const string dotNET_Name = "Mono 4.5";
        #elif dotNET4
            public const string dotNET_Version = "4.0.30319.1";
            public const string dotNET_Name = "Mono 4.0";
        #elif dotNET35
            public const string dotNET_Version = "3.5.21022.8";
            public const string dotNET_Name = "Mono 3.5";
        #elif dotNET2
            public const string dotNET_Version = "2.0.50727.1433";
            public const string dotNET_Name = "Mono 2.0";
        #elif dotNET11
            public const string dotNET_Version = "1.0.5000.0";
            public const string dotNET_Name = "Mono";
        #endif
    #elif PORTABLE
            public const string dotNET_Name = ".NET Portable Framework";
        #if (Profile2 || Profile4 || Profile95)
            public const string dotNET_Version = "4.0.20.0";
        #elif (Profile88 || Profile96 || Profile104)
            public const string dotNET_Version = "4.0.30.0";
        #elif (Profile3 || Profile18 || Profile23 || Profile36 || Profile41 || Profile46 || Profile143 || Profile154)
            public const string dotNET_Version = "4.0.40.0";
        #elif (Profile136 || Profile147 || Profile158 || Profile328 || Profile336 || Profile344)
            public const string dotNET_Version = "4.0.45.0";
        #elif (Profile14 || Profile19 || Profile24 || Profile37 || Profile42 || Profile47 || Profile225 || Profile240 || Profile255)
            public const string dotNET_Version = "4.0.50.0";
        #elif (Profile92 || Profile102)
            public const string dotNET_Version = "4.0.55.0";
        #elif (Profile5 || Profile6)
            public const string dotNET_Version = "4.0.60.0";
        #elif (Profile7 || Profile49 || Profile75 || Profile78 || Profile111 || Profile259)
            public const string dotNET_Version = "4.0.30319.17929";
        #elif (Profile31 || Profile32 || Profile44 || Profile84 || Profile151 || Profile157)        
            public const string dotNET_Version = "4.0.30319.17929";
        #endif
    #elif dotNETMF
        public const string dotNET_Version = "4.4.0.0";
        public const string dotNET_Name = ".NET Micro Framework";
    #else
        #if WINDOWS_PHONE
            #if WP81
                public const string dotNET_Version = "4.0.30227.0";
                public const string dotNET_Name = "WindowsPhone 8.1";
            #elif WP80
                public const string dotNET_Version = "4.0.50829.0";
                public const string dotNET_Name = "WindowsPhone 8.0";
            #elif WP71
                public const string dotNET_Version = "3.7.11140.0";
                public const string dotNET_Name = "WindowsPhone 7.1";
            #elif WP70
                public const string dotNET_Version = "3.7.10302.0";
                public const string dotNET_Name = "WindowsPhone 7.0";
            #endif
        #elif dotNET452
            public const string dotNET_Version = "4.0.30319.34209";
            public const string dotNET_Name = ".NET Framework 4.5.2";
        #elif dotNET451
            #if WP81
                public const string dotNET_Version = "4.0.40024.3";
                public const string dotNET_Name = "WindowsPhoneApp 8.1";
            #elif Core
                public const string dotNET_Version = "4.0.40013.0";
                public const string dotNET_Name = "Windows Store 8.1 apps";
            #else
                public const string dotNET_Version = "4.0.30319.18402";
                public const string dotNET_Name = ".NET Framework 4.5.1";
            #endif
        #elif Core
            public const string dotNET_Version = "4.0.30319.18020";
            public const string dotNET_Name = "Windows Store 8 apps";
        #elif dotNET45
            public const string dotNET_Version = "4.0.30319.17929";
            public const string dotNET_Name = ".NET Framework 4.5";
        #elif dotNET4
            public const string dotNET_Version = "4.0.30319.1";
            #if ClientProfile
                public const string dotNET_Name = ".NET Framework 4.0 Client Profile";
            #else
                public const string dotNET_Name = ".NET Framework 4.0";
            #endif
        #elif dotNET35
            public const string dotNET_Version = "3.5.30729.1";
            #if ClientProfile
                public const string dotNET_Name = ".NET Framework 3.5 Client Profile";
            #else
                public const string dotNET_Name = ".NET Framework 3.5";
            #endif
        #elif dotNET2
            public const string dotNET_Version = "2.0.50727.3607";
            public const string dotNET_Name = ".NET Framework 2.0";
        #elif dotNET11
            public const string dotNET_Version = "1.1.4322.2032";
            public const string dotNET_Name = ".NET Framework 1.1";
        #elif dotNET10
            public const string dotNET_Version = "1.0.3705.6018";
            public const string dotNET_Name = ".NET Framework 1.0";
        #elif dotNETCF10
            public const string dotNET_Version = "1.0.4292.0";
            public const string dotNET_Name = ".NET Compact Framework 1.0";
        #elif dotNETCF20
            public const string dotNET_Version = "2.0.7045.0";
            public const string dotNET_Name = ".NET Compact Framework 2.0";
        #elif dotNETCF35
            public const string dotNET_Version = "3.5.7283.0";
            public const string dotNET_Name = ".NET Compact Framework 3.5";
        #elif dotNETCF39
            public const string dotNET_Version = "3.9.15155.0";
            public const string dotNET_Name = ".NET Compact Framework 3.9";
        #elif SL5
            public const string dotNET_Version = "5.0.61118.0";
            public const string dotNET_Name = "Silverlight 5";
        #elif SL4
            public const string dotNET_Version = "4.0.50524.0";
            public const string dotNET_Name = "Silverlight 4";
        #elif SL3
            public const string dotNET_Version = "3.0.40818.0";
            public const string dotNET_Name = "Silverlight 3";
        #elif SL2
            public const string dotNET_Version = "2.0.31005.0";
            public const string dotNET_Name = "Silverlight 2";
        #endif
    #endif
    #if PORTABLE
        #if (Profile2 || Profile3 || Profile4 || Profile5 || Profile6 || Profile14 || Profile18 || Profile19 || Profile23 || Profile24 || Profile36 || Profile37 || Profile41 || Profile42 || Profile46 || Profile47 || Profile88 || Profile92 || Profile95 || Profile96 || Profile102 || Profile104 || Profile136 || Profile143 || Profile147 || Profile154 || Profile158 || Profile225 || Profile240 || Profile255 || Profile328 || Profile336 || Profile344)
            public const string dotNET_MajorVersion = "4.0";
        #elif (Profile7 || Profile49 || Profile75 || Profile78 || Profile111 || Profile259)
            public const string dotNET_MajorVersion = "4.5";
        #elif (Profile31 || Profile32 || Profile44 || Profile84 || Profile151 || Profile157)        
            public const string dotNET_MajorVersion = "4.6";
        #endif
    #elif SL5
        public const string dotNET_MajorVersion = "5.0";
    #elif (dotNET45 || WP81)
        public const string dotNET_MajorVersion = "4.5";
    #elif dotNETMF
        public const string dotNET_MajorVersion = "4.4";
    #elif (dotNET4 || SL4 || WP80)
        public const string dotNET_MajorVersion = "4.0";
    #elif (WP70 || WP71)
        public const string dotNET_MajorVersion = "3.7";
    #elif dotNETCF39
        public const string dotNET_MajorVersion = "3.9";
    #elif (dotNET35 || dotNETCF35)
        public const string dotNET_MajorVersion = "3.5";
    #elif SL3
        public const string dotNET_MajorVersion = "3.0";
    #elif (dotNET2 || dotNETCF20 || SL2)
        public const string dotNET_MajorVersion = "2.0";
    #elif dotNET11
        public const string dotNET_MajorVersion = "1.1";
    #elif (dotNET10 || dotNETCF10)
        public const string dotNET_MajorVersion = "1.0";
    #endif
    }
}
