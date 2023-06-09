using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetHome.Core.Framework.Entites
{
    public abstract class BaseSweetHomePerspective
    {
        public int Id { get; set; }
        public List<BaseSweetHomeDevice> Devices { get; set; } = null!;
    }
}
