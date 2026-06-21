using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    [Serializable]
    public class MenuItem
    {
        public bool topMost { get; set; }
        public string title { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public bool group { get; set; }
        public string roles { get; set; }
        public List<MenuItem> children { get; set; }
        public MenuItem parent { get; set; }

        public MenuItem(MenuItem parent)
        {
            children = new List<MenuItem>();
            if (parent != null)
            {
                this.parent = parent;
            }

            topMost = false;
        }

        public MenuItem()
        {
            children = new List<MenuItem>();

            topMost = false;
        }

    }
}