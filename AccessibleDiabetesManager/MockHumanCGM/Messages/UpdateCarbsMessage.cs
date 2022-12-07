using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockHumanCGM.Messages
{
    public class UpdateCarbsMessage : ValueChangedMessage<int>
    {
        public UpdateCarbsMessage(int value) : base(value)
        {
        }
    }
}