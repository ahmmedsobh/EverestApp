using EverestApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EverestApp.CustomControls.CustomCells
{
    class MyDataTemplateSelector : DataTemplateSelector
    {
        public MyDataTemplateSelector()
        {
            // Retain instances!
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;
            return (messageVm.Sender == null || messageVm.Sender == "") ? this.outgoingDataTemplate : this.incomingDataTemplate;
        }

        private readonly DataTemplate incomingDataTemplate; 
        private readonly DataTemplate outgoingDataTemplate;
    }
}
