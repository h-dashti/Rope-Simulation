using System;
using System.Windows.Forms;

using Physics;
using OpenGL;

namespace RopeSimulation__GLUMethod
{
    public partial class FormRopeSimulation : Form
    {
        //static Form _this = null;

        private RopeControl gl = new RopeControl();

        public FormRopeSimulation()
        {
            InitializeComponent();
            ResizeRedraw = true;
            this.Load += new EventHandler(FormRopeSimulation_Load);
            this.Activated += new EventHandler(FormRopeSimulation_Activated);
            this.KeyPreview = true;
            
            gl.Parent = this;
            gl.Location = new System.Drawing.Point(0, 0);
            gl.Dock = DockStyle.Fill;


            string stStatusLable = "For motion:     X(Left-Right)     " +
            "Z(Up-Down)     Y(Home-End)      Release(Space)";
            toolStripStatusLabel1.Text = stStatusLable;
        }

        void FormRopeSimulation_Activated(object sender, EventArgs e)
        {
            gl.Focus();
        }


        // just for capture the control 
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                KeyEventArgs kea = new KeyEventArgs(keyData);
                gl.CallOnKeyDown(kea);
                return true;
            }
            
            return base.ProcessDialogKey(keyData);
        }
        void FormRopeSimulation_Load(object sender, EventArgs e)
        {
            //DialogResult key = MessageBox.Show(null, "Would You Like To Run In Fullscreen Mode?",
            //    "Start Fullscreen?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //if (key == DialogResult.Yes)
            //{
            //    this.FormBorderStyle = FormBorderStyle.None;
            //    this.Location = new System.Drawing.Point(0, 0);
            //    this.Size = Screen.PrimaryScreen.Bounds.Size;
            //}
        }

      
             
    }
}