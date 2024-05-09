using PTCL_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PTCL_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public PTCL.SPBD SPBDData { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_CTPK = new OpenFileDialog()
            {
                Title = "Open PTCL (3DS)",
                //InitialDirectory = @"C:\Users\User\Desktop",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "ptcl file|*.ptcl"
            };

            if (Open_CTPK.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(Open_CTPK.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                SPBDData = new PTCL.SPBD();
                SPBDData.Read_SPBD(br, EndianConvert.GetEnumEndianToBytes(EndianConvert.Endian.LittleEndian));

                br.Close();
                fs.Close();


                #region TreeView
                EmitterSetTreeView.HideSelection = false;

                List<TreeNode> EmitterDataSetTreeNodeList = new List<TreeNode>();
                foreach (var item in SPBDData.EmitterDataSet_List)
                {
                    List<TreeNode> EmitterDataTreeNodeList = new List<TreeNode>();
                    foreach (var data in item.EmitterData_List)
                    {
                        EmitterDataTreeNodeList.Add(new TreeNode(data.Unknown_Data.EmitterDataName));
                    }

                    EmitterDataSetTreeNodeList.Add(new TreeNode(item.EmitterDataSetName, EmitterDataTreeNodeList.ToArray()));
                }

                TreeNode treeNode = new TreeNode("Root");
                treeNode.Nodes.Add(new TreeNode("EmitterDataSet", EmitterDataSetTreeNodeList.ToArray()));



                EmitterSetTreeView.Nodes.Add(treeNode);
                EmitterSetTreeView.TopNode.Expand();
                #endregion
            }
        }

        private void EmitterSetTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (EmitterSetTreeView.SelectedNode != null)
            {
                EmitterSetTreeView.PathSeparator = ",";

                string[] Set = EmitterSetTreeView.SelectedNode.FullPath.Split(',');

                if (Set.Length == 1)
                {

                }
                if (Set.Length == 2)
                {

                }
                if (Set.Length == 3)
                {
                    var n = SPBDData.EmitterDataSet_List.Find(x => x.EmitterDataSetName == Set[2]);

                    propertyGrid1.SelectedObject = n;
                }
                if (Set.Length == 4)
                {
                    var n = SPBDData.EmitterDataSet_List.Find(x => x.EmitterDataSetName == Set[2]).EmitterData_List.Find(y => y.Unknown_Data.EmitterDataName == Set[3]).Unknown_Data;

                    propertyGrid1.SelectedObject = n;
                }
            }
        }
    }
}
