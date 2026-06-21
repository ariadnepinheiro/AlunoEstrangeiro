using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Techne.Library
{
    internal class StrBlock
    {
        private readonly ArrayList m_List;

        private static int m_IndentSize = 2;

        private static string m_LineSep = "\r\n";

        private string description;

        private bool needTerminator = true;

        public StrBlock(string description)
        {
            this.Description = description;
            this.m_List = new ArrayList();
        }

        public StrBlock(string description, params string[] lines) : this(description)
        {
            Add(lines);
        }

        public StrBlock(string description, params StrBlock[] strBlocks) : this(description)
        {
            Add(strBlocks);
        }

        public StrBlock() : this(string.Empty)
        {
        }

        public static int IndentSize
        {
            get
            {
                return m_IndentSize;
            }

            set
            {
                m_IndentSize = value;
            }
        }

        public static string LineSep
        {
            get
            {
                return m_LineSep;
            }

            set
            {
                m_LineSep = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value == null ? string.Empty : value;
            }
        }

        public bool NeedTerminator
        {
            get
            {
                return this.needTerminator;
            }

            set
            {
                this.needTerminator = value;
            }
        }

        public static StrBlock ToStrBlock(string blockTerminator, params StrBlock[] strBlocks)
        {
            var b = new StrBlock();
            foreach (var blk in strBlocks)
            {
                b.Add(blk);
                if (blk.NeedTerminator)
                {
                    b.Add(blockTerminator);
                }
            }

            return b;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (BlockLine b in this.m_List)
            {
                if (sb.Length > 0)
                {
                    sb.Append(m_LineSep);
                }

                sb.Append(b.ToString());
            }

            return sb.ToString();
        }

        public void Add(params StrBlock[] blocks)
        {
            foreach (var block in blocks)
            {
                Add(block, 0, 0);
            }
        }

        public void Add(StrBlock block, int indent)
        {
            Add(block, indent, 0);
        }

        public void Add(StrBlock block, int indent, int offset)
        {
            if (block == null)
            {
                return;
            }

            foreach (BlockLine b in block.m_List)
            {
                this.Add(b.Line, indent + b.Indent, offset + b.Offset);
            }
        }

        public void Add()
        {
            this.Add(string.Empty, 0, 0);
        }

        public void Add(params string[] lines)
        {
            foreach (var line in lines)
            {
                Add(line, 0, 0);
            }
        }

        public void Add(string line, int indent)
        {
            Add(line, indent, 0);
        }

        public void Add(string line, int indent, int offset)
        {
            this.m_List.AddRange(tokenize(line, indent, offset));
        }

        public void Add(object[] blocks)
        {
            foreach (var o in blocks)
            {
                if (o == null)
                {
                    continue;
                }

                if (o is string)
                {
                    this.Add((string)o);
                }
                else if (o is StrBlock)
                {
                    this.Add((StrBlock)o);
                }
                else
                {
                    throw new Exception("Tipo não suportado no construtor de StrBlock");
                }
            }
        }

        public void Insert()
        {
            this.Insert(string.Empty, 0, 0);
        }

        [Obsolete("Utilize outro overload.")]
        public void Save(string FileName)
        {
            var f = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            var bufchar = this.ToString().ToCharArray();
            var buffer = new byte[bufchar.Length]; // = (byte[])this.ToString();
            for (var n = 0; n < bufchar.Length; n++)
            {
                buffer[n] = (byte)bufchar[n];
            }

            f.Write(buffer, 0, buffer.Length);
            f.Close();
        }

        public void Save(string fileName, Encoding encoding)
        {
            var writer = new StreamWriter(fileName, false, encoding);
            writer.Write(this.ToString());
            writer.Close();
        }

        private static ICollection tokenize(string line, int indent, int offset)
        {
            if (indent < 0)
            {
                throw new ArgumentException("O parâmetro indent deve ser não negativo", "indent");
            }

            if (offset < 0)
            {
                throw new ArgumentException("O parâmetro offset deve ser não negativo", "offset");
            }

            var lst = new ArrayList();

            do
            {
                var p = line.IndexOf(m_LineSep);
                if (p < 0)
                {
                    lst.Add(new BlockLine(line, indent, offset));
                    line = string.Empty;
                }
                else
                {
                    lst.Add(new BlockLine(line.Substring(0, p), indent, offset));
                    if (line.Substring(p) == m_LineSep)
                    {
                        lst.Add(new BlockLine(string.Empty, 0, 0)); // Trata separador no final
                    }

                    line = line.Substring(p + m_LineSep.Length);
                }
            }
            while (line.Length > 0);

            return lst;
        }

        /// <summary>
        ///   Foi criado como private só por não estar sendo usado.
        /// </summary>
        private void Insert(string line, int indent, int offset)
        {
            this.m_List.InsertRange(0, tokenize(line, indent, offset));
        }

        private struct BlockLine
        {
            private int m_Indent;

            private string m_Line;

            private int m_Offset;

            public BlockLine(string line, int indent, int offset)
            {
                this.m_Line = line;
                this.m_Indent = indent;
                this.m_Offset = offset;
            }

            public int Indent
            {
                get
                {
                    return this.m_Indent;
                }

                set
                {
                    this.m_Indent = value;
                }
            }

            public string Line
            {
                get
                {
                    return this.m_Line;
                }

                set
                {
                    this.m_Line = value;
                }
            }

            public int Offset
            {
                get
                {
                    return this.m_Offset;
                }

                set
                {
                    this.m_Offset = value;
                }
            }

            public override string ToString()
            {
                if (this.m_Line.Length > 0)
                {
                    return StrLib.Space(this.m_Indent * m_IndentSize + this.m_Offset) + this.m_Line;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}