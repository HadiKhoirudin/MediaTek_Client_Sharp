using System;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using System.Text;

namespace mtkclient.library.xflash
{
    internal class MtkChipConfig : IEquatable<MtkChipConfig>
    {
        public static readonly MtkChipConfig[] ChipConfigs;

        protected virtual Type EqualityContract
        {
            [CompilerGenerated]
            get { return typeof(MtkChipConfig); }
        }

        public uint HardwareCode { get; set; }

        public string Name { get; set; }

        public uint DaCode { get; set; }

        public uint? WdgAddress { get; set; }

        public uint? PayloadAddress { get; set; }

        public uint? UartAddress { get; set; }

        public uint? PtrDl { get; set; }

        public uint? PtrDa { get; set; }

        public uint? Var1 { get; set; }

        public object SejBase { get; set; }

        public string PayloadFileName { get; set; }

        public bool UseXFlash { get; set; }

        public MtkChipConfig()
        {
            Name = "";
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("MtkChipConfig");
            stringBuilder.Append(" { ");
            if (PrintMembers(stringBuilder))
            {
                stringBuilder.Append(' ');
            }
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("HardwareCode = ");
            builder.Append(HardwareCode.ToString());
            builder.Append(", Name = ");
            builder.Append((object)Name);
            builder.Append(", DaCode = ");
            builder.Append(DaCode.ToString());
            builder.Append(", WdgAddress = ");
            builder.Append(WdgAddress.ToString());
            builder.Append(", PayloadAddress = ");
            builder.Append(PayloadAddress.ToString());
            builder.Append(", UartAddress = ");
            builder.Append(UartAddress.ToString());
            builder.Append(", PtrDl = ");
            builder.Append(PtrDl.ToString());
            builder.Append(", PtrDa = ");
            builder.Append(PtrDa.ToString());
            builder.Append(", Var1 = ");
            builder.Append(Var1.ToString());
            builder.Append(", SejBase = ");
            builder.Append(SejBase.ToString());
            builder.Append(", PayloadFileName = ");
            builder.Append((object)PayloadFileName);
            builder.Append(", UseXFlash = ");
            builder.Append(UseXFlash.ToString());
            return true;
        }

        public static bool operator !=(MtkChipConfig left, MtkChipConfig right)
        {
            return !(left == right);
        }

        public static bool operator ==(MtkChipConfig left, MtkChipConfig right)
        {
            if ((object)left != right)
            {
                return (left?.Equals(right) ?? false);
            }
            return true;
        }

        public override int GetHashCode()
        {
            return (
                    (
                        (
                            (
                                (
                                    (
                                        (
                                            (
                                                (
                                                    (
                                                        (
                                                            EqualityComparer<Type>.Default.GetHashCode(
                                                                EqualityContract
                                                            ) * -1521134295
                                                            + EqualityComparer<uint>.Default.GetHashCode(
                                                                HardwareCode
                                                            )
                                                        ) * -1521134295
                                                        + EqualityComparer<string>.Default.GetHashCode(
                                                            Name
                                                        )
                                                    ) * -1521134295
                                                    + EqualityComparer<uint>.Default.GetHashCode(
                                                        DaCode
                                                    )
                                                ) * -1521134295
                                                + EqualityComparer<uint?>.Default.GetHashCode(
                                                    WdgAddress
                                                )
                                            ) * -1521134295
                                            + EqualityComparer<uint?>.Default.GetHashCode(
                                                PayloadAddress
                                            )
                                        ) * -1521134295
                                        + EqualityComparer<uint?>.Default.GetHashCode(UartAddress)
                                    ) * -1521134295
                                    + EqualityComparer<uint?>.Default.GetHashCode(PtrDl)
                                ) * -1521134295
                                + EqualityComparer<uint?>.Default.GetHashCode(PtrDa)
                            ) * -1521134295
                            + EqualityComparer<uint?>.Default.GetHashCode(Var1)
                        ) * -1521134295
                        + EqualityComparer<object>.Default.GetHashCode(SejBase)
                    ) * -1521134295
                    + EqualityComparer<string>.Default.GetHashCode(PayloadFileName)
                ) * -1521134295
                + EqualityComparer<bool>.Default.GetHashCode(UseXFlash);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MtkChipConfig);
        }

