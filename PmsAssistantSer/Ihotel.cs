﻿using FluorineFx;
using FluorineFx.IO;
using FluorineFx.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluorineFx.AMF3;

namespace PmsAssistant
{
    public class Ihotel
    {
        private const string BaseAddress = "http://119.29.215.133:8090";
        private const string LogIn = "http://119.29.215.133:8090/ipmsthef/loginCenter";
        private const string LogOut = "http://119.29.215.133:8090/ipmsthef/messagebroker/amf";

        private const string Cookie = "http://119.29.215.133:8090/ipmsthef/messagebroker/amf";

        private readonly HttpClient _httpClient;

        private const string AMF_INI = "00 03 00 00 00 01 00 04 6e 75 6c 6c 00 02 2f 31" +
                                       "00 00 00 e0 0a 00 00 00 01 11 0a 81 13 4d 66 6c" +
                                       "65 78 2e 6d 65 73 73 61 67 69 6e 67 2e 6d 65 73" +
                                       "73 61 67 65 73 2e 43 6f 6d 6d 61 6e 64 4d 65 73" +
                                       "73 61 67 65 13 6f 70 65 72 61 74 69 6f 6e 1b 63" +
                                       "6f 72 72 65 6c 61 74 69 6f 6e 49 64 13 74 69 6d" +
                                       "65 73 74 61 6d 70 11 63 6c 69 65 6e 74 49 64 17" +
                                       "64 65 73 74 69 6e 61 74 69 6f 6e 13 6d 65 73 73" +
                                       "61 67 65 49 64 15 74 69 6d 65 54 6f 4c 69 76 65" +
                                       "09 62 6f 64 79 0f 68 65 61 64 65 72 73 04 05 06" +
                                       "01 04 00 01 06 01 06 49 34 32 30 39 39 32 43 41" +
                                       "2d 35 41 36 45 2d 37 34 36 43 2d 38 35 38 44 2d" +
                                       "44 32 44 30 45 37 46 34 39 39 30 43 04 00 0a 0b" +
                                       "01 01 0a 05 25 44 53 4d 65 73 73 61 67 69 6e 67" +
                                       "56 65 72 73 69 6f 6e 04 01 09 44 53 49 64 06 07" +
                                       "6e 69 6c 01";

