using Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Leap;
namespace Servicio
{
    public partial class Service1 : ServiceBase
    {
         GestureApp gestureApp = new GestureApp();
         Controller controller = new Controller();
        public Service1()
        {
            InitializeComponent();
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("Codigo"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Codigo", "Evento");
            }
            eventLog1.Source = "Codigo";
            eventLog1.Log = "Evento";
        }

        protected override void OnStart(string[] args)
        {
            controller.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);
            eventLog1.WriteEntry("en start2");
            controller.AddListener(gestureApp);
        }

        protected override void OnStop()
        {
            controller.RemoveListener(gestureApp);
        }
    }
}
