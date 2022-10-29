using System;
using FluentFTP;
using OpenPop.Mime;

namespace CyberForce.Models.ViewModels
{
    public class AdminViewModel
    {
        public AuthUsers User { get; set; }
        public List<FtpListItem> ftpListItems { get; set;  } = new();
        public List<Message> messages { get; set;  } = new();
    }
}

