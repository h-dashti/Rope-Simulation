//************************************************************************
//
// by H.dashti 
// email : ebi_2000_9@yahoo.com
//**************************************************************************

namespace Physics
{
    public class Mass
    {
        public double m;									// The mass value
        public Vector3D pos;								// Position in space
        public Vector3D vel;								// Velocity
        public Vector3D force;								// Force applied on this mass at an instance

        public Mass(double m)
        {
            this.m = m;
            this.pos = new Vector3D();
            this.vel = new Vector3D();
            this.force = new Vector3D();
        }

        public void ApplyForce(Vector3D force)
        {
            this.force += force;
           
        }
        public void ResetForce()
        {
            this.force = new Vector3D();
        }
        public void Simulate(double dt)
        {
            EulerMethod2(dt);
        }
        private void EulerMethod(double dt)
        {
            this.vel += (this.force / this.m) * dt;				// Change in velocity is added to the velocity.
            // The change is proportinal with the acceleration (force / m) and change in time

            this.pos += this.vel * dt;						// Change in position is added to the position.
            // Change in position is velocity times the change in time

        }
        private void EulerMethod2(double dt)
        {
            this.vel += (this.force / this.m) * dt;
            this.pos += this.vel * dt + 0.5 * (this.force / this.m) * dt * dt;
           
        }
        
    }// End Class Mass
    //________________________________________________________________________________________
    //________________________________________________________________________________________

    public class ParticlesSimulation
    {
        public Mass[] masses;
        public int numOfMass;

        public ParticlesSimulation(double m, int numOfMass)
        {
            this.numOfMass = numOfMass;
            this.masses = new Mass[numOfMass];
            for (int i = 0; i < numOfMass; i++)
                masses[i] = new Mass(m);
        }
        public Mass GetMass(int index)
        {
            if (index < 0 || index >= numOfMass)
                return null;
            return masses[index];
        }

        public virtual void Init()
        {
            for (int i = 0; i <numOfMass; i++)
                this.masses[i].ResetForce();
        }
        public virtual void Solve()
        {
        }
        public virtual void Simulate(double dt)
        {
            for (int i = 0; i <numOfMass; i++)
                this.masses[i].Simulate(dt);
        }
        public virtual void Operate(double dt)
        {
            Init();
            Solve();
            Simulate(dt);
        }
    }// End class ParticlesSimulation

    //________________________________________________________________________________________
    //________________________________________________________________________________________

    public class Spring
    {
        public Mass mass1, mass2;
        public double springConstant;                   //A constant to represent the stiffness of the 
        public double springRestLenght;
        public double springFrictionConstant;        	//A constant to be used for the inner friction of the spring
       

        public Spring(Mass mass1, Mass mass2, double springConstant, double springRestLenght,
            double springFrictionConstant)
        {

            this.mass1 = mass1;
            this.mass2 = mass2;
            this.springConstant = springConstant;
            this.springRestLenght = springRestLenght;
            this.springFrictionConstant = springFrictionConstant;
        }

        public void Solve()
        {
            Vector3D springVector = mass1.pos - mass2.pos;	    //vector between the two masses
            double r = springVector.Lenght;					//distance between the two masses

            Vector3D force = new Vector3D();
            if (r != 0)
                force += (-springConstant) * (springVector / r) * (r - springRestLenght);

            force += -(mass1.vel - mass2.vel) * springFrictionConstant;

            mass1.ApplyForce(force);
            mass2.ApplyForce(-force);       //the opposite of force is applied to mass2

        }
        
    }// End Class Spring

    //________________________________________________________________________________________
    //________________________________________________________________________________________

    public class RopeSimulation : ParticlesSimulation //An object to simulate a rope interacting with a planer surface(XZ) and air(Y)
    {
        # region public Fields

        public bool isReleasedRope;  // showing that the conetction positon is falling

        public Spring[] springs;   //Springs binding the masses (there shall be [numOfMasses - 1] of them)
        public Vector3D gravitation;	//gravitational acceleration (gravity will be applied to all masses)

        public Vector3D ropeConnectionPos;	 //a point in space that is used to set the position of the first
        //mass in the system (mass with index 0)

        public Vector3D ropeConnectionVel;	 //a variable to move the ropeConnectionPos (by this, we can
        // swing the rope)

        public double groundRepulsionConstant;//a constant to represent how much the ground shall repel the masses

        public double groundFrictionConstant; //a constant of friction applied to masses by the ground
        //(used for the sliding of rope on the ground)

        public double groundAbsorptionConstant;//a constant of absorption friction applied to masses by the ground
        //(used for vertical collisions of the rope with the ground)

        public double groundHeight;	//a value to represent the z position value of the ground
        //(the ground is a planer surface facing +z direction)

        public double airFrictionConstant;				//a constant of air friction applied to masses

        # endregion

