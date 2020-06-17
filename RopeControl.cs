//************************************************************************
//
// by H.dashti 
// email : ebi_2000_9@yahoo.com
//**************************************************************************

using System;
using System.Windows.Forms;
using RopeSimulation__GLUMethod;
using Physics;


namespace OpenGL
{
    public class RopeControl : BaseGLControl
    {
        # region variables
        private System.ComponentModel.Container components = null;

        private Timer operateTimer = new Timer();
        private double dt = 0.001;     // the step time for simulation
        private double scale = 1;     // scaling for drwing in screen
        private int numOfIteration = 70;
        public RopeSimulation ropeSimulation;
        # endregion


        # region Constructor
        public RopeControl(RopeSimulation ropeSimulation)
        {
            InitializeComponent();
            this.ropeSimulation = ropeSimulation;
            ropeSimulation.GetMass(ropeSimulation.numOfMass - 1).vel.z = 10;

            operateTimer.Interval = 50;
            operateTimer.Tick += new EventHandler(operateTimer_Tick);
            operateTimer.Start();
        }
        public RopeControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            ropeSimulation = new RopeSimulation(   new Vector3D(0, 0, 0),   // conection position of Rope
                                                   80,						// 80 Particles (Masses)
                                                   0.05,					// Each Particle Has A Weight Of 50 Grams
                                                   10000.0,				// springConstant In The Rope
                                                   0.05,					// Normal Length Of Springs In The Rope
                                                   0.2,					// Spring Inner Friction Constant
                                                   new Vector3D(0, -9.81, 0), // Gravitational Acceleration
                                                   0.02,					// Air Friction Constant
                                                   100.0,					// Ground Repel Constant
                                                   0.2,					// Ground Slide Friction Constant
                                                   2.0,					// Ground Absoption Constant
                                                   -1.5,					// Height Of Ground
                                                   false);              // is rope, released?
            ropeSimulation.GetMass(ropeSimulation.numOfMass - 1).vel.z = 10;


            operateTimer.Interval = 50;
            operateTimer.Tick += new EventHandler(operateTimer_Tick);
            operateTimer.Start();
            

        }
        # endregion


