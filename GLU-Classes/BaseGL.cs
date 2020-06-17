using System;
using System.Windows.Forms;

namespace OpenGL
{
	/// <summary>
	/// BaseGLControl is the base class for a control that displays OpenGL
	/// rendering within it.  Developers should inherit from it and implement
	/// overrides on InitializeGL, ResizeGL, and OnPaint.
	/// </summary>
	public class BaseGLControl : System.Windows.Forms.UserControl
    {
        # region Member data
        uint m_uint_HWND = 0;
        uint m_uint_DC = 0;
        uint m_uint_RC = 0;
        System.ComponentModel.Container components = null;
        # endregion

        # region Events
       
        # endregion

        # region property
        /// <summary>
		/// Gets the window handle for the control.
		/// </summary>
		public uint HWND
		{
			get{ return m_uint_HWND; }
		}
		
		/// <summary>
		/// Gets the device context for the control.
		/// </summary>
		public uint DC
		{
			get{ return m_uint_DC;}
		}
		
		/// <summary>
		/// Gets the rendering context for the control.
		/// </summary>
		public uint RC
		{
			get{ return m_uint_RC; }
        }


        # endregion


        public BaseGLControl()
		{
			InitializeComponent();
			this.components = new System.ComponentModel.Container();
			//Attach load and size change event handlers
			this.Load += new EventHandler(InitializeGL);
			this.SizeChanged += new EventHandler(ResizeGL);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(m_uint_RC != 0)
				{
					WGL.wglDeleteContext(m_uint_RC);
					m_uint_RC = 0;
					//Don't release DC!! don't know why, but bad!
				}
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			// 
			// BaseGLControl
			// 
			this.Name = "BaseGLControl";
			this.Size = new System.Drawing.Size(300, 300);
		}
		#endregion

		protected override void OnPaintBackground( PaintEventArgs e )
		{ 
			// This overrides the System.Windows.Forms.Control protected method
			// "OnPaintBackground()" so that we don't clear the client area of
			// this form window -- so the OpenGL doesn't flicker on each frame.
		}

		/// <summary>
		/// OnPaint is called when the control needs to be refreshed.  OpenGL
		/// specific drawing should be done by overriding this method.
		/// </summary>
		protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )
		{
			if(m_uint_DC == 0 || m_uint_RC == 0)
				return;

         
			//Just clear the screen
			GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
			GL.glLoadIdentity();
            
			WGL.wglSwapBuffers(m_uint_DC);
		}

		/// <summary>
		///	Event handler called when the form is loaded.  It retrieves the controls
		///	window handle and device context and creates the rendering context.
		/// </summary>
		protected virtual void InitializeGL(object sender, EventArgs e)
		{
			m_uint_HWND = (uint)this.Handle.ToInt32();
			m_uint_DC   = WGL.GetDC(m_uint_HWND);
			// CAUTION: Not doing the following WGL.wglSwapBuffers() on the DC will
			// result in a failure to subsequently create the RC.
			WGL.wglSwapBuffers(m_uint_DC);

			//Get the pixel format		
			WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
			WGL.ZeroPixelDescriptor(ref pfd);
			pfd.nVersion        = 1; 
			pfd.dwFlags         = (WGL.PFD_DRAW_TO_WINDOW |  WGL.PFD_SUPPORT_OPENGL |  WGL.PFD_DOUBLEBUFFER); 
			pfd.iPixelType      = (byte)(WGL.PFD_TYPE_RGBA);
			pfd.cColorBits      = 32;
			pfd.cDepthBits      = 32;
			pfd.iLayerType      = (byte)(WGL.PFD_MAIN_PLANE);

			int pixelFormatIndex = 0;
			pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
			if(pixelFormatIndex == 0)
			{
				MessageBox.Show("Unable to retrieve pixel format");
				return;
			}

			if(WGL.SetPixelFormat(m_uint_DC,pixelFormatIndex,ref pfd) == 0)
			{
				MessageBox.Show("Unable to set pixel format");
				return;
			}
			//Create rendering context
			m_uint_RC = WGL.wglCreateContext(m_uint_DC);
			if(m_uint_RC == 0)
			{
				MessageBox.Show("Unable to get rendering context");
				return;
			}
			if(WGL.wglMakeCurrent(m_uint_DC,m_uint_RC) == 0)
			{
				MessageBox.Show("Unable to make rendering context current");
				return;
			}
			//Set up OpenGL related characteristics
			ResizeGL(null,null);
            GL.glClearColor(0, 0, 0, 0.0f);
            //GL.glEnable(GL.GL_DEPTH_TEST);
			//GL.glDepthFunc(GL.GL_LEQUAL);
			//GL.glEnable(GL.GL_CULL_FACE);
			//GL.glCullFace(GL.GL_BACK);
		}

		/// <summary>
		/// Event handler called when the control is resized.
		/// </summary>
		protected virtual void ResizeGL(object sender, EventArgs e)
		{
			if(m_uint_DC == 0 || m_uint_RC == 0)
				return;
			if(this.Width == 0 || this.Height == 0)
				return;
			WGL.wglMakeCurrent(m_uint_DC,m_uint_RC);
			GL.glViewport( 0, 0, this.Width, this.Height );

			GL.glMatrixMode ( GL.GL_PROJECTION );
			GL.glLoadIdentity();
			GLU.gluPerspective( 45.0, (double) Width / (double)Height, 1.0,100.0);
			GL.glMatrixMode ( GL.GL_MODELVIEW );
			GL.glLoadIdentity();

            //GL.glClearColor(0, 0, 0, 0.5f);
            //GL.glClearDepth(1);
            //GL.glShadeModel(GL.GL_SMOOTH);
            //GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);
		}
	}
}
