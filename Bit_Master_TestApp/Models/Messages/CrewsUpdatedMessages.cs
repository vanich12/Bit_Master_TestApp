using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bit_Master_TestApp.Models.Messages
{
    public class CrewsUpdatedMessages
    {
        public List<int> UpdatedCarIds { get; set; } = [];
        public List<int> UpdatedDriverIds { get; set; } = [];
    }
}