        # region private Methods
        
        
        void operateTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i <= numOfIteration; i++)
                ropeSimulation.Operate(dt);

            this.Invalidate();
        }

        private void ShowingFormParameter()
        {
            FormParameters formParameters = new FormParameters(
               ropeSimulation.numOfMass,
               ropeSimulation.masses[0].m,
               ropeSimulation.springs[0].springConstant,
               ropeSimulation.springs[0].springRestLenght,
               ropeSimulation.springs[0].springFrictionConstant,
               -ropeSimulation.gravitation.y,
               ropeSimulation.airFrictionConstant,
               ropeSimulation.groundRepulsionConstant,
               ropeSimulation.groundFrictionConstant,
               ropeSimulation.groundAbsorptionConstant,
               dt, numOfIteration);

            formParameters.ShowDialog();
            if (formParameters.isChangedParameters)
            {
                ropeSimulation = new RopeSimulation(
                  new Vector3D(0, 0, 0),   // conection position of Rope
                  formParameters.numOfMasses,// Particles (Masses)
                  formParameters.m,		// Each Particle Has A Weight Of 50 Grams
                  formParameters.springConstant,		// springConstant In The Rope
                  formParameters.springRestLength,		// Normal Length Of Springs In The Rope
                  formParameters.springFrictionConstant,// Spring Inner Friction Constant
                  new Vector3D(0, -formParameters.gravitationConstant, 0), // Gravitational Acceleration
                  formParameters.airFrictionConstant,   // Air Friction Constant
                  formParameters.groundRepulsionConstant,	// Ground Repel Constant
                  formParameters.groundFrictionConstant,    // Ground Slide Friction Constant
                  formParameters.groundAbsorptionConstant,	// Ground Absoption Constant
                  -1.5,					// Height Of Ground
                  false);

                ropeSimulation.GetMass(ropeSimulation.numOfMass - 1).vel.z = 10;

                dt = formParameters.dt;
                numOfIteration = formParameters.numOfIterations;
            }
        }

        # endregion

        # region public Methods
        public void CallOnKeyDown(KeyEventArgs kea)
        {
            OnKeyDown(kea);
        }
        # endregion

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion


        # region overriden Method

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void InitializeGL(object sender, EventArgs e)
        {
            base.InitializeGL(sender, e);
            GL.glClearColor(0f, 0f, 0f, 0.5f);
            GL.glClearDepth(1.0f);
            GL.glShadeModel(GL.GL_SMOOTH);
            GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
           
            if (DC == 0 || RC == 0)
                return;
            WGL.wglMakeCurrent(DC, RC); 
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
            GLU.gluLookAt(0, 0, 6, 0, 0, 0, 0, 1, 0);   // default Eye is in ponit(0,0,0)

           

            # region  Plot here

            GL.glBegin(GL.GL_QUADS);  // Draw A Plane To Represent The Ground (Different Colors To Create A Fade)
            GL.glColor3ub(0, 0, 255);
            GL.glVertex3d(20, ropeSimulation.groundHeight * scale, 20);
            GL.glVertex3d(-20, ropeSimulation.groundHeight * scale, 20);

            GL.glColor3ub(0, 0, 0);
            GL.glVertex3d(-20, ropeSimulation.groundHeight * scale, -20);
            GL.glVertex3d(20, ropeSimulation.groundHeight * scale, -20);
            GL.glEnd();


            // Draw shadow of Rope
            GL.glColor3ub(0, 0, 0);
            for (int i = 0; i < ropeSimulation.numOfMass - 1; i++)
            {
                Vector3D pos1 = ropeSimulation.GetMass(i).pos;
                Vector3D pos2 = ropeSimulation.GetMass(i + 1).pos;

                GL.glLineWidth(2f);
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(pos1.x * scale, ropeSimulation.groundHeight * scale, pos1.z * scale); // Shadoe at GroundHeight
                GL.glVertex3d(pos2.x * scale, ropeSimulation.groundHeight * scale, pos2.z * scale);
                GL.glEnd();

            }
            // End Draw Shadow of Rope


            // Drawin Rope
            GL.glColor3ub(255, 255, 0);
            for (int i = 0; i < ropeSimulation.numOfMass - 1; i++)
            {
                Vector3D pos1 = ropeSimulation.GetMass(i).pos;
                Vector3D pos2 = ropeSimulation.GetMass(i + 1).pos;

                GL.glLineWidth(4.0f);
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(pos1.x * scale, pos1.y * scale, pos1.z * scale);
                GL.glVertex3d(pos2.x * scale, pos2.y * scale, pos2.z * scale);
                GL.glEnd();

            }// End Drwaing  the Rope

            # endregion

            GL.glFlush();													// Flush The GL Rendering Pipeline
            WGL.wglSwapBuffers(DC);

        }


        protected override void OnKeyDown(KeyEventArgs kea)
        {
            Vector3D ropeConnectionVel = new Vector3D();
            double speed = 2.5;
            Keys key = kea.KeyCode;

            if (key == Keys.Escape)
                Application.Exit();
            else if (key == Keys.F2)
            {
                ShowingFormParameter();
            }
            else if (key == Keys.D || key == Keys.Right)
            {
                ropeConnectionVel.x += speed;
                ropeSimulation.isReleasedRope = false;
            }
            else if (key == Keys.A || key == Keys.Left)
            {
                ropeConnectionVel.x -= speed;
                ropeSimulation.isReleasedRope = false;
            }

            else if (key == Keys.W || key == Keys.Up)
            {
                ropeConnectionVel.z -= speed;
                ropeSimulation.isReleasedRope = false;
            }
            else if (key == Keys.S || key == Keys.Down)
            {
                ropeConnectionVel.z += speed;
                ropeSimulation.isReleasedRope = false;
            }
            else if (key == Keys.Home)
            {
                ropeConnectionVel.y += speed;
                ropeSimulation.isReleasedRope = false;
            }
            else if (key == Keys.End)
            {
                ropeConnectionVel.y -= speed;
                ropeSimulation.isReleasedRope = false;
            }
            else if (key == Keys.Space)
            {
                ropeSimulation.isReleasedRope = true;

            }
           


            ropeSimulation.setRopeConnectionVel(ropeConnectionVel);

            this.Invalidate();
        }


        protected override void OnKeyUp(KeyEventArgs kea)
        {
            ropeSimulation.setRopeConnectionVel(new Vector3D());
            
        }
        
        # endregion

    }
}
