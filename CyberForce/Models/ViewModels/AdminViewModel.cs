using System;
using FluentFTP;
using OpenPop.Mime;

namespace CyberForce.Models.ViewModels
{
    public class AdminViewModel
    {
        public AuthUsers User { get; set; }
        public String[] ftpListItems { get; set; }
        public String[] messages { get; set;  }
    }
}