        public const string AMFHexString = "00 03 00 00 00 01 00 04 6e 75 6c 6c 00 03 2f 34" +
                                             "38 00 00 10 eb 0a 00 00 00 01 11 0a 81 13 4f 66" +
                                             "6c 65 78 2e 6d 65 73 73 61 67 69 6e 67 2e 6d 65" +
                                             "73 73 61 67 65 73 2e 52 65 6d 6f 74 69 6e 67 4d" +
                                             "65 73 73 61 67 65 0d 73 6f 75 72 63 65 13 6f 70" +
                                             "65 72 61 74 69 6f 6e 0f 68 65 61 64 65 72 73 13" +
                                             "74 69 6d 65 73 74 61 6d 70 11 63 6c 69 65 6e 74" +
                                             "49 64 13 6d 65 73 73 61 67 65 49 64 09 62 6f 64" +
                                             "79 17 64 65 73 74 69 6e 61 74 69 6f 6e 15 74 69" +
                                             "6d 65 54 6f 4c 69 76 65 01 06 17 73 61 76 65 52" +
                                             "65 73 65 72 76 65 0a 0b 01 3f 66 6c 65 78 2e 6d" +
                                             "65 73 73 61 67 69 6e 67 2e 72 65 71 75 65 73 74" +
                                             "2e 6c 61 6e 67 75 61 67 65 06 0b 7a 68 5f 43 4e" +
                                             "19 75 73 65 72 44 74 6f 42 79 74 65 73 0c 89 67" +
                                             "78 da 85 93 dd 4e db 30 18 86 6f 05 f9 38 8d 6c" +
                                             "c7 69 13 f7 88 95 96 56 03 ca 68 99 84 a6 09 39" +
                                             "b6 a1 de f2 a7 24 65 30 d4 c3 31 98 b4 93 fd 68" +
                                             "87 d3 a4 dd c1 ce 0a b7 d3 55 3b e2 16 e6 a4 b4" +
                                             "25 fc 88 c8 52 64 eb d5 f7 bd 7e de cf a7 2c 8e" +
                                             "e9 29 8f 84 a4 00 01 43 c8 94 27 2a ce 28 f8 f7" +
                                             "e1 cb 64 fc 63 fa e7 72 7a f9 73 79 de 0c 29 50" +
                                             "ed 28 93 3e 30 94 a0 c8 f0 55 9a 75 13 21 13 0a" +
                                             "47 86 a7 de af b1 4c 57 c2 10 d5 2a d0 d6 6b 05" +
                                             "42 5a 2c 60 70 96 0e 94 16 02 02 8c 41 5e 62 de" +
                                             "b7 f1 62 af f7 bc d2 2a 75 ff f8 79 32 3e fb 7b" +
                                             "f5 6d fa 7b 3c 19 7f bd be 3a 9f 7c 3f 9b 7c fa" +
                                             "35 bd 38 bf be ba 28 fb 79 4a 5b f4 5a 4f a2 61" +
                                             "dc 11 d4 2e 6c db 65 df 4b 45 c9 d2 fa 63 8e 1e" +
                                             "ef 5f 50 b9 53 dd 8f 0e 55 d8 57 81 5c 8d e3 9e" +
                                             "4c 8e 72 04 0b 40 b5 1c 10 21 14 56 c1 52 d8 f0" +
                                             "95 0c b3 87 54 d6 5c 75 12 6b 97 1b dd f5 ce d6" +
                                             "7e 7f 6f bb b9 df ee f6 9b 1b c0 18 a6 ba fa 29" +
                                             "67 7c 20 f5 65 e1 82 39 31 66 f7 da dc d3 39 24" +
                                             "52 87 94 07 95 e9 5e 45 97 6a 05 ba 15 5c 5b 41" +
                                             "98 12 8b 22 38 17 ed e6 e5 c0 ea da 66 67 2b bf" +
                                             "73 9c 35 8a 2a bd dd ed e6 ce cb 4e af bb 33 3b" +
                                             "cd b9 42 d7 35 64 c0 94 4f 01 30 e4 71 96 b0 96" +
                                             "cf 0e 29 80 0f 7c f7 32 29 b6 9d 22 18 cd 0f bb" +
                                             "16 34 54 da 1a 86 bc 17 4b ae 98 ae d9 d7 64 d3" +
                                             "36 f3 35 15 3d 28 6f 22 8f 82 67 b9 cf 5b a4 35" +
                                             "19 fe b6 97 b1 6c 98 52 b0 75 03 ea ce 3c 16 20" +
                                             "2d 9b e2 1a 30 82 c8 53 be cc ed 06 91 50 07 27" +
                                             "25 22 f7 d3 99 89 66 44 72 8c 21 0b 6e 80 c6 2c" +
                                             "4d df 45 89 a0 c0 41 82 bb 9e f0 6c 2c 20 11 1c" +
                                             "43 68 55 85 27 1c 0b 59 52 40 db d6 da 41 14 16" +
                                             "3d 53 76 34 8b 28 95 c7 f9 d3 1b 19 ba 44 61 5f" +
                                             "45 e1 e2 45 5a fa ad f0 28 88 87 99 4c 3a e1 41" +
                                             "44 c1 2b 54 75 4d 6c 13 13 43 c7 c4 16 a9 cf f7" +
                                             "c8 44 b0 56 47 2e 36 51 d5 31 6d fd 5b 6c 90 49" +
                                             "ea da 0b a2 90 ba c2 72 68 95 79 0e 25 79 d8 5c" +
                                             "d6 a8 4d b0 47 2d ee c9 3a 42 d0 44 0e 31 75 31" +
                                             "db 79 7d 7b fa cb 13 ff 44 7e 8e 85 ad 52 30 01" +
                                             "e3 7a 10 b4 03 d2 20 2d d2 b2 21 18 8d fe 03 fa" +
                                             "44 7f f8 15 44 53 45 6e 64 70 6f 69 6e 74 01 09" +
                                             "44 53 49 64 06 07 6e 69 6c 21 44 53 52 65 71 75" +
                                             "65 73 74 54 69 6d 65 6f 75 74 04 84 58 01 04 00" +
                                             "01 06 49 46 43 44 35 34 42 37 42 2d 42 36 39 44" +
                                             "2d 30 31 44 43 2d 34 34 39 31 2d 44 45 41 37 36" +
                                             "42 46 45 42 46 45 45 09 09 01 04 05 04 0f 0a 84" +
                                             "23 3b 63 6f 6d 2e 67 72 65 65 6e 63 6c 6f 75 64" +
                                             "2e 64 74 6f 2e 46 4d 61 73 74 65 72 44 74 6f 15" +
                                             "72 73 76 54 79 70 65 44 65 73 11 76 61 6c 75 65" +
                                             "4d 61 70 0b 76 61 6c 75 65 11 63 61 72 64 4e 61" +
                                             "6d 65 13 6d 61 73 74 65 72 44 65 73 13 6d 61 73" +
                                             "74 65 72 53 75 62 19 72 61 74 65 63 6f 64 65 46" +
                                             "6c 61 67 15 6c 69 76 65 44 61 79 4e 75 6d 11 6c" +
                                             "69 76 65 48 6f 75 72 15 64 69 73 63 6f 75 6e 74" +
                                             "4e 6f 21 73 65 6c 65 63 74 65 64 53 74 65 70 4e" +
                                             "61 6d 65 0d 6d 73 54 79 70 65 0f 66 72 6f 6d 43" +
                                             "72 73 19 6d 61 73 74 65 72 53 74 61 6c 6f 67 15" +
                                             "72 73 76 53 72 63 4c 69 73 74 1d 6d 61 73 74 65" +
                                             "72 4c 69 76 65 4c 69 73 74 1d 6d 61 73 74 65 72" +
                                             "4c 69 6e 6b 4c 69 73 74 15 6d 61 73 74 65 72 42" +
                                             "61 73 65 1f 6d 61 73 74 65 72 47 72 6f 75 70 4c" +
                                             "69 73 74 17 73 74 61 79 52 73 76 52 61 74 65 17" +
                                             "6d 61 73 74 65 72 47 75 65 73 74 13 72 6d 74 79" +
                                             "70 65 44 65 73 21 73 65 72 69 61 6c 56 65 72 73" +
                                             "69 6f 6e 55 49 44 0d 73 65 78 44 65 73 1f 72 61" +
                                             "74 65 63 6f 64 65 43 61 74 65 44 65 73 13 69 64" +
                                             "74 79 70 65 44 65 73 17 72 61 74 65 63 6f 64 65" +
                                             "44 65 73 15 63 6f 6d 70 61 6e 79 44 65 73 13 6d" +
                                             "61 72 6b 65 74 44 65 73 0d 73 72 63 44 65 73 0d" +
                                             "69 73 54 72 61 79 15 63 68 61 6e 6e 65 6c 44 65" +
                                             "73 15 73 61 6c 65 6d 61 6e 44 65 73 15 63 6d 73" +
                                             "43 6f 64 65 44 65 73 06 01 0a 05 01 01 01 0a 82" +
                                             "43 3f 63 6f 6d 2e 67 72 65 65 6e 63 6c 6f 75 64" +
                                             "2e 65 6e 74 69 74 79 2e 4d 61 73 74 65 72 44 65" +
                                             "73 28 0f 72 73 76 54 79 70 65 17 73 61 6c 65 73" +
                                             "4d 61 6e 44 65 73 0f 63 6d 73 43 6f 64 65 17 63" +
                                             "61 72 64 54 79 70 65 44 65 73 11 73 61 6c 65 73" +
                                             "4d 61 6e 19 63 61 72 64 4c 65 76 65 6c 44 65 73" +
                                             "13 63 61 72 64 4c 65 76 65 6c 0d 72 6d 74 79 70" +
                                             "65 11 63 61 72 64 54 79 70 65 13 63 6f 6d 70 61" +
                                             "6e 79 49 64 52 5e 1d 63 72 65 61 74 65 44 61 74" +
                                             "65 74 69 6d 65 15 6d 6f 64 69 66 79 55 73 65 72" +
                                             "1d 6d 6f 64 69 66 79 44 61 74 65 74 69 6d 65 15" +
                                             "63 72 65 61 74 65 55 73 65 72 05 69 64 19 68 6f" +
                                             "74 65 6c 47 72 6f 75 70 49 64 0f 68 6f 74 65 6c" +
                                             "49 64 01 01 01 01 01 01 01 01 01 01 05 7f ff ff" +
                                             "ff e0 00 00 00 01 01 01 06 01 01 06 01 05 7f f8" +
                                             "00 00 00 00 00 00 05 7f f8 00 00 00 00 00 00 05" +
                                             "7f f8 00 00 00 00 00 00 0a 83 73 3f 63 6f 6d 2e" +
                                             "67 72 65 65 6e 63 6c 6f 75 64 2e 65 6e 74 69 74" +
                                             "79 2e 4d 61 73 74 65 72 53 75 62 17 73 72 63 43" +
                                             "68 61 6e 6e 65 6c 31 1b 73 72 63 4d 65 6d 62 65" +
                                             "72 44 65 73 63 15 6f 74 61 43 68 61 6e 6e 65 6c" +
                                             "17 73 72 63 43 68 61 6e 6e 65 6c 32 19 73 72 63" +
                                             "48 6f 74 65 6c 44 65 73 63 17 70 72 6f 64 75 63" +
                                             "74 43 6f 64 65 0f 70 61 79 62 61 63 6b 17 73 72" +
                                             "63 43 68 61 6e 6e 65 6c 34 11 6f 74 61 52 73 76" +
                                             "4e 6f 11 77 65 62 43 6c 61 73 73 19 73 72 63 48" +
                                             "6f 74 65 6c 43 6f 64 65 0f 67 63 52 73 76 4e 6f" +
                                             "17 73 72 63 4d 65 6d 62 65 72 4e 6f 1b 61 67 65" +
                                             "6e 63 79 4f 72 64 65 72 4e 6f 0d 6f 70 65 6e 69" +
                                             "64 0f 75 6e 69 6f 6e 69 64 23 73 72 63 48 6f 74" +
                                             "65 6c 47 72 6f 75 70 44 65 73 63 1d 73 72 63 4d" +
                                             "65 6d 62 65 72 4c 65 76 65 6c 15 6f 74 68 65 72" +
                                             "52 73 76 4e 6f 13 6f 74 61 52 65 6d 61 72 6b 23" +
                                             "73 72 63 48 6f 74 65 6c 47 72 6f 75 70 43 6f 64" +
                                             "65 0f 77 65 62 46 72 6f 6d 0b 61 70 70 69 64 17" +
                                             "73 72 63 43 68 61 6e 6e 65 6c 33 81 02 81 04 81" +
                                             "06 81 08 81 0a 81 0c 81 0e 06 01 06 01 06 01 06" +
                                             "01 06 01 06 01 06 01 06 01 06 01 06 01 06 01 06" +
                                             "01 06 01 06 01 06 01 06 01 06 01 06 01 06 01 06" +
                                             "01 06 01 06 01 06 01 06 01 01 06 01 01 06 01 05" +
                                             "7f f8 00 00 00 00 00 00 05 7f f8 00 00 00 00 00" +
                                             "00 05 7f f8 00 00 00 00 00 00 01 04 01 05 7f ff" +
                                             "ff ff e0 00 00 00 06 01 01 01 01 01 0a 07 43 66" +
                                             "6c 65 78 2e 6d 65 73 73 61 67 69 6e 67 2e 69 6f" +
                                             "2e 41 72 72 61 79 43 6f 6c 6c 65 63 74 69 6f 6e" +
                                             "09 03 01 0a 84 63 39 63 6f 6d 2e 67 72 65 65 6e" +
                                             "63 6c 6f 75 64 2e 65 6e 74 69 74 79 2e 52 73 76" +
                                             "53 72 63 11 70 61 63 6b 61 67 65 73 11 73 70 65" +
                                             "63 69 61 6c 73 11 6e 65 67 6f 52 61 74 65 0b 61" +
                                             "63 63 6e 74 13 6c 69 73 74 4f 72 64 65 72 0f 6f" +
                                             "63 63 46 6c 61 67 0f 61 72 72 44 61 74 65 0f 64" +
                                             "65 70 44 61 74 65 0d 6d 61 72 6b 65 74 15 72 73" +
                                             "76 41 72 72 44 61 74 65 09 72 6d 6e 6f 15 72 73" +
                                             "76 44 65 70 44 61 74 65 11 72 73 76 4f 63 63 49" +
                                             "64 15 69 73 53 75 72 65 52 61 74 65 0f 62 6c 6f" +
                                             "63 6b 49 64 0b 61 64 75 6c 74 13 61 6d 65 6e 69" +
                                             "74 69 65 73 7c 07 73 72 63 17 72 61 74 65 43 68" +
                                             "61 6e 67 65 64 11 63 68 69 6c 64 72 65 6e 0f 6f" +
                                             "6c 64 52 61 74 65 13 64 73 63 52 65 61 73 6f 6e" +
                                             "11 72 65 61 6c 52 61 74 65 11 6d 61 73 74 65 72" +
                                             "49 64 0b 72 6d 6e 75 6d 21 72 61 74 65 63 6f 64" +
                                             "65 43 61 74 65 67 6f 72 79 0d 72 65 6d 61 72 6b" +
                                             "11 72 61 63 6b 52 61 74 65 1d 6d 61 73 74 65 72" +
                                             "52 73 76 53 72 63 49 64 11 72 61 74 65 63 6f 64" +
                                             "65 81 02 81 04 81 06 81 08 81 0a 81 0c 81 0e 06" +
                                             "01 01 04 82 0c 04 00 04 00 06 01 08 01 42 75 be" +
                                             "25 9f d0 00 00 08 01 42 75 be 6a 49 f0 00 00 06" +
                                             "07 57 41 4b 08 01 42 75 be 25 9f d0 00 00 06 01" +
                                             "08 01 42 75 be 6a 49 f0 00 00 05 7f ff ff ff e0" +
                                             "00 00 00 06 03 46 05 7f ff ff ff e0 00 00 00 04" +
                                             "01 01 06 05 53 53 01 02 04 00 04 82 0c 06 01 04" +
                                             "82 0c 04 00 04 01 06 03 52 01 04 82 0c 04 00 06" +
                                             "07 52 41 43 01 06 01 01 06 01 05 7f f8 00 00 00" +
                                             "00 00 00 05 7f f8 00 00 00 00 00 00 05 7f f8 00" +
                                             "00 00 00 00 00 0a 15 09 01 01 0a 15 09 01 01 0a" +
                                             "8d 33 41 63 6f 6d 2e 67 72 65 65 6e 63 6c 6f 75" +
                                             "64 2e 65 6e 74 69 74 79 2e 4d 61 73 74 65 72 42" +
                                             "61 73 65 81 78 82 00 0f 63 68 61 6e 6e 65 6c 07" +
                                             "73 74 61 81 5a 09 61 72 6e 6f 11 72 73 76 53 72" +
                                             "63 49 64 19 67 72 6f 75 70 4d 61 6e 61 67 65 72" +
                                             "11 72 73 76 43 6c 61 73 73 81 62 0f 69 73 52 65" +
                                             "73 72 76 81 64 0d 72 65 73 53 74 61 11 75 70 52" +
                                             "6d 74 79 70 65 0d 72 65 73 44 65 70 0d 63 68 61" +
                                             "72 67 65 11 75 70 52 65 61 73 6f 6e 07 70 61 79" +
                                             "11 75 70 52 65 6d 61 72 6b 15 6d 6b 74 61 63 74" +
                                             "43 6f 64 65 81 6c 81 72 0d 65 78 70 53 74 61 0b" +
                                             "72 73 76 49 64 81 76 81 7e 11 6d 65 6d 62 65 72" +
                                             "4e 6f 15 69 73 52 6d 70 6f 73 74 65 64 0b 74 6d" +
                                             "53 74 61 15 6d 65 6d 62 65 72 54 79 70 65 81 70" +
                                             "0d 63 72 65 64 69 74 17 69 6e 6e 65 72 43 61 72" +
                                             "64 49 64 81 48 6e 13 72 6d 70 6f 73 74 53 74 61" +
                                             "81 46 13 69 73 46 69 78 52 6d 6e 6f 13 69 73 46" +
                                             "69 78 52 61 74 65 11 69 73 57 61 6c 6b 69 6e 0d" +
                                             "69 73 53 75 72 65 11 67 72 70 41 63 63 6e 74 0d" +
                                             "73 63 46 6c 61 67 11 69 73 53 65 63 72 65 74 0f" +
                                             "63 72 69 62 4e 75 6d 19 69 73 53 65 63 72 65 74" +
                                             "52 61 74 65 11 63 72 65 64 69 74 4e 6f 81 68 13" +
                                             "63 72 65 64 69 74 4d 61 6e 1b 63 72 65 64 69 74" +
                                             "43 6f 6d 70 61 6e 79 17 63 72 65 64 69 74 4d 6f" +
                                             "6e 65 79 0f 72 6d 6f 63 63 49 64 13 70 6b 67 4c" +
                                             "69 6e 6b 49 64 0b 72 73 76 4e 6f 13 77 68 65 72" +
                                             "65 46 72 6f 6d 0f 77 68 65 72 65 54 6f 0b 63 72" +
                                             "73 4e 6f 0f 70 75 72 70 6f 73 65 07 64 65 70 0d" +
                                             "69 73 53 65 6e 64 19 73 61 6c 65 73 43 68 61 6e" +
                                             "6e 65 6c 07 61 72 72 11 62 75 69 6c 64 69 6e 67" +
                                             "7c 0d 72 73 76 4d 61 6e 15 63 75 74 6f 66 66 44" +
                                             "61 79 73 81 6a 15 63 75 74 6f 66 66 44 61 74 65" +
                                             "81 00 81 6e 15 72 73 76 43 6f 6d 70 61 6e 79 0b" +
                                             "63 6f 4d 73 67 13 70 72 6f 6d 6f 74 69 6f 6e 81" +
                                             "74 17 65 78 74 72 61 42 65 64 4e 75 6d 13 65 78" +
                                             "74 72 61 46 6c 61 67 81 56 0d 6c 69 6e 6b 49 64" +
                                             "0f 70 61 79 43 6f 64 65 11 73 6f 75 72 63 65 49" +
                                             "64 0f 61 67 65 6e 74 49 64 0d 6d 6f 62 69 6c 65" +
                                             "81 66 17 69 73 50 65 72 6d 61 6e 65 6e 74 17 6c" +
                                             "61 73 74 4e 75 6d 4c 69 6e 6b 81 7c 0f 62 69 7a" +
                                             "44 61 74 65 0f 6c 61 73 74 4e 75 6d 09 74 61 67" +
                                             "30 81 4a 17 70 6f 73 74 69 6e 67 46 6c 61 67 13" +
                                             "64 73 63 41 6d 6f 75 6e 74 11 73 61 6c 65 73 6d" +
                                             "61 6e 15 64 73 63 50 65 72 63 65 6e 74 0f 63 6d" +
                                             "73 63 6f 64 65 13 67 72 6f 75 70 43 6f 64 65 19" +
                                             "65 78 74 72 61 42 65 64 52 61 74 65 11 63 72 69" +
                                             "62 52 61 74 65 11 6c 69 6d 69 74 41 6d 74 81 7a" +
                                             "81 02 81 04 81 06 81 08 81 0a 81 0c 81 0e 06 82" +
                                             "08 06 82 0a 06 07 4f 54 48 06 82 08 06 01 06 01" +
                                             "04 00 06 01 06 82 04 04 00 06 03 54 04 01 06 01" +
                                             "06 01 01 04 00 06 01 04 00 06 01 06 01 04 00 04" +
                                             "00 06 01 04 00 04 01 04 00 06 01 06 82 04 06 01" +
                                             "06 01 06 01 04 00 05 7f f8 00 00 00 00 00 00 06" +
                                             "01 06 01 06 82 04 06 01 06 82 04 06 82 04 06 82" +
                                             "04 06 82 04 04 00 06 01 06 82 04 04 00 06 82 04" +
                                             "06 01 06 07 57 49 4b 06 01 06 01 05 7f ff ff ff" +
                                             "e0 00 00 00 05 7f ff ff ff e0 00 00 00 05 7f ff" +
                                             "ff ff e0 00 00 00 06 01 06 01 06 01 06 01 06 01" +
                                             "08 1c 06 82 04 06 01 08 1a 06 01 06 82 06 06 13" +
                                             "e9 a9 ac e4 ba 8c e4 ba 8c 05 7f ff ff ff e0 00" +
                                             "00 00 02 08 01 42 75 be 2c 7d a0 00 00 04 00 05" +
                                             "7f ff ff ff e0 00 00 00 06 01 06 01 06 01 04 00" +
                                             "04 00 06 3d 30 30 30 30 30 30 30 30 30 30 30 30" +
                                             "30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30" +
                                             "30 30 06 82 02 05 7f ff ff ff e0 00 00 00 06 01" +
                                             "04 00 04 00 06 01 06 01 06 82 04 04 00 04 00 01" +
                                             "04 00 06 01 04 00 06 03 30 04 00 06 01 04 00 06" +
                                             "01 06 01 04 00 04 00 04 00 06 19 74 65 73 74 74" +
                                             "65 73 74 74 65 73 74 01 06 01 01 06 01 05 7f f8" +
                                             "00 00 00 00 00 00 05 7f f8 00 00 00 00 00 00 05" +
                                             "7f f8 00 00 00 00 00 00 0a 15 09 01 01 0a 15 09" +
                                             "01 01 0a 86 43 43 63 6f 6d 2e 67 72 65 65 6e 63" +
                                             "6c 6f 75 64 2e 65 6e 74 69 74 79 2e 4d 61 73 74" +
                                             "65 72 47 75 65 73 74 0d 63 61 72 65 65 72 0f 74" +
                                             "69 6d 65 73 49 6e 0f 76 69 73 61 45 6e 64 09 69" +
                                             "64 4e 6f 0d 73 74 72 65 65 74 11 69 6e 74 65 72" +
                                             "65 73 74 13 70 72 6f 66 69 6c 65 49 64 11 76 69" +
                                             "73 61 54 79 70 65 0f 7a 69 70 43 6f 64 65 09 63" +
                                             "69 74 79 17 70 72 6f 66 69 6c 65 54 79 70 65 0b" +
                                             "6e 61 6d 65 32 0b 70 68 6f 6e 65 0b 62 69 72 74" +
                                             "68 0f 63 6f 75 6e 74 72 79 13 76 69 73 61 42 65" +
                                             "67 69 6e 07 73 65 78 0d 69 64 43 6f 64 65 0d 6e" +
                                             "61 74 69 6f 6e 0b 65 6d 61 69 6c 17 6e 61 6d 65" +
                                             "43 6f 6d 62 69 6e 65 19 65 6e 74 65 72 44 61 74" +
                                             "65 45 6e 64 11 64 69 76 69 73 69 6f 6e 13 65 6e" +
                                             "74 65 72 50 6f 72 74 83 08 15 73 61 6c 75 74 61" +
                                             "74 69 6f 6e 13 70 68 6f 74 6f 53 69 67 6e 07 66" +
                                             "61 78 0d 76 69 73 61 4e 6f 09 72 61 63 65 0b 74" +
                                             "69 74 6c 65 15 6f 63 63 75 70 61 74 69 6f 6e 07" +
                                             "76 69 70 11 72 65 6c 69 67 69 6f 6e 0b 69 64 45" +
                                             "6e 64 11 70 68 6f 74 6f 50 69 63 09 6e 61 6d 65" +
                                             "13 65 6e 74 65 72 44 61 74 65 0b 73 74 61 74 65" +
                                             "11 6c 61 6e 67 75 61 67 65 13 76 69 73 61 47 72" +
                                             "61 6e 74 11 6c 61 73 74 4e 61 6d 65 13 66 69 72" +
                                             "73 74 4e 61 6d 65 81 7a 0b 6e 61 6d 65 33 81 02" +
                                             "81 04 81 06 81 08 81 0a 81 0c 81 0e 06 01 05 7f" +
                                             "ff ff ff e0 00 00 00 01 06 01 06 01 06 01 04 00" +
                                             "06 01 06 01 06 01 06 0b 47 55 45 53 54 06 11 4d" +
                                             "61 20 45 72 20 45 72 06 01 01 06 05 43 4e 01 06" +
                                             "03 31 06 05 30 31 06 84 10 06 01 06 01 01 06 01" +
                                             "06 01 06 01 06 01 05 7f ff ff ff e0 00 00 00 06" +
                                             "01 06 01 06 01 06 01 06 01 06 83 30 06 01 01 05" +
                                             "7f ff ff ff e0 00 00 00 06 83 2c 01 06 01 06 01" +
                                             "06 01 06 01 06 01 06 01 06 01 01 06 01 01 06 01" +
                                             "05 7f f8 00 00 00 00 00 00 05 7f f8 00 00 00 00" +
                                             "00 00 05 7f f8 00 00 00 00 00 00 06 82 06 05 42" +
                                             "75 bd ea 61 ba 90 00 01 06 13 e9 97 a8 e5 b8 82" +
                                             "e7 b1 bb 01 01 01 06 19 e4 b8 8a e9 97 a8 e6 95" +
                                             "a3 e5 ae a2 06 25 e9 85 92 e5 ba 97 e7 9b b4 e6" +
                                             "8e a5 e9 a2 84 e8 ae a2 02 06 0d e5 85 b6 e4 bb" +
                                             "96 06 01 06 01 06 82 04 06 29 66 4d 61 73 74 65" +
                                             "72 46 61 63 61 64 65 53 65 72 76 69 63 65 04 00";


