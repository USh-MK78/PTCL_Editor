using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PTCL_Library
{

    public class PTCL
    {
        /// <summary>
        /// CTR Version
        /// </summary>
        public class SPBD
        {
            public string Name;

            public char[] SPBD_Header { get; set; }
            public byte[] Version { get; set; }
            public int EmitterDataSetCount { get; set; }

            public int NameOffset { get; set; } // Starts from the beginning of the string table.
            public int StringDataOffset { get; set; }
            public int ImageDataOffset { get; set; }
            public int ImageDataSize { get; set; }

            public List<EmitterDataSet> EmitterDataSet_List { get; set; }
            public class EmitterDataSet
            {
                public byte[] UnknownByteData1 { get; set; } //0x4

                public UnknownByteData Unknown_ByteData { get; set; }
                public class UnknownByteData
                {
                    public byte Data1 { get; set; }
                    public byte Data2 { get; set; }
                    public byte Data3 { get; set; }
                    public byte Data4 { get; set; }

                    public void Read_UnknownByteData(BinaryReader br)
                    {
                        Data1 = br.ReadByte();
                        Data2 = br.ReadByte();
                        Data3 = br.ReadByte();
                        Data4 = br.ReadByte();
                    }

                    public UnknownByteData()
                    {
                        Data1 = 0x00;
                        Data2 = 0x00;
                        Data3 = 0x00;
                        Data4 = 0x00;
                    }
                }

                //0x4, From : CharArrayOffset
                public int EmitterDataSetNameCharArrayOffset { get; set; }
                //public char[] EmitterDataSetCharArray { get; set; }
                //public string EmitterDataSetName => new string(EmitterDataSetCharArray);
                public string EmitterDataSetName { get; set; }

                public byte[] UnknownByteData2 { get; set; } //0x4
                public int EmitterCount { get; set; }

                public int EmitterDataOffset { get; set; } //0x4, From : SPBD Header

                public List<EmitterData> EmitterData_List { get; set; }
                public class EmitterData
                {
                    public int UnknownOffset { get; set; } //0x4, From : SPBD Header

                    public UnknownData Unknown_Data { get; set; }
                    public class UnknownData
                    {
                        public int UnknownData1 { get; set; }
                        public int UnknownData2 { get; set; }
                        public int UnknownData3 { get; set; }


                        public int EmitterDataNameCharArrayOffset { get; set; } //CharArrayOffset
                        public string EmitterDataName { get; set; }

                        public int UnknownData5 { get; set; }

                        public short ImageHeight { get; set; }
                        public short ImageWidth { get; set; }

                        public short ETC1_ImageFormat { get; set; }
                        public short UnknownShortData4 { get; set; } //MipCount (?)
                        //public byte[] UnknownByteArray1 { get; set; } //0x4
                        public byte[] UnknownByteArray2 { get; set; } //0x4

                        public int UnknownAreaOffset1 { get; set; }

                        public UnknownArea UnknownAreaData { get; set; }
                        public class UnknownArea
                        {
                            public UnknownColorData UnknownColorData1 { get; set; }
                            public UnknownColorData UnknownColorData2 { get; set; }
                            public UnknownColorData UnknownColorData3 { get; set; }
                            public UnknownColorData UnknownColorData4 { get; set; }
                            public UnknownColorData UnknownColorData5 { get; set; }
                            public UnknownColorData UnknownColorData6 { get; set; }
                            public UnknownColorData UnknownColorData7 { get; set; }
                            public UnknownColorData UnknownColorData8 { get; set; }

                            public class UnknownColorData
                            {
                                public float Data1 { get; set; }
                                public float Data2 { get; set; }
                                public float Data3 { get; set; }
                                public float Data4 { get; set; }

                                public System.Windows.Media.Color GetColorData => GetColor();
                                public System.Windows.Media.Color GetColor()
                                {
                                    return System.Windows.Media.Color.FromScRgb(Data1, Data2, Data3, Data4);
                                }

                                public void Read_UnknownColorData(BinaryReader br, byte[] BOM)
                                {
                                    EndianConvert endianConvert = new EndianConvert(BOM);
                                    Data1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    Data2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    Data3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                    Data4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                }

                                public UnknownColorData()
                                {
                                    Data1 = 0;
                                    Data2 = 0;
                                    Data3 = 0;
                                    Data4 = 0;
                                }
                            }

                            public void Read_UnknownArea(BinaryReader br, byte[] BOM)
                            {
                                UnknownColorData1.Read_UnknownColorData(br, BOM);
                                UnknownColorData2.Read_UnknownColorData(br, BOM);
                                UnknownColorData3.Read_UnknownColorData(br, BOM);
                                UnknownColorData4.Read_UnknownColorData(br, BOM);
                                UnknownColorData5.Read_UnknownColorData(br, BOM);
                                UnknownColorData6.Read_UnknownColorData(br, BOM);
                                UnknownColorData7.Read_UnknownColorData(br, BOM);
                                UnknownColorData8.Read_UnknownColorData(br, BOM);
                            }

                            //public UnknownColorData1 UnknownColorData1 { get; set; }
                            //public class UnknownColorData1
                            //{
                            //    public float Data1 { get; set; }
                            //    public float Data2 { get; set; }
                            //    public float Data3 { get; set; }
                            //    public float Data4 { get; set; }

                            //    public void Read_UnknownColorData1(BinaryReader br, byte[] BOM)
                            //    {
                            //        EndianConvert endianConvert = new EndianConvert(BOM);
                            //        Data1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //        Data2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //        Data3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //        Data4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            //    }

                            //    public UnknownColorData1()
                            //    {
                            //        Data1 = 0;
                            //        Data2 = 0;
                            //        Data3 = 0;
                            //        Data4 = 0;
                            //    }
                            //}

                            public UnknownArea()
                            {
                                UnknownColorData1 = new UnknownColorData();
                                UnknownColorData2 = new UnknownColorData();
                                UnknownColorData3 = new UnknownColorData();
                                UnknownColorData4 = new UnknownColorData();
                                UnknownColorData5 = new UnknownColorData();
                                UnknownColorData6 = new UnknownColorData();
                                UnknownColorData7 = new UnknownColorData();
                                UnknownColorData8 = new UnknownColorData();
                            }
                        }

                        public byte[] UnknownByteArray3 { get; set; } //0x4
                        public byte[] UnknownByteArray4 { get; set; } //0x4

                        public int UnknownAreaOffset2 { get; set; }

                        public byte[] UnknownByteArray5 { get; set; } //0x4

                        public UnknownColorData UnknownColorData1 { get; set; }
                        public class UnknownColorData
                        {
                            public float Data1 { get; set; }
                            public float Data2 { get; set; }
                            public float Data3 { get; set; }
                            public float Data4 { get; set; }

                            public System.Windows.Media.Color GetColorData => GetColor();
                            public System.Windows.Media.Color GetColor()
                            {
                                return System.Windows.Media.Color.FromScRgb(Data1, Data2, Data3, Data4);
                            }

                            public void Read_UnknownColorData(BinaryReader br, byte[] BOM)
                            {
                                EndianConvert endianConvert = new EndianConvert(BOM);
                                Data1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                Data2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                Data3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                                Data4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            }

                            public UnknownColorData()
                            {
                                Data1 = 0;
                                Data2 = 0;
                                Data3 = 0;
                                Data4 = 0;
                            }
                        }

                        public byte[] UnknownByteArray6 { get; set; } //0x4

                        public UnknownColorData UnknownColorData2 { get; set; }
                        public UnknownColorData UnknownColorData3 { get; set; }
                        public UnknownColorData UnknownColorData4 { get; set; }

                        public int UnknownIntValue1 { get; set; }
                        public int UnknownIntValue2 { get; set; }
                        public int UnknownIntValue3 { get; set; }
                        public int UnknownIntValue4 { get; set; }


                        public byte[] UnknownByteArray7 { get; set; } //0x4
                        public float UnknownFloatValue1 { get; set; }
                        public byte[] UnknownByteArray8 { get; set; } //0x4
                        public int UnknownIntValue5 { get; set; }

                        public byte[] UnknownByteArray9 { get; set; } //0x4
                        public byte[] UnknownByteArray10 { get; set; } //0x4


                        public void Read_UnknownData(BinaryReader br, byte[] BOM, long SPBDPos, int StringDataOffset)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            EmitterDataNameCharArrayOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //CharArrayOffset
                            if (StringDataOffset != 0)
                            {
                                long CurrentPos = br.BaseStream.Position;

                                //Move SPBD
                                br.BaseStream.Position = SPBDPos;

                                br.BaseStream.Seek(StringDataOffset + EmitterDataNameCharArrayOffset, SeekOrigin.Current);

                                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                                readByteLine.ReadByte(br, 0x00);
                                EmitterDataName = new string(readByteLine.ConvertToCharArray());

                                //Leave CurrentPos
                                br.BaseStream.Position = CurrentPos;
                            }

                            UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            ImageHeight = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            ImageWidth = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

                            ETC1_ImageFormat = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownShortData4 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            //UnknownByteArray1 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownByteArray2 = endianConvert.Convert(br.ReadBytes(4));

                            UnknownAreaOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (UnknownAreaOffset1 != 0)
                            {
                                long CurrentPos = br.BaseStream.Position;

                                br.BaseStream.Seek(-4, SeekOrigin.Current);

                                br.BaseStream.Seek(UnknownAreaOffset1, SeekOrigin.Current);

                                //Read UnknownArea
                                UnknownAreaData.Read_UnknownArea(br, BOM);

                                br.BaseStream.Position = CurrentPos;

                            }

                            UnknownByteArray3 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownByteArray4 = endianConvert.Convert(br.ReadBytes(4));

                            UnknownAreaOffset2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            UnknownByteArray5 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownColorData1.Read_UnknownColorData(br, BOM);
                            UnknownByteArray6 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownColorData2.Read_UnknownColorData(br, BOM);
                            UnknownColorData3.Read_UnknownColorData(br, BOM);
                            UnknownColorData4.Read_UnknownColorData(br, BOM);

                            UnknownIntValue1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownIntValue2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownIntValue3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownIntValue4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            UnknownByteArray7 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownFloatValue1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownByteArray8 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownIntValue5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownByteArray9 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownByteArray10 = endianConvert.Convert(br.ReadBytes(4));
                        }

                        public UnknownData()
                        {
                            UnknownData1 = 0;
                            UnknownData2 = 0;
                            UnknownData3 = 0;
                            EmitterDataNameCharArrayOffset = 0;
                            EmitterDataName = "";
                            UnknownData5 = 0;

                            ImageHeight = 0; //ImageSize, Height
                            ImageWidth = 0; //ImageSize, WIdth

                            ETC1_ImageFormat = 0;
                            UnknownShortData4 = 0;
                            //UnknownByteArray1 = new byte[4];
                            UnknownByteArray2 = new byte[4];

                            UnknownAreaOffset1 = 0;
                            UnknownAreaData = new UnknownArea();

                            UnknownByteArray3 = new byte[4];
                            UnknownByteArray4 = new byte[4];

                            UnknownAreaOffset2 = 0;

                            UnknownByteArray5 = new byte[4];
                            UnknownColorData1 = new UnknownColorData();
                            UnknownByteArray6 = new byte[4];
                            UnknownColorData2 = new UnknownColorData();
                            UnknownColorData3 = new UnknownColorData();
                            UnknownColorData4 = new UnknownColorData();

                            UnknownIntValue1 = 0;
                            UnknownIntValue2 = 0;
                            UnknownIntValue3 = 0;
                            UnknownIntValue4 = 0;

                            UnknownByteArray7 = new byte[4];
                            UnknownFloatValue1 = 0;
                            UnknownByteArray8 = new byte[4];
                            UnknownIntValue5 = 0;
                            UnknownByteArray9 = new byte[4];
                            UnknownByteArray10 = new byte[4];
                        }

                        public override string ToString()
                        {
                            return EmitterDataName;
                        }
                    }

                    public int UnknownOffset2 { get; set; }

                    public void Read_UnknownData(BinaryReader br, byte[] BOM, long SPBDPos, int StringDataOffset)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (UnknownOffset != 0)
                        {
                            long CurrentPos = br.BaseStream.Position;

                            //Move SPBD
                            br.BaseStream.Position = SPBDPos;

                            br.BaseStream.Seek(UnknownOffset, SeekOrigin.Begin);

                            //Read UnknownData
                            Unknown_Data.Read_UnknownData(br, BOM, SPBDPos, StringDataOffset);

                            //Leave CurrentPos
                            br.BaseStream.Position = CurrentPos;
                        }

                        UnknownOffset2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public EmitterData()
                    {
                        UnknownOffset = 0;
                        Unknown_Data = new UnknownData();
                        UnknownOffset2 = 0;
                    }
                }

                public byte[] UnknownByteData3 { get; set; } //0x4

                public void Read_EmitterDataSet(BinaryReader br, byte[] BOM, long SPBDPos, int StringDataOffset)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownByteData1 = endianConvert.Convert(br.ReadBytes(4));
                    Unknown_ByteData.Read_UnknownByteData(br);
                    EmitterDataSetNameCharArrayOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (StringDataOffset != 0)
                    {
                        long CurrentPos = br.BaseStream.Position;

                        //Move SPBD
                        br.BaseStream.Position = SPBDPos;

                        br.BaseStream.Seek(StringDataOffset + EmitterDataSetNameCharArrayOffset, SeekOrigin.Current);

                        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                        readByteLine.ReadByte(br, 0x00);
                        EmitterDataSetName = new string(readByteLine.ConvertToCharArray());

                        //Leave CurrentPos
                        br.BaseStream.Position = CurrentPos;
                    }


                    UnknownByteData2 = endianConvert.Convert(br.ReadBytes(4));
                    EmitterCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    EmitterDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (EmitterDataOffset != 0)
                    {
                        long CurrentPos = br.BaseStream.Position;

                        //Move SPBD
                        br.BaseStream.Position = SPBDPos;

                        br.BaseStream.Seek(EmitterDataOffset, SeekOrigin.Current);

                        for (int i = 0; i < EmitterCount; i++)
                        {
                            EmitterData emitterData = new EmitterData();
                            emitterData.Read_UnknownData(br, BOM, SPBDPos, StringDataOffset);
                            EmitterData_List.Add(emitterData);
                        }

                        //Leave CurrentPos
                        br.BaseStream.Position = CurrentPos;
                    }

                    UnknownByteData3 = endianConvert.Convert(br.ReadBytes(4));
                }

                public EmitterDataSet()
                {
                    UnknownByteData1 = new byte[4];
                    Unknown_ByteData = new UnknownByteData();
                    EmitterDataSetNameCharArrayOffset = 0;
                    EmitterDataSetName = "";
                    UnknownByteData2 = new byte[4];
                    EmitterCount = 0;
                    EmitterDataOffset = 0;
                    EmitterData_List = new List<EmitterData>();
                    UnknownByteData3 = new byte[4];
                }
            }

            public byte[] ETC1_ImageData { get; set; }

            public void Read_SPBD(BinaryReader br, byte[] BOM)
            {
                long SPBDPos = br.BaseStream.Position;

                SPBD_Header = br.ReadChars(4);
                if (new string(SPBD_Header) != "SPBD") throw new Exception("Unknown Format.");

                EndianConvert endianConvert = new EndianConvert(BOM);
                Version = endianConvert.Convert(br.ReadBytes(4));
                EmitterDataSetCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                StringDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (StringDataOffset != 0)
                {
                    //ReadName
                    long CurrentPos = br.BaseStream.Position;

                    //Move SPBD
                    br.BaseStream.Position = SPBDPos;

                    br.BaseStream.Seek(StringDataOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);
                    Name = new string(readByteLine.ConvertToCharArray());

                    //Leave CurrentPos
                    br.BaseStream.Position = CurrentPos;
                }

                ImageDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                ImageDataSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (ImageDataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(0, SeekOrigin.Begin);

                    br.BaseStream.Seek(ImageDataOffset, SeekOrigin.Current);

                    ETC1_ImageData = br.ReadBytes(ImageDataSize);

                    br.BaseStream.Position = Pos;
                }

                if ((EmitterDataSetCount != 0 && 0 < EmitterDataSetCount) == true)
                {
                    for (int i = 0; i < EmitterDataSetCount; i++)
                    {
                        EmitterDataSet emitterDataSet = new EmitterDataSet();
                        emitterDataSet.Read_EmitterDataSet(br, BOM, SPBDPos, StringDataOffset);
                        EmitterDataSet_List.Add(emitterDataSet);
                    }
                }


            }

            public SPBD()
            {
                Name = "";

                SPBD_Header = "SPBD".ToArray();
                Version = new byte[4];
                EmitterDataSetCount = 0;
                EmitterDataSet_List = new List<EmitterDataSet>();
                NameOffset = 0;
                StringDataOffset = 0;
                ImageDataOffset = 0;
                ImageDataSize = 0;

                ETC1_ImageData = new byte[ImageDataSize];
            }
        }

        /// <summary>
        /// Cafe Version
        /// </summary>
        public class EFTF
        {

        }

    }
}
