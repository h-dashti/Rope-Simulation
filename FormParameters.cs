using System;
using System.Windows.Forms;

namespace RopeSimulation__GLUMethod
{
   
    public partial class FormParameters : Form
    {
        # region public Fields
        public bool isChangedParameters;
        public int numOfMasses;
        public double m;
        public double springConstant;
        public double springRestLength;
        public double springFrictionConstant;
        public double gravitationConstant;
        public double airFrictionConstant;
        public double groundRepulsionConstant;
        public double groundFrictionConstant;
        public double groundAbsorptionConstant;
        public double dt;
        public int numOfIterations;

        # endregion

        # region Constructor
        public FormParameters(
            int numOfMasses,
            double m,
            double springConstant,
            double springRestLength,
            double springFrictionConstant,
            double gravitationConstant,
            double airFrictionConstant,
            double groundRepulsionConstant,
            double groundFrictionConstant,
            double groundAbsorptionConstant,  
            double dt, int numOfIterations)
        {
            InitializeComponent();
            this.numOfMasses = numOfMasses;
            this.m = m;
            this.springConstant = springConstant;
            this.springRestLength = springRestLength;
            this.springFrictionConstant = springFrictionConstant;
            this.gravitationConstant = gravitationConstant;
            this.airFrictionConstant = airFrictionConstant;
            this.groundRepulsionConstant = groundRepulsionConstant;
            this.groundFrictionConstant = groundFrictionConstant;
            this.groundAbsorptionConstant = groundAbsorptionConstant;
            this.dt = dt;
            this.numOfIterations = numOfIterations;

            this.isChangedParameters = false;
            SetTextBoxes();
        }
        # endregion

        # region private Methods
        private void DefaultParameters()
        {
            numOfMasses = 80;
            m = 0.05;
            springConstant = 10000.0;
            springRestLength = 0.05;
            springFrictionConstant = 0.2;
            gravitationConstant = 9.81;
            airFrictionConstant = 0.02;
            groundRepulsionConstant = 100.0;
            groundFrictionConstant = 0.2;
            groundAbsorptionConstant = 2.0;

            dt = 0.001;
            numOfIterations = 65;
        }

        private void SetTextBoxes()
        {
            textBoxNumberMases.Text = numOfMasses.ToString();
            textBoxWeightMass.Text = m.ToString();
            textBoxSpringConstant.Text = springConstant.ToString();
            textBoxRestLength.Text = springRestLength.ToString();
            textBoxSpringFriction.Text = springFrictionConstant.ToString();
            textBoxGravitationConstant.Text = gravitationConstant.ToString();
            textBoxAirFriction.Text = airFrictionConstant.ToString();
            textBoxGroundRepulsion.Text = groundRepulsionConstant.ToString();
            textBoxGroundFriction.Text = groundFrictionConstant.ToString();
            textBoxGroundAbsoption.Text = groundAbsorptionConstant.ToString();
            textBoxDt.Text = dt.ToString();
            textBoxNumOfIteration.Text = numOfIterations.ToString();
        }
        #  endregion

        # region Methods of Delegates
        private void buttonOk_Click(object sender, EventArgs e)
        {
            numOfMasses = int.Parse(textBoxNumberMases.Text);
            m = double.Parse(textBoxWeightMass.Text);
            springConstant = double.Parse(textBoxSpringConstant.Text);
            springRestLength = double.Parse(textBoxRestLength.Text);
            springFrictionConstant = double.Parse(textBoxSpringFriction.Text);
            gravitationConstant = double.Parse(textBoxGravitationConstant.Text);
            airFrictionConstant = double.Parse(textBoxAirFriction.Text);
            groundRepulsionConstant = double.Parse(textBoxGroundRepulsion.Text);
            groundFrictionConstant = double.Parse(textBoxGroundFriction.Text);
            groundAbsorptionConstant = double.Parse(textBoxGroundAbsoption.Text);
            dt = double.Parse(textBoxDt.Text);
            numOfIterations = int.Parse(textBoxNumOfIteration.Text);

            isChangedParameters = true;
            this.Close();
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            DefaultParameters();
            SetTextBoxes();

        }

        private void FormParameters_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        # endregion
    }
}