        private string logCode;

        private string jsessionid;

        public Ihotel()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseAddress) };
        }

        public async Task<bool> Login()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"password", "1234"},
                {"userName", "MY"},
                {"typeCode", "CQYSK-F"},
                {"type", "LOGIN_TYPE_HOTEL"},
                {"flex.messaging.request.language", "zh_CN"}
            });

            //await异步等待回应
            var response = await _httpClient.PostAsync(LogIn, content);

            //await异步
            var ret = await response.Content.ReadAsStringAsync();
            if (ret.Contains("faultCode"))
            {
                return false;
            }
            // 还不知道这个返回值的作用
            logCode = ret;

            await InitAMF();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dep">depart 2017-5-5 6:00:00</param>
        /// <param name="arr">到达时间 2017-5-4 10:00:00</param>
        /// <param name="cutoffDate">截止时间 2017-5-4 12:00:00</param>
        /// <returns></returns>
        public async Task<bool> SaveReserve(string name, DateTime dep, DateTime arr, DateTime cutoffDate)
        {
            byte[] bys = GetSaveReserveBytes(name, dep, arr, cutoffDate);

            //await异步等待回应
            var response = await _httpClient.PostAsync(Cookie + ";" + this.jsessionid,
            new ByteArrayContent(bys));

            //await异步
            var ret = await response.Content.ReadAsByteArrayAsync();
            var ad = new AMFDeserializer(new MemoryStream(ret));
            var message = ad.ReadAMFMessage();
            if (message.BodyCount <= 0)
            {
                return true;
            }

            //response = await _httpClient.PostAsync(Cookie + ";" + this.jsessionid,
            //    new ByteArrayContent(StrToToHexByte(AMFHexString)));
            //ret = await response.Content.ReadAsByteArrayAsync();
            //ad = new AMFDeserializer(new MemoryStream(ret));
            //var message1 = ad.ReadAMFMessage();

            foreach (var body in message.Bodies)
            {
                object[] content = body.Content as object[];
                RemotingMessage rm = content[0] as RemotingMessage;
                // 解析
            }

            return false;
        }

        private byte[] GetSaveReserveBytes(string name, DateTime dep, DateTime arr, DateTime cutoffDate)
        {
            var ad = new AMFDeserializer(new MemoryStream(StrToToHexByte(AMFHexString)));
            var message = ad.ReadAMFMessage();

            foreach (var body in message.Bodies)
            {
                object[] content = body.Content as object[];
                RemotingMessage rm = content[0] as RemotingMessage;
                object[] bodys = rm.body as object[];
                ASObject ab = bodys[2] as ASObject;

                ASObject masterBase = ab["masterBase"] as ASObject;
                masterBase["dep"] = dep;
                masterBase["arr"] = arr;
                masterBase["rsvMan"] = name;
                masterBase["cutoffDate"] = cutoffDate;

                ASObject masterGuest = ab["masterGuest"] as ASObject;
                masterGuest["name"] = name;
                masterGuest["name2"] = "Ma San";
                masterGuest["sex"] = "1";

                ArrayCollection rsvSrcList = ab["rsvSrcList"] as ArrayCollection;
                ASObject rsvObject = rsvSrcList[0] as ASObject;
                rsvObject["arrDate"] = arr;
                rsvObject["depDate"] = dep;
                rsvObject["rsvArrDate"] = arr;
                rsvObject["rsvDepDate"] = dep;
                rsvObject["negoRate"] = 268;
                rsvObject["oldRate"] = 268;
                rsvObject["realRate"] = 268;
                rsvObject["rackRate"] = 268;
            }

            var m = new MemoryStream();
            AMFSerializer amfSerializer = new AMFSerializer(m);
            amfSerializer.WriteMessage(message);
            amfSerializer.Flush();
            return m.ToArray();
        }

        public async Task<bool> Logout()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"password", "1234"},
                {"userName", "MY"},
                {"typeCode", "CQYSK-F"},
                {"flex.messaging.request.language", "zh_CN"}
            });

            //await异步等待回应
            var response = await _httpClient.PostAsync(LogOut, content);

            //await异步
            var ret = await response.Content.ReadAsStringAsync();
            if (ret.Contains("faultCode"))
            {
                return false;
            }
            logCode = ret;
            return true;
        }

        /// <summary>  
        /// 16进制字符串转字节数组  
        /// </summary>  
        /// <param name="hexString"></param>  
        /// <returns></returns>  
        public byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;

        }

        private async Task<bool> InitAMF()
        {
            var ad = new AMFDeserializer(new MemoryStream(StrToToHexByte(AMF_INI)));
            var message = ad.ReadAMFMessage();

            foreach (var body in message.Bodies)
            {
                object[] content = body.Content as object[];
                CommandMessage cm = content[0] as CommandMessage;
                cm.messageId = Guid.NewGuid().ToString("D");
            }

            var m = new MemoryStream();
            AMFSerializer amfSerializer = new AMFSerializer(m);
            amfSerializer.WriteMessage(message);
            amfSerializer.Flush();


            _httpClient.DefaultRequestHeaders.Add("content_type", "application/x-amf");
            //await异步等待回应
            var response = await _httpClient.PostAsync(Cookie, new ByteArrayContent(m.ToArray()));
            //await异步
            var ret = await response.Content.ReadAsByteArrayAsync();
            ad = new AMFDeserializer(new MemoryStream(ret));
            message = ad.ReadAMFMessage();
            if (response.StatusCode != HttpStatusCode.OK) return false;
            var c = response.Headers.GetValues("Set-Cookie");
            foreach (var header in c)
            {
                if (header.Contains("JSESSIONID"))
                {
                    string[] ss = header.Split(';');
                    foreach (var s in ss)
                    {
                        if (s.Contains("JSESSIONID"))
                        {
                            this.jsessionid = s;
                            return true;
                        }
                    }
                }
            }
            return true;
        }
    }
}
