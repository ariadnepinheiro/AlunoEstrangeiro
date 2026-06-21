using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    internal class RecordManagerDesigner : ContainerControlDesigner, IDataSourceProvider
    {
        private Style frameStyle;

        public override string FrameCaption
        {
            get
            {
                return string.Empty;
            }
        }

        public override Style FrameStyle
        {
            get
            {
                if (this.frameStyle == null)
                {
                    this.frameStyle = new Style();
                }

                var rm = (RecordManager)this.Component;
                this.frameStyle.Font.Name = "Verdana";
                this.frameStyle.Font.Size = new FontUnit("XSmall");
                this.frameStyle.BackColor = rm.BackColor;
                this.frameStyle.ForeColor = rm.BorderColor;
                this.frameStyle.BorderStyle = rm.BorderStyle;
                this.frameStyle.BorderWidth = 3;

                return this.frameStyle;
            }
        }

        public string DataSource
        {
            get
            {
                var binding = this.DataBindings["DataSource"];
                if (binding != null)
                {
                    return binding.Expression;
                }

                return string.Empty;
            }

            set
            {
                if (value == null || value.Length == 0)
                {
                    this.DataBindings.Remove("DataSource", false);
                }
                else
                {
                    var binding = this.DataBindings["DataSource"];

                    if (binding == null)
                    {
                        binding = new DataBinding("DataSource", typeof (IEnumerable), value);
                        this.DataBindings.Add(binding);
                    }
                    else
                    {
                        binding.Expression = value;
                        this.DataBindings.Remove(binding); // para forçar evento Changed
                        this.DataBindings.Add(binding);
                    }
                }
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            PropertyDescriptor prop;

            base.PreFilterProperties(properties);

            

            prop = TypeDescriptor.CreateProperty(
                this.GetType(), 
                "DataSource", typeof (string), 
                new CategoryAttribute("Techne"), 
                new TypeConverterAttribute(typeof (DataSourceConverter)), 
                new BindableAttribute(true), 
                new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)
                );

            if (properties.Contains("DataSource"))
            {
                properties.Remove("DataSource");
            }

            properties.Add("DataSource", prop);

            
        }

        IEnumerable IDataSourceProvider.GetResolvedSelectedDataSource()
        {
            throw new NotImplementedException();
        }

        object IDataSourceProvider.GetSelectedDataSource()
        {
            var binding = this.DataBindings["DataSource"];
            if (binding == null)
            {
                return null;
            }

            var dataSource = binding.Expression;
            if (dataSource == null)
            {
                return null;
            }

            var componentSite = this.Component.Site;
            if (componentSite == null)
            {
                return null;
            }

            var container = (IContainer)componentSite.GetService(typeof (IContainer));
            if (container == null)
            {
                return null;
            }

            var comp = container.Components[dataSource];

            return comp;
        }
    }
}