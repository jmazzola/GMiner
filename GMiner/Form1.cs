using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace GMiner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // The file currently opened
        private string filePath = "";

        public enum SectionHeader : uint
        {
            Form        = 0x4D524F46, // FORM
            General     = 0x384E4547, // GEN8
            Options     = 0x4E54504F, // OPTN
            Extensions  = 0x4E545845, // EXTN
            Sounds      = 0x444E4F53, // SOND
            AudioGroup  = 0x50524741, // AGRP
            Sprites     = 0x54525053, // SPRT
            Backgrounds = 0x444E4742, // BGND
            Paths       = 0x48544150, // PATH
            Scripts     = 0x54504353, // SCPT
            Shaders     = 0x52444853, // SHDR
            Fonts       = 0x544E4F46, // FONT
            Timelines   = 0x4E4C4D54, // TMLN
            Objects     = 0x544A424F, // OBJT
            Rooms       = 0x4D4F4F52, // ROOM
            DataFiles   = 0x4C464144, // DAFL
            TexturePage = 0x47415054, // TPAG
            Code        = 0x45444F43, // CODE
            Variables   = 0x49524156, // VARI
            Functions   = 0x434E5546, // FUNC
            Strings     = 0x47525453, // STRG
            Textures    = 0x52545854, // TXTR
            Audio       = 0x4F445541, // AUDO

            Count = 23,
         }

        public enum GeneralOptionInfoFlags : uint
        {
            Fullscreen = 0x1,
            SyncVertex_And1 = 0x2,              // (class5_0.Options.Sync_Vertex & 1) != 0
            SyncVertex_AndNegMaxNum = 0x4,      // ((long)class5_0.Options.Sync_Vertex & (ulong)-2147483648) != (long)0
            InterpolatePixels = 0x8,
            NegativeScale = 0x10,
            ShowCursor = 0x20,
            Sizeable = 0x40,
            ScreenKey = 0x80,
            SyncVertex_And4Million = 0x100,     // (class5_0.Options.Sync_Vertex & 0x40000000) != 0
            StudioEdition_Equals1 = 0x200,
            StudioEdition_Equals2 = 0x400,
            StudioEdition_Equals3 = 0x800,
            StudioEdition_Equals4 = 0x600,
            SteamProject_OrYoyoPlayer = 0x1000, // if (Class168.SteamProject) || if (Class168.MachineType.Name == "YoYoPlayer")
            SaveLocation = 0x2000,
            Borderless = 0x4000,
        }

        public struct Chunk
        {
            SectionHeader header;
            uint sizeOfChunk;
        }

        public unsafe struct GeneralInfo
        {
            public Chunk chunk;
            public int debug;                                   // (class5_0.Debug ? 0 : 1) | 0xe00
            public int fileNameWithoutExtensionOffset;     
            public int optionsConfigNameOffset;                 // class5_0.Options.Config
            public int roomMaxID;
            public int roomMaxTileID;
            public int gameID;
            public fixed uint padding0[4];
            public int gameNameOffset;
            public int majorVersion;
            public int minorVersion;
            public int releaseVersion;
            public int buildVersion;
            public Point windowSize;
            public GeneralOptionInfoFlags flags;
            public int CRC;
            public fixed byte CRC32Checksum[16];
            public double totalSecondsTimestamp;
            public int displayNameOffset;
            public int activeTargets_AndNeg1;                   // class5_0.Options.ActiveTargets & (ulong)-1
            public int activeTargets_SHR32AndNeg1;              // class5_0.Options.ActiveTargets >> 32 & (ulong)-1
            public int functionClassifications_AndNeg1;         // class5_0.FunctionClassifications & (ulong)-1
            public int functionClassifications_SHR32AndNeg1;    // class5_0.FunctionClassifications & (ulong)-1
            public int steamAppID;
            public int debuggerPort;
            public int roomOrderCount;
            public int roomNumbers;                             // It literally writes 0-roomCount for some reason.
        }

        private void UpdateStatus(string status)
        {
            toolStripStatusLabel1.Text = status;
            statusStrip1.Update();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            filePath = ofd.FileName;

            if (filePath != "")
                ParseFile();
        }

        private void ParseFile()
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(filePath)))
            {
                if (br.ReadInt32() != (int)SectionHeader.Form)
                {
                    UpdateStatus("Not a compatible .win file.");
                }

            }
        }
    }
}
