﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace Laabaaaaaaaaaaaaaaaaaaa4_Server_
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;
        public Installer1()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "Ержан вставай,Саламалейкум";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
