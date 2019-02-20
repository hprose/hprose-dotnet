// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.Numerics {

    [StructLayout(LayoutKind.Sequential)]
    internal struct BigIntegerBuilder {
        private const int kcbitUint = 0x20;
        private int _iuLast;
        private uint _uSmall;
        private uint[] _rgu;
        private bool _fWritable;

        public BigIntegerBuilder(ref BigIntegerBuilder reg) {
            this = reg;
            if (_fWritable) {
                _fWritable = false;
                if (_iuLast == 0) {
                    _rgu = null;
                }
                else {
                    reg._fWritable = false;
                }
            }
        }

        public BigIntegerBuilder(int cuAlloc) {
            _iuLast = 0;
            _uSmall = 0;
            if (cuAlloc > 1) {
                _rgu = new uint[cuAlloc];
                _fWritable = true;
            }
            else {
                _rgu = null;
                _fWritable = false;
            }
        }

        public BigIntegerBuilder(BigInteger bn) {
            _fWritable = false;
            _rgu = bn._Bits;
            if (_rgu == null) {
                _iuLast = 0;
                _uSmall = NumericsHelpers.Abs(bn._Sign);
            }
            else {
                _iuLast = _rgu.Length - 1;
                _uSmall = _rgu[0];
                while ((_iuLast > 0) && (_rgu[_iuLast] == 0)) {
                    _iuLast--;
                }
            }
        }

        public BigIntegerBuilder(BigInteger bn, ref int sign) {
            _fWritable = false;
            _rgu = bn._Bits;
            int num = bn._Sign;
            int num2 = num >> 0x1f;
            sign = (sign ^ num2) - num2;
            if (_rgu == null) {
                _iuLast = 0;
                _uSmall = (uint)((num ^ num2) - num2);
            }
            else {
                _iuLast = _rgu.Length - 1;
                _uSmall = _rgu[0];
                while ((_iuLast > 0) && (_rgu[_iuLast] == 0)) {
                    _iuLast--;
                }
            }
        }

        public BigInteger GetInteger(int sign) {
            GetIntegerParts(sign, out sign, out uint[] numArray);
            return new BigInteger(sign, numArray);
        }

        internal void GetIntegerParts(int signSrc, out int sign, out uint[] bits) {
            if (_iuLast == 0) {
                if (_uSmall <= 0x7fffffff) {
                    sign = (int)(signSrc * _uSmall);
                    bits = null;
                    return;
                }
                if (_rgu == null) {
                    _rgu = new uint[] { _uSmall };
                }
                else if (_fWritable) {
                    _rgu[0] = _uSmall;
                }
                else if (_rgu[0] != _uSmall) {
                    _rgu = new uint[] { _uSmall };
                }
            }
            sign = signSrc;
            int num = _rgu.Length - _iuLast - 1;
            if (num <= 1) {
                if ((num == 0) || (_rgu[_iuLast + 1] == 0)) {
                    _fWritable = false;
                    bits = _rgu;
                    return;
                }
                if (_fWritable) {
                    _rgu[_iuLast + 1] = 0;
                    _fWritable = false;
                    bits = _rgu;
                    return;
                }
            }
            bits = _rgu;
            Array.Resize(ref bits, _iuLast + 1);
            if (!_fWritable) {
                _rgu = bits;
            }
        }

        public void Set(uint u) {
            _uSmall = u;
            _iuLast = 0;
        }

        public void Set(ulong uu) {
            uint hi = NumericsHelpers.GetHi(uu);
            if (hi == 0) {
                _uSmall = NumericsHelpers.GetLo(uu);
                _iuLast = 0;
            }
            else {
                SetSizeLazy(2);
                _rgu[0] = (uint)uu;
                _rgu[1] = hi;
            }
        }

        public int Size => _iuLast + 1;
        public uint High => _iuLast != 0 ? _rgu[_iuLast] : _uSmall;
        public void GetApproxParts(out int exp, out ulong man) {
            if (_iuLast == 0) {
                man = _uSmall;
                exp = 0;
            }
            else {
                int num2;
                int index = _iuLast - 1;
                man = NumericsHelpers.MakeUlong(_rgu[index + 1], _rgu[index]);
                exp = index * 0x20;
                if ((index > 0) && ((num2 = NumericsHelpers.CbitHighZero(_rgu[index + 1])) > 0)) {
                    man = (man << (num2 & 0x3f)) | (_rgu[index - 1] >> (0x20 - num2));
                    exp -= num2;
                }
            }
        }

        private void Trim() {
            if ((_iuLast > 0) && (_rgu[_iuLast] == 0)) {
                _uSmall = _rgu[0];
                while ((--_iuLast > 0) && (_rgu[_iuLast] == 0)) {
                }
            }
        }

        private int CuNonZero {
            get {
                int num = 0;
                for (int i = _iuLast; i >= 0; i--) {
                    if (_rgu[i] != 0) {
                        num++;
                    }
                }
                return num;
            }
        }
        private void SetSizeLazy(int cu) {
            if (cu <= 1) {
                _iuLast = 0;
            }
            else {
                if (!_fWritable || (_rgu.Length < cu)) {
                    _rgu = new uint[cu];
                    _fWritable = true;
                }
                _iuLast = cu - 1;
            }
        }

        private void SetSizeClear(int cu) {
            if (cu <= 1) {
                _iuLast = 0;
                _uSmall = 0;
            }
            else {
                if (!_fWritable || (_rgu.Length < cu)) {
                    _rgu = new uint[cu];
                    _fWritable = true;
                }
                else {
                    Array.Clear(_rgu, 0, cu);
                }
                _iuLast = cu - 1;
            }
        }

        private void SetSizeKeep(int cu, int cuExtra) {
            if (cu <= 1) {
                if (_iuLast > 0) {
                    _uSmall = _rgu[0];
                }
                _iuLast = 0;
            }
            else {
                if (!_fWritable || (_rgu.Length < cu)) {
                    uint[] destinationArray = new uint[cu + cuExtra];
                    if (_iuLast == 0) {
                        destinationArray[0] = _uSmall;
                    }
                    else {
                        Array.Copy(_rgu, 0, destinationArray, 0, Math.Min(cu, _iuLast + 1));
                    }
                    _rgu = destinationArray;
                    _fWritable = true;
                }
                else if ((_iuLast + 1) < cu) {
                    Array.Clear(_rgu, _iuLast + 1, (cu - _iuLast) - 1);
                    if (_iuLast == 0) {
                        _rgu[0] = _uSmall;
                    }
                }
                _iuLast = cu - 1;
            }
        }

        public void EnsureWritable(int cu, int cuExtra) {
            if (!_fWritable || (_rgu.Length < cu)) {
                uint[] destinationArray = new uint[cu + cuExtra];
                if (_iuLast > 0) {
                    if (_iuLast >= cu) {
                        _iuLast = cu - 1;
                    }
                    Array.Copy(_rgu, 0, destinationArray, 0, _iuLast + 1);
                }
                _rgu = destinationArray;
                _fWritable = true;
            }
        }

        public void EnsureWritable(int cuExtra) {
            if (!_fWritable) {
                uint[] destinationArray = new uint[(_iuLast + 1) + cuExtra];
                Array.Copy(_rgu, 0, destinationArray, 0, _iuLast + 1);
                _rgu = destinationArray;
                _fWritable = true;
            }
        }

        public void EnsureWritable() => EnsureWritable(0);

        public void Load(ref BigIntegerBuilder reg) => Load(ref reg, 0);

        public void Load(ref BigIntegerBuilder reg, int cuExtra) {
            if (reg._iuLast == 0) {
                _uSmall = reg._uSmall;
                _iuLast = 0;
            }
            else {
                if (!_fWritable || (_rgu.Length <= reg._iuLast)) {
                    _rgu = new uint[(reg._iuLast + 1) + cuExtra];
                    _fWritable = true;
                }
                _iuLast = reg._iuLast;
                Array.Copy(reg._rgu, 0, _rgu, 0, _iuLast + 1);
            }
        }

        public void Add(uint u) {
            if (_iuLast == 0) {
                if ((_uSmall += u) < u) {
                    SetSizeLazy(2);
                    _rgu[0] = _uSmall;
                    _rgu[1] = 1;
                }
            }
            else if (u != 0) {
                uint num = _rgu[0] + u;
                if (num < u) {
                    EnsureWritable(1);
                    ApplyCarry(1);
                }
                else if (!_fWritable) {
                    EnsureWritable();
                }
                _rgu[0] = num;
            }
        }

        public void Add(ref BigIntegerBuilder reg) {
            if (reg._iuLast == 0) {
                Add(reg._uSmall);
            }
            else if (_iuLast == 0) {
                uint u = _uSmall;
                if (u == 0) {
                    this = new BigIntegerBuilder(ref reg);
                }
                else {
                    Load(ref reg, 1);
                    Add(u);
                }
            }
            else {
                EnsureWritable(Math.Max(_iuLast, reg._iuLast) + 1, 1);
                int iu = reg._iuLast + 1;
                if (_iuLast < reg._iuLast) {
                    iu = _iuLast + 1;
                    Array.Copy(reg._rgu, _iuLast + 1, _rgu, _iuLast + 1, reg._iuLast - _iuLast);
                    _iuLast = reg._iuLast;
                }
                uint uCarry = 0;
                for (int i = 0; i < iu; i++) {
                    uCarry = AddCarry(ref _rgu[i], reg._rgu[i], uCarry);
                }
                if (uCarry != 0) {
                    ApplyCarry(iu);
                }
            }
        }

        public void Sub(ref int sign, uint u) {
            if (_iuLast == 0) {
                if (u <= _uSmall) {
                    _uSmall -= u;
                }
                else {
                    _uSmall = u - _uSmall;
                    sign = -sign;
                }
            }
            else if (u != 0) {
                EnsureWritable();
                uint num = _rgu[0];
                _rgu[0] = num - u;
                if (num < u) {
                    ApplyBorrow(1);
                    Trim();
                }
            }
        }

        public void Sub(ref int sign, ref BigIntegerBuilder reg) {
            if (reg._iuLast == 0) {
                Sub(ref sign, reg._uSmall);
            }
            else if (_iuLast == 0) {
                uint u = _uSmall;
                if (u == 0) {
                    this = new BigIntegerBuilder(ref reg);
                }
                else {
                    Load(ref reg);
                    Sub(ref sign, u);
                }
                sign = -sign;
            }
            else if (_iuLast < reg._iuLast) {
                SubRev(ref reg);
                sign = -sign;
            }
            else {
                int iuMin = reg._iuLast + 1;
                if (_iuLast == reg._iuLast) {
                    _iuLast = BigInteger.GetDiffLength(_rgu, reg._rgu, _iuLast + 1) - 1;
                    if (_iuLast < 0) {
                        _iuLast = 0;
                        _uSmall = 0;
                        return;
                    }
                    uint num3 = _rgu[_iuLast];
                    uint num4 = reg._rgu[_iuLast];
                    if (_iuLast == 0) {
                        if (num3 < num4) {
                            _uSmall = num4 - num3;
                            sign = -sign;
                            return;
                        }
                        _uSmall = num3 - num4;
                        return;
                    }
                    if (num3 < num4) {
                        reg._iuLast = _iuLast;
                        SubRev(ref reg);
                        reg._iuLast = iuMin - 1;
                        sign = -sign;
                        return;
                    }
                    iuMin = _iuLast + 1;
                }
                EnsureWritable();
                uint uBorrow = 0;
                for (int i = 0; i < iuMin; i++) {
                    uBorrow = SubBorrow(ref _rgu[i], reg._rgu[i], uBorrow);
                }
                if (uBorrow != 0) {
                    ApplyBorrow(iuMin);
                }
                Trim();
            }
        }

        private void SubRev(ref BigIntegerBuilder reg) {
            EnsureWritable(reg._iuLast + 1, 0);
            int iuMin = _iuLast + 1;
            if (_iuLast < reg._iuLast) {
                Array.Copy(reg._rgu, _iuLast + 1, _rgu, _iuLast + 1, reg._iuLast - _iuLast);
                _iuLast = reg._iuLast;
            }
            uint uBorrow = 0;
            for (int i = 0; i < iuMin; i++) {
                uBorrow = SubRevBorrow(ref _rgu[i], reg._rgu[i], uBorrow);
            }
            if (uBorrow != 0) {
                ApplyBorrow(iuMin);
            }
            Trim();
        }

        public void Mul(uint u) {
            if (u == 0) {
                Set(0);
            }
            else if (u != 1) {
                if (_iuLast == 0) {
                    Set(_uSmall * (ulong)u);
                }
                else {
                    EnsureWritable(1);
                    uint uCarry = 0;
                    for (int i = 0; i <= _iuLast; i++) {
                        uCarry = MulCarry(ref _rgu[i], u, uCarry);
                    }
                    if (uCarry != 0) {
                        SetSizeKeep(_iuLast + 2, 0);
                        _rgu[_iuLast] = uCarry;
                    }
                }
            }
        }

        public void Mul(ref BigIntegerBuilder regMul) {
            if (regMul._iuLast == 0) {
                Mul(regMul._uSmall);
            }
            else if (_iuLast == 0) {
                uint u = _uSmall;
                switch (u) {
                    case 1:
                        this = new BigIntegerBuilder(ref regMul);
                        return;

                    case 0:
                        return;
                }
                Load(ref regMul, 1);
                Mul(u);
            }
            else {
                int num2 = _iuLast + 1;
                SetSizeKeep(num2 + regMul._iuLast, 1);
                int index = num2;
                while (--index >= 0) {
                    uint num4 = _rgu[index];
                    _rgu[index] = 0;
                    uint uCarry = 0;
                    for (int i = 0; i <= regMul._iuLast; i++) {
                        uCarry = AddMulCarry(ref _rgu[index + i], regMul._rgu[i], num4, uCarry);
                    }
                    if (uCarry != 0) {
                        for (int j = (index + regMul._iuLast) + 1; (uCarry != 0) && (j <= _iuLast); j++) {
                            uCarry = AddCarry(ref _rgu[j], 0, uCarry);
                        }
                        if (uCarry != 0) {
                            SetSizeKeep(_iuLast + 2, 0);
                            _rgu[_iuLast] = uCarry;
                        }
                    }
                }
            }
        }

        public void Mul(ref BigIntegerBuilder reg1, ref BigIntegerBuilder reg2) {
            if (reg1._iuLast == 0) {
                if (reg2._iuLast == 0) {
                    Set(reg1._uSmall * (ulong)reg2._uSmall);
                }
                else {
                    Load(ref reg2, 1);
                    Mul(reg1._uSmall);
                }
            }
            else if (reg2._iuLast == 0) {
                Load(ref reg1, 1);
                Mul(reg2._uSmall);
            }
            else {
                uint[] numArray;
                uint[] numArray2;
                int num;
                int num2;
                SetSizeClear(reg1._iuLast + reg2._iuLast + 2);
                if (reg1.CuNonZero <= reg2.CuNonZero) {
                    numArray = reg1._rgu;
                    num = reg1._iuLast + 1;
                    numArray2 = reg2._rgu;
                    num2 = reg2._iuLast + 1;
                }
                else {
                    numArray = reg2._rgu;
                    num = reg2._iuLast + 1;
                    numArray2 = reg1._rgu;
                    num2 = reg1._iuLast + 1;
                }
                for (int i = 0; i < num; i++) {
                    uint num4 = numArray[i];
                    if (num4 != 0) {
                        uint uCarry = 0;
                        int index = i;
                        int num7 = 0;
                        while (num7 < num2) {
                            uCarry = AddMulCarry(ref _rgu[index], num4, numArray2[num7], uCarry);
                            num7++;
                            index++;
                        }
                        while (uCarry != 0) {
                            uCarry = AddCarry(ref _rgu[index++], 0, uCarry);
                        }
                    }
                }
                Trim();
            }
        }

        public uint DivMod(uint uDen) {
            if (uDen == 1) {
                return 0;
            }
            if (_iuLast == 0) {
                uint num = _uSmall;
                _uSmall = num / uDen;
                return (num % uDen);
            }
            EnsureWritable();
            ulong num2 = 0L;
            for (int i = _iuLast; i >= 0; i--) {
                num2 = NumericsHelpers.MakeUlong((uint)num2, _rgu[i]);
                _rgu[i] = (uint)(num2 / uDen);
                num2 = num2 % uDen;
            }
            Trim();
            return (uint)num2;
        }

        public static uint Mod(ref BigIntegerBuilder regNum, uint uDen) {
            if (uDen == 1) {
                return 0;
            }
            if (regNum._iuLast == 0) {
                return (regNum._uSmall % uDen);
            }
            ulong num = 0L;
            for (int i = regNum._iuLast; i >= 0; i--) {
                num = NumericsHelpers.MakeUlong((uint)num, regNum._rgu[i]) % uDen;
            }
            return (uint)num;
        }

        public void Mod(ref BigIntegerBuilder regDen) {
            if (regDen._iuLast == 0) {
                Set(Mod(ref this, regDen._uSmall));
            }
            else if (_iuLast != 0) {
                BigIntegerBuilder regQuo = new BigIntegerBuilder();
                ModDivCore(ref this, ref regDen, false, ref regQuo);
            }
        }

        public void Div(ref BigIntegerBuilder regDen) {
            if (regDen._iuLast == 0) {
                this.DivMod(regDen._uSmall);
            }
            else if (_iuLast == 0) {
                _uSmall = 0;
            }
            else {
                BigIntegerBuilder regQuo = new BigIntegerBuilder();
                ModDivCore(ref this, ref regDen, true, ref regQuo);
                NumericsHelpers.Swap<BigIntegerBuilder>(ref this, ref regQuo);
            }
        }

        public void ModDiv(ref BigIntegerBuilder regDen, ref BigIntegerBuilder regQuo) {
            if (regDen._iuLast == 0) {
                regQuo.Set(DivMod(regDen._uSmall));
                NumericsHelpers.Swap<BigIntegerBuilder>(ref this, ref regQuo);
            }
            else if (_iuLast != 0) {
                ModDivCore(ref this, ref regDen, true, ref regQuo);
            }
        }

        private static void ModDivCore(ref BigIntegerBuilder regNum, ref BigIntegerBuilder regDen, bool fQuo, ref BigIntegerBuilder regQuo) {
            regQuo.Set((uint)0);
            if (regNum._iuLast >= regDen._iuLast) {
                int num = regDen._iuLast + 1;
                int num2 = regNum._iuLast - regDen._iuLast;
                int cu = num2;
                int index = regNum._iuLast;
                while (true) {
                    if (index < num2) {
                        cu++;
                        break;
                    }
                    if (regDen._rgu[index - num2] != regNum._rgu[index]) {
                        if (regDen._rgu[index - num2] < regNum._rgu[index]) {
                            cu++;
                        }
                        break;
                    }
                    index--;
                }
                if (cu != 0) {
                    if (fQuo) {
                        regQuo.SetSizeLazy(cu);
                    }
                    uint u = regDen._rgu[num - 1];
                    uint num6 = regDen._rgu[num - 2];
                    int num7 = NumericsHelpers.CbitHighZero(u);
                    int num8 = 0x20 - num7;
                    if (num7 > 0) {
                        u = (u << num7) | (num6 >> num8);
                        num6 = num6 << num7;
                        if (num > 2) {
                            num6 |= regDen._rgu[num - 3] >> num8;
                        }
                    }
                    regNum.EnsureWritable();
                    int num9 = cu;
                    while (--num9 >= 0) {
                        uint uHi = ((num9 + num) <= regNum._iuLast) ? regNum._rgu[num9 + num] : 0;
                        ulong num11 = NumericsHelpers.MakeUlong(uHi, regNum._rgu[(num9 + num) - 1]);
                        uint uLo = regNum._rgu[(num9 + num) - 2];
                        if (num7 > 0) {
                            num11 = (num11 << num7) | (uLo >> num8);
                            uLo = uLo << num7;
                            if ((num9 + num) >= 3) {
                                uLo |= regNum._rgu[(num9 + num) - 3] >> num8;
                            }
                        }
                        ulong num13 = num11 / u;
                        ulong num14 = (uint)(num11 % u);
                        if (num13 > 0xffffffffL) {
                            num14 += u * (num13 - 0xffffffffL);
                            num13 = 0xffffffffL;
                        }
                        while ((num14 <= 0xffffffffL) && ((num13 * num6) > NumericsHelpers.MakeUlong((uint)num14, uLo))) {
                            num13 -= 1L;
                            num14 += u;
                        }
                        if (num13 > 0L) {
                            ulong num15 = 0L;
                            for (int i = 0; i < num; i++) {
                                num15 += regDen._rgu[i] * num13;
                                uint num17 = (uint)num15;
                                num15 = num15 >> 0x20;
                                if (regNum._rgu[num9 + i] < num17) {
                                    num15 += 1L;
                                }
                                regNum._rgu[num9 + i] -= num17;
                            }
                            if (uHi < num15) {
                                uint uCarry = 0;
                                for (int j = 0; j < num; j++) {
                                    uCarry = AddCarry(ref regNum._rgu[num9 + j], regDen._rgu[j], uCarry);
                                }
                                num13 -= 1L;
                            }
                            regNum._iuLast = (num9 + num) - 1;
                        }
                        if (fQuo) {
                            if (cu == 1) {
                                regQuo._uSmall = (uint)num13;
                            }
                            else {
                                regQuo._rgu[num9] = (uint)num13;
                            }
                        }
                    }
                    regNum._iuLast = num - 1;
                    regNum.Trim();
                }
            }
        }

        public void ShiftRight(int cbit) {
            if (cbit <= 0) {
                if (cbit < 0) {
                    ShiftLeft(-cbit);
                }
            }
            else {
                ShiftRight(cbit / 0x20, cbit % 0x20);
            }
        }

        public void ShiftRight(int cuShift, int cbitShift) {
            if ((cuShift | cbitShift) != 0) {
                if (cuShift > _iuLast) {
                    Set(0);
                }
                else if (_iuLast == 0) {
                    _uSmall = _uSmall >> cbitShift;
                }
                else {
                    uint[] sourceArray = _rgu;
                    int num = _iuLast + 1;
                    _iuLast -= cuShift;
                    if (_iuLast == 0) {
                        _uSmall = sourceArray[cuShift] >> cbitShift;
                    }
                    else {
                        if (!_fWritable) {
                            _rgu = new uint[_iuLast + 1];
                            _fWritable = true;
                        }
                        if (cbitShift > 0) {
                            int index = cuShift + 1;
                            for (int i = 0; index < num; i++) {
                                _rgu[i] = (sourceArray[index - 1] >> cbitShift) | (sourceArray[index] << (0x20 - cbitShift));
                                index++;
                            }
                            _rgu[_iuLast] = sourceArray[num - 1] >> cbitShift;
                            Trim();
                        }
                        else {
                            Array.Copy(sourceArray, cuShift, _rgu, 0, _iuLast + 1);
                        }
                    }
                }
            }
        }

        public void ShiftLeft(int cbit) {
            if (cbit <= 0) {
                if (cbit < 0) {
                    ShiftRight(-cbit);
                }
            }
            else {
                ShiftLeft(cbit / 0x20, cbit % 0x20);
            }
        }

        public void ShiftLeft(int cuShift, int cbitShift) {
            int index = _iuLast + cuShift;
            uint num2 = 0;
            if (cbitShift > 0) {
                num2 = High >> (0x20 - cbitShift);
                if (num2 != 0) {
                    index++;
                }
            }
            if (index == 0) {
                _uSmall = _uSmall << cbitShift;
            }
            else {
                uint[] sourceArray = _rgu;
                bool flag = cuShift > 0;
                if (!_fWritable || (_rgu.Length <= index)) {
                    _rgu = new uint[index + 1];
                    _fWritable = true;
                    flag = false;
                }
                if (_iuLast == 0) {
                    if (num2 != 0) {
                        _rgu[cuShift + 1] = num2;
                    }
                    _rgu[cuShift] = _uSmall << cbitShift;
                }
                else if (cbitShift == 0) {
                    Array.Copy(sourceArray, 0, _rgu, cuShift, _iuLast + 1);
                }
                else {
                    int num3 = _iuLast;
                    int num4 = _iuLast + cuShift;
                    if (num4 < index) {
                        _rgu[index] = num2;
                    }
                    while (num3 > 0) {
                        _rgu[num4] = (sourceArray[num3] << cbitShift) | (sourceArray[num3 - 1] >> (0x20 - cbitShift));
                        num3--;
                        num4--;
                    }
                    _rgu[cuShift] = sourceArray[0] << cbitShift;
                }
                _iuLast = index;
                if (flag) {
                    Array.Clear(_rgu, 0, cuShift);
                }
            }
        }

        private ulong GetHigh2(int cu) {
            if ((cu - 1) <= _iuLast) {
                return NumericsHelpers.MakeUlong(_rgu[cu - 1], _rgu[cu - 2]);
            }
            if ((cu - 2) == _iuLast) {
                return _rgu[cu - 2];
            }
            return 0L;
        }

        private void ApplyCarry(int iu) {
        Label_0000:
            if (iu > _iuLast) {
                if ((_iuLast + 1) == _rgu.Length) {
                    Array.Resize(ref _rgu, _iuLast + 2);
                }
                _rgu[++_iuLast] = 1;
            }
            else if (++_rgu[iu] <= 0) {
                iu++;
                goto Label_0000;
            }
        }

        private void ApplyBorrow(int iuMin) {
            for (int i = iuMin; i <= _iuLast; i++) {
                uint num2 = _rgu[i]--;
                if (num2 > 0) {
                    return;
                }
            }
        }

        private static uint AddCarry(ref uint u1, uint u2, uint uCarry) {
            ulong num = u1 + (ulong)u2 + uCarry;
            u1 = (uint)num;
            return (uint)(num >> 0x20);
        }

        private static uint SubBorrow(ref uint u1, uint u2, uint uBorrow) {
            ulong num = u1 - (ulong)u2 - uBorrow;
            u1 = (uint)num;
            return (uint)-((int)(num >> 0x20));
        }

        private static uint SubRevBorrow(ref uint u1, uint u2, uint uBorrow) {
            ulong num = u2 - (ulong)u1 - uBorrow;
            u1 = (uint)num;
            return (uint)-((int)(num >> 0x20));
        }

        private static uint MulCarry(ref uint u1, uint u2, uint uCarry) {
            ulong num = u1 * (ulong)u2 + uCarry;
            u1 = (uint)num;
            return (uint)(num >> 0x20);
        }

        private static uint AddMulCarry(ref uint uAdd, uint uMul1, uint uMul2, uint uCarry) {
            ulong num = uMul1 * (ulong)uMul2 + uAdd + uCarry;
            uAdd = (uint)num;
            return (uint)(num >> 0x20);
        }

        public static void GCD(ref BigIntegerBuilder reg1, ref BigIntegerBuilder reg2) {
            if (((reg1._iuLast > 0) && (reg1._rgu[0] == 0)) || ((reg2._iuLast > 0) && (reg2._rgu[0] == 0))) {
                int num = reg1.MakeOdd();
                int num2 = reg2.MakeOdd();
                LehmerGcd(ref reg1, ref reg2);
                int cbit = Math.Min(num, num2);
                if (cbit > 0) {
                    reg1.ShiftLeft(cbit);
                }
            }
            else {
                LehmerGcd(ref reg1, ref reg2);
            }
        }

        private static void LehmerGcd(ref BigIntegerBuilder reg1, ref BigIntegerBuilder reg2) {
            int num2;
            uint num11;
            int sign = 1;
        Label_0002:
            num2 = reg1._iuLast + 1;
            int b = reg2._iuLast + 1;
            if (num2 < b) {
                NumericsHelpers.Swap(ref reg1, ref reg2);
                NumericsHelpers.Swap(ref num2, ref b);
            }
            if (b == 1) {
                if (num2 == 1) {
                    reg1._uSmall = NumericsHelpers.GCD(reg1._uSmall, reg2._uSmall);
                    return;
                }
                if (reg2._uSmall != 0) {
                    reg1.Set(NumericsHelpers.GCD(Mod(ref reg1, reg2._uSmall), reg2._uSmall));
                }
                return;
            }
            if (num2 == 2) {
                reg1.Set(NumericsHelpers.GCD(reg1.GetHigh2(2), reg2.GetHigh2(2)));
                return;
            }
            if (b <= (num2 - 2)) {
                reg1.Mod(ref reg2);
                goto Label_0002;
            }
            ulong a = reg1.GetHigh2(num2);
            ulong num5 = reg2.GetHigh2(num2);
            int num6 = NumericsHelpers.CbitHighZero(a | num5);
            if (num6 > 0) {
                a = (a << num6) | (reg1._rgu[num2 - 3] >> (0x20 - num6));
                num5 = (num5 << num6) | (reg2._rgu[num2 - 3] >> (0x20 - num6));
            }
            if (a < num5) {
                NumericsHelpers.Swap(ref a, ref num5);
                NumericsHelpers.Swap(ref reg1, ref reg2);
            }
            if ((a == ulong.MaxValue) || (num5 == ulong.MaxValue)) {
                a = a >> 1;
                num5 = num5 >> 1;
            }
            if (a == num5) {
                reg1.Sub(ref sign, ref reg2);
                goto Label_0002;
            }
            if (NumericsHelpers.GetHi(num5) == 0) {
                reg1.Mod(ref reg2);
                goto Label_0002;
            }
            uint num7 = 1;
            uint num8 = 0;
            uint num9 = 0;
            uint num10 = 1;
        Label_0159:
            num11 = 1;
            ulong num12 = a - num5;
            while ((num12 >= num5) && (num11 < 0x20)) {
                num12 -= num5;
                num11++;
            }
            if (num12 >= num5) {
                ulong num13 = a / num5;
                if (num13 > 0xffffffffL) {
                    goto Label_029E;
                }
                num11 = (uint)num13;
                num12 = a - (num11 * num5);
            }
            ulong num14 = num7 + num11 * (ulong)num9;
            ulong num15 = num8 + num11 * (ulong)num10;
            if (((num14 <= 0x7fffffffL) && (num15 <= 0x7fffffffL)) && ((num12 >= num15) && ((num12 + num14) <= (num5 - num9)))) {
                num7 = (uint)num14;
                num8 = (uint)num15;
                a = num12;
                if (a > num8) {
                    num11 = 1;
                    num12 = num5 - a;
                    while ((num12 >= a) && (num11 < 0x20)) {
                        num12 -= a;
                        num11++;
                    }
                    if (num12 >= a) {
                        ulong num16 = num5 / a;
                        if (num16 > 0xffffffffL) {
                            goto Label_029E;
                        }
                        num11 = (uint)num16;
                        num12 = num5 - (num11 * a);
                    }
                    num14 = num10 + num11 * (ulong)num8;
                    num15 = num9 + num11 * (ulong)num7;
                    if (((num14 <= 0x7fffffffL) && (num15 <= 0x7fffffffL)) && ((num12 >= num15) && ((num12 + num14) <= (a - num8)))) {
                        num10 = (uint)num14;
                        num9 = (uint)num15;
                        num5 = num12;
                        if (num5 > num9) {
                            goto Label_0159;
                        }
                    }
                }
            }
        Label_029E:
            if (num8 == 0) {
                if ((a / 2L) >= num5) {
                    reg1.Mod(ref reg2);
                }
                else {
                    reg1.Sub(ref sign, ref reg2);
                }
            }
            else {
                reg1.SetSizeKeep(b, 0);
                reg2.SetSizeKeep(b, 0);
                int num17 = 0;
                int num18 = 0;
                for (int i = 0; i < b; i++) {
                    uint num20 = reg1._rgu[i];
                    uint num21 = reg2._rgu[i];
                    long num22 = num20 * (long)num7 - num21 * (long)num8 + num17;
                    long num23 = num21 * (long)num10 - num20 * (long)num9 + num18;
                    num17 = (int)(num22 >> 0x20);
                    num18 = (int)(num23 >> 0x20);
                    reg1._rgu[i] = (uint)num22;
                    reg2._rgu[i] = (uint)num23;
                }
                reg1.Trim();
                reg2.Trim();
            }
            goto Label_0002;
        }

        public int CbitLowZero() {
            if (_iuLast == 0) {
                if (((_uSmall & 1) == 0) && (_uSmall != 0)) {
                    return NumericsHelpers.CbitLowZero(_uSmall);
                }
                return 0;
            }
            int index = 0;
            while (_rgu[index] == 0) {
                index++;
            }
            return (NumericsHelpers.CbitLowZero(_rgu[index]) + (index * 0x20));
        }

        public int MakeOdd() {
            int cbit = CbitLowZero();
            if (cbit > 0) {
                ShiftRight(cbit);
            }
            return cbit;
        }
    }
}

