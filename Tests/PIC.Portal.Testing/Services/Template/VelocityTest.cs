using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using NUnit.Framework;
using NVelocity;
using NVelocity.Runtime.Parser.Node;
using NVelocity.Context;
using PIC.Portal.Template;

namespace PIC.Portal.Testing.Services.Template
{
    [TestFixture]
    public class VelocityTest
    {
        [Test]
        public void Test_RuntimeInstance()
        {
            TmplContext tmplCtx = new TmplContext();

            tmplCtx.Set("key", "value");
            tmplCtx.Set("firstName", "Cort");
            tmplCtx.Set("lastName", "Schaefer");

            Hashtable h = new Hashtable();
            h.Add("foo", "bar");
            tmplCtx.Set("hashtable", h);

            AddressData address = new AddressData();
            address.Address1 = "9339 Grand Teton Drive";
            address.Address2 = "Office in the back";
            tmplCtx.Set("address", address);

            ContactData contact = new ContactData();
            contact.Name = "Cort";
            contact.Address = address;
            tmplCtx.Set("contact", contact);

            NVelocity.Runtime.RuntimeInstance ri = new NVelocity.Runtime.RuntimeInstance();
            ri.Init();

            VelocityContext c = tmplCtx.ToVelocityContext();

            object val = TmplHelper.Execute("$contact", c, ri);
            Assert.IsTrue(val is ContactData);

            val = TmplHelper.Execute("$contact.Address", c, ri);
            Assert.IsTrue(val is AddressData);

            val = TmplHelper.Execute("$contact.Address.Address2", c, ri);
            Assert.IsTrue(val is string);

            val = TmplHelper.Execute("${contact.Address.Address2}X", c, ri);
            Assert.IsTrue(val is string);
        }


        // inner classes to support tests --------------------------

        public class ContactData
        {
            private String name = String.Empty;
            private AddressData address = new AddressData();

            public String Name
            {
                get { return name; }
                set { name = value; }
            }

            public AddressData Address
            {
                get { return address; }
                set { address = value; }
            }
        }

        public class AddressData
        {
            private String address1 = String.Empty;
            private String address2 = String.Empty;

            public String Address1
            {
                get { return this.address1; }
                set { this.address1 = value; }
            }

            public String Address2
            {
                get { return this.address2; }
                set { this.address2 = value; }
            }
        }
    }
}