        public new virtual bool Equals(MtkChipConfig other)
        {
            if ((object)this != other)
            {
                if (
                    (object)other != null
                    && EqualityContract == other.EqualityContract
                    && EqualityComparer<uint>.Default.Equals(HardwareCode, other.HardwareCode)
                    && EqualityComparer<string>.Default.Equals(Name, other.Name)
                    && EqualityComparer<uint>.Default.Equals(DaCode, other.DaCode)
                    && EqualityComparer<uint?>.Default.Equals(WdgAddress, other.WdgAddress)
                    && EqualityComparer<uint?>.Default.Equals(PayloadAddress, other.PayloadAddress)
                    && EqualityComparer<uint?>.Default.Equals(UartAddress, other.UartAddress)
                    && EqualityComparer<uint?>.Default.Equals(PtrDl, other.PtrDl)
                    && EqualityComparer<uint?>.Default.Equals(PtrDa, other.PtrDa)
                    && EqualityComparer<uint?>.Default.Equals(Var1, other.Var1)
                    && EqualityComparer<object>.Default.Equals(SejBase, other.SejBase)
                    && EqualityComparer<string>.Default.Equals(
                        PayloadFileName,
                        other.PayloadFileName
                    )
                )
                {
                    return EqualityComparer<bool>.Default.Equals(UseXFlash, other.UseXFlash);
                }
                return false;
            }
            return true;
        }

        public virtual MtkChipConfig _get()
        {
            return new MtkChipConfig(this);
        }

        protected MtkChipConfig(MtkChipConfig original)
        {
            HardwareCode = original.HardwareCode;
            Name = original.Name;
            DaCode = original.DaCode;
            WdgAddress = original.WdgAddress;
            PayloadAddress = original.PayloadAddress;
            UartAddress = original.UartAddress;
            PtrDl = original.PtrDl;
            PtrDa = original.PtrDa;
            Var1 = original.Var1;
            SejBase = original.SejBase;
            PayloadFileName = original.PayloadFileName;
            UseXFlash = original.UseXFlash;
        }

