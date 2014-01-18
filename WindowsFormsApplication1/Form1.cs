using Leap;
using Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        GestureApp gestureApp;
        bool comeon = true;
        Thread listen = null;
        Controller controller = new Controller();
        public Form1()
        {
            InitializeComponent();
            gestureApp = new GestureApp();
            gestureApp.debbugMode = false;
            controller.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);

            controller.AddListener(gestureApp);
            listen = new Thread(listenOptions);
            listen.Start();
        }
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelOption.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelOption.Text = text;
            }
        }
        private void SetTextNext(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelNext.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelNext.Text = text;
            }
        }
        private void SetTextPrev(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelPrev.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelPrev.Text = text;
            }
        }
        public void listenOptions()
        {
            string last_option = Option.ZOOMPAN.ToString();
            while (comeon)
            {
                if (OptionsController.Instance.getOption() == Option.ROTATE && last_option != Option.ROTATE.ToString())
                {
                    last_option = Option.ROTATE.ToString();
                    SetText( "ROTATE");
                    pictureBoxLogo1.Image = Properties.Resources.rotar_izq_logo;
                    pictureBoxLogo2.Image = Properties.Resources.rotar_der_logo;
                    pictureBoxAnimation.Image = Properties.Resources.rotate;
                    pictureBoxNext.Image = null;
                    pictureBoxPrev.Image = Properties.Resources.brillo_logo;
                }
                if (OptionsController.Instance.getOption() == Option.CONTRAST && last_option != Option.CONTRAST.ToString())
                {
                    last_option = Option.CONTRAST.ToString();
                    SetText( "CONTR. + BRIGHT.");
                    pictureBoxLogo1.Image = Properties.Resources.contraste_logo;
                    pictureBoxLogo2.Image = Properties.Resources.brillo_logo;
                    pictureBoxAnimation.Image = Properties.Resources.contrast;
                    pictureBoxNext.Image = Properties.Resources.rotar_der_logo;
                    pictureBoxPrev.Image = Properties.Resources.zoom_logo;
                    
                }
                if (OptionsController.Instance.getOption() == Option.ZOOMPAN && last_option != Option.ZOOMPAN.ToString())
                {
                    last_option = Option.ZOOMPAN.ToString();
                    SetText("ZOOM + PANNING");
                    pictureBoxLogo1.Image = Properties.Resources.zoom_logo;
                    pictureBoxLogo2.Image = Properties.Resources.panning_logo;
                    pictureBoxAnimation.Image = Properties.Resources.zoom_panning;
                    pictureBoxNext.Image = Properties.Resources.brillo_logo;
                    pictureBoxPrev.Image = Properties.Resources.click_logo;
                    
                }
                if (OptionsController.Instance.getOption() == Option.NONE && last_option != Option.NONE.ToString())
                {
                    last_option = Option.NONE.ToString();
                    SetText("MOUSE CONTROL");
                    pictureBoxLogo1.Image = Properties.Resources.mouse;
                    pictureBoxLogo2.Image = Properties.Resources.click_logo;
                    pictureBoxAnimation.Image = Properties.Resources.click;
                    pictureBoxNext.Image = Properties.Resources.zoom_logo;
                    pictureBoxPrev.Image = null;

                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            comeon = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listen != null)
                listen.Abort();
            comeon = false;
        }

        private void labelOption_Click(object sender, EventArgs e)
        {

        }

        private void labelOption_TextChanged(object sender, EventArgs e)
        {
            
            
        }
        
    }
}