        # region Constructor
        public RopeSimulation(Vector3D conectionPOs,    //0. the positon that rope is connected
        int numOfMasses,			                    //1. the number of masses
        double m,										//2. weight of each mass
        double springConstant,							//3. how stiff the springs are
        double springRestLength,						//4. the length that a spring does not exert any force
        double springFrictionConstant,					//5. inner friction constant of spring
        Vector3D gravitation,							//6. gravitational acceleration in Y direction
        double airFrictionConstant,						//7. air friction constant
        double groundRepulsionConstant,					//8. ground repulsion constant
        double groundFrictionConstant,					//9. ground friction constant
        double groundAbsorptionConstant,				//10. ground absorption constant
        double groundHeight,					 	    //11. height of the ground (z position)
        bool isReleasedRope)
            : base(m, numOfMasses)
        {
            this.isReleasedRope = isReleasedRope;
            this.gravitation = gravitation;
            this.groundAbsorptionConstant = groundAbsorptionConstant;
            this.groundFrictionConstant = groundFrictionConstant;
            this.groundRepulsionConstant = groundRepulsionConstant;
            this.groundHeight = groundHeight;
            this.airFrictionConstant = airFrictionConstant;


            
            for (int i = 0; i < numOfMasses; i++)
            {

                this.masses[i].pos = 
                    new Vector3D(i * springRestLength + conectionPOs.x, conectionPOs.y, conectionPOs.z);
                 //Set x position of masses[a] with springLength distance 
                 // the rope is hold  1.5m on the ground in +Y direction
            }
            ropeConnectionPos = masses[0].pos;
            ropeConnectionVel = new Vector3D();

            this.springs = new Spring[numOfMasses - 1];
            for (int i = 0; i < numOfMasses - 1; i++)
                springs[i] = new Spring(masses[i], masses[i + 1], springConstant, springRestLength,
                    springFrictionConstant);      
          
        }
        # endregion

        # region Methods
        public void setRopeConnectionVel(Vector3D ropeConnectionVel)	//the method to set ropeConnectionVel
        {
            this.ropeConnectionVel = ropeConnectionVel;
        }
        # endregion

        # region overriden Methods
        public override void Solve()
        {
            for (int i = 0; i < numOfMass - 1; i++) //apply force of all springs
                springs[i].Solve();

            for (int i = 0; i < numOfMass; i++)
            {
                masses[i].ApplyForce(gravitation * masses[i].m);
                masses[i].ApplyForce(-airFrictionConstant * masses[i].vel);

                if (masses[i].pos.y < groundHeight)//Forces from the ground are applied if a mass
                //collides with the ground
                {
                    Vector3D v;        //A temporary Vector3D
                    v = masses[i].vel;
                    v.y = 0;             //omit the velocity component in y direction
                    //The velocity in y direction is omited because we will apply a friction force to create 
                    //a sliding effect. Sliding is parallel to the ground. Velocity in y direction will be used
                    //in the absorption effect(under ground).

                    masses[i].ApplyForce(-groundFrictionConstant * v);

                    v = masses[i].vel;						//get the velocity
                    v.x = 0;								//omit the x and z components of the velocity
                    v.z = 0;								//we will use v in the absorption effect

                    //above, we obtained a velocity which is vertical to the ground and it will be used in 
                    //the absorption force

                    if (v.y < 0)  //let's absorb energy only when a mass collides towards the ground
                        masses[i].ApplyForce(-groundAbsorptionConstant * v);


                    //The ground shall repel a mass like a spring. 
                    //By "Vector3D(0, groundRepulsionConstant, 0)" we create a vector in the plane normal direction 
                    //with a magnitude of groundRepulsionConstant.
                    //By (groundHeight - masses[i].pos.y) we repel a mass as much as it crashes into the ground.

                    Vector3D forceRepulse = new Vector3D(0, groundRepulsionConstant, 0) *
                        (groundHeight - masses[i].pos.y);
                    masses[i].ApplyForce(forceRepulse);

                }// end if 
            } //end for i
        }
        public override void Simulate(double dt)
        {
            base.Simulate(dt);                            //the super class shall simulate the masses

            if (isReleasedRope)
            {
                ropeConnectionVel = masses[0].vel;
                ropeConnectionPos = masses[0].pos;

                if (ropeConnectionVel == Vector3D.Zero)
                {
                    isReleasedRope = false;
                }
                //if (ropeConnectionPos.y < groundHeight)
                //{
                //    isReleasedRope = false;
                //    ropeConnectionVel = new Vector3D();
                //}
            }
            else
            {
                ropeConnectionPos += ropeConnectionVel * dt;  //iterate the positon of ropeConnectionPos          
                if (ropeConnectionPos.y < groundHeight)
                {
                    ropeConnectionPos.y = groundHeight;       //ropeConnectionPos shall not go under the ground
                    ropeConnectionVel.y = 0;
                }
            }

           
            masses[0].pos = ropeConnectionPos;			//mass with index "0" shall position at ropeConnectionPos
            masses[0].vel = ropeConnectionVel;			//the mass's velocity is set to be equal to ropeConnectionVel
        }
        # endregion

    } // End class RopeSimulation
}