        static MtkChipConfig()
        {
            ChipConfigs = new MtkChipConfig[31]
            {
                new MtkChipConfig
                {
                    HardwareCode = 2450U,
                    Name = "MT0992",
                    DaCode = 2450U,
                    WdgAddress = null,
                    PayloadAddress = null,
                    UartAddress = null,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = null,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1587U,
                    Name = "MT6570",
                    DaCode = 25968U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = 0xA0110000U,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1689U,
                    Name = "MT6739/MT6731",
                    DaCode = 26425U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 57116U,
                    PtrDa = 58344U,
                    Var1 = 180U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6739_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1537U,
                    Name = "MT6750",
                    DaCode = 26453U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 806U,
                    Name = "MT6755/MT6750/M/T/S",
                    DaCode = 26453U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 39532U,
                    PtrDa = 40724U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6755_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1361U,
                    Name = "MT6757/MT6757D",
                    DaCode = 26455U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 39980U,
                    PtrDa = 41192U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6757_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1672U,
                    Name = "MT6758",
                    DaCode = 26456U,
                    WdgAddress = 270602240U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285343744U,
                    PtrDl = 55392U,
                    PtrDa = 56620U,
                    Var1 = 10U,
                    SejBase = 0x10080000,
                    PayloadFileName = "mt6758_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1815U,
                    Name = "MT6761/MT6762/MT3369/MT8766B",
                    DaCode = 26465U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 48268U,
                    PtrDa = 49496U,
                    Var1 = 37U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6761_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1680U,
                    Name = "MT6763",
                    DaCode = 26467U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 54892U,
                    PtrDa = 56120U,
                    Var1 = 127U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6763_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1894U,
                    Name = "MT6765/MT8768t",
                    DaCode = 26469U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 48576U,
                    PtrDa = 49804U,
                    Var1 = 37U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6765_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1799U,
                    Name = "MT6768",
                    DaCode = 26472U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 49552U,
                    PtrDa = 50768U,
                    Var1 = 37U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6768_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1928U,
                    Name = "MT6771/MT8385/MT8183/MT8666",
                    DaCode = 26481U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 57020U,
                    PtrDa = 58248U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6771_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1829U,
                    Name = "MT6779",
                    DaCode = 26489U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 57420U,
                    PtrDa = 58636U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6779_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 4198U,
                    Name = "MT6781",
                    DaCode = 26497U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 58840U,
                    PtrDa = 60052U,
                    Var1 = 115U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6781_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2067U,
                    Name = "MT6785",
                    DaCode = 26501U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 58020U,
                    PtrDa = 59236U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6785_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 633U,
                    Name = "MT6797/MT6767",
                    DaCode = 26519U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 40620U,
                    PtrDa = 41812U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6797_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 1378U,
                    Name = "MT6799",
                    DaCode = 26521U,
                    WdgAddress = 270602240U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285343744U,
                    PtrDl = 62892U,
                    PtrDa = 64120U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6799_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2441U,
                    Name = "MT6833",
                    DaCode = 26675U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 57312U,
                    PtrDa = 58528U,
                    Var1 = 115U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6833_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2454U,
                    Name = "MT6853",
                    DaCode = 26707U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 60004U,
                    PtrDa = 61220U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6853_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2182U,
                    Name = "MT6873",
                    DaCode = 26739U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 60024U,
                    PtrDa = 61240U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6873_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2393U,
                    Name = "MT6877",
                    DaCode = 26743U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 59600U,
                    PtrDa = 60816U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6877_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2070U,
                    Name = "MT6885/MT6883/MT6889/MT6880/MT6890",
                    DaCode = 26757U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 59132U,
                    PtrDa = 60348U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6885_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2384U,
                    Name = "MT6893",
                    DaCode = 26771U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 59292U,
                    PtrDa = 60508U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt6893_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 33040U,
                    Name = "MT8110",
                    DaCode = 33040U,
                    WdgAddress = null,
                    PayloadAddress = null,
                    UartAddress = null,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = null,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 33127U,
                    Name = "MT8167/MT8516/MT8362",
                    DaCode = 33127U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285233152U,
                    PtrDl = 53988U,
                    PtrDa = 55212U,
                    Var1 = 204U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt8167_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 33128U,
                    Name = "MT8168",
                    DaCode = 33128U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = null,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2352U,
                    Name = "MT8195",
                    DaCode = 33173U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285217280U,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = null,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 34066U,
                    Name = "MT8512",
                    DaCode = 34066U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 52292U,
                    PtrDa = 53652U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt8512_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 34072U,
                    Name = "MT8518",
                    DaCode = 34072U,
                    WdgAddress = null,
                    PayloadAddress = null,
                    UartAddress = null,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = null,
                    PayloadFileName = null,
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 34453U,
                    Name = "MT8695",
                    DaCode = 34453U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = 285220864U,
                    PtrDl = 48876U,
                    PtrDa = 50168U,
                    Var1 = 10U,
                    SejBase = 0x1000A000,
                    PayloadFileName = "mt8695_payload.bin",
                    UseXFlash = true
                },
                new MtkChipConfig
                {
                    HardwareCode = 2312U,
                    Name = "MT8696",
                    DaCode = 34454U,
                    WdgAddress = 268464128U,
                    PayloadAddress = 1051136U,
                    UartAddress = null,
                    PtrDl = null,
                    PtrDa = null,
                    Var1 = null,
                    SejBase = null,
                    PayloadFileName = null,
                    UseXFlash = true
                }
            };
        }
    }
}
