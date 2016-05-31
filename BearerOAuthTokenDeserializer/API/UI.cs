using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BearerOAuthTokenDeserializer
{
    public class UI
    {
        public static void RenderTree(AuthenticationTicket t, TreeView tvBearer)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            tvBearer.Nodes.Clear();
            var n = tvBearer.Nodes;
            var rn = n.Add("token", "AuthenticationToken");

            if (t != null && t.Identity != null)
            {
                System.Security.Claims.ClaimsIdentity identity = t.Identity;
                Type identityType = identity.GetType();
                PropertyInfo[] properties = identityType.GetProperties(flags);

                foreach (PropertyInfo p in properties)
                {
                    //Console.WriteLine("Name: " + p.Name + ", Value: " + p.GetValue(obj, null));
                    var isIEnumerable = p.IsPropertyACollection();
                    var cn1 = rn.Nodes.Add(p.Name + "Label", p.Name);
                    var val = p.GetValue(identity);

                    switch (p.Name)
                    {
                        case "Claims":
                            var claims = val as IEnumerable<System.Security.Claims.Claim>;
                            foreach (var c in claims)
                            {
                                var labNode = cn1.Nodes.Add("Claim", "ClaimType: " + c.Type);

                                foreach (var pi in c.GetType().GetProperties(flags))
                                {
                                    if (pi.Name != "Type")
                                    {
                                        var clab = new TreeNode(pi.Name);
                                        clab.Nodes.Add(new TreeNode(pi.GetValue(c)?.ToString()));
                                        labNode.Nodes.Add(clab);
                                    }

                                }
                            }
                            break;
                        default:
                            var tn = new TreeNode(val?.ToString());
                            cn1.Nodes.Add(tn);
                            cn1.Expand();
                            break;
                    }

                }
            }

            if (t.Properties != null)
            {
                AuthenticationProperties authenticationProperties = t.Properties;
                Type authenticationPropertiesType = authenticationProperties.GetType();

                PropertyInfo[] props = authenticationPropertiesType.GetProperties(flags);

                var propertiesNode = rn.Nodes.Add("PropertiesLabel", "Properties");

                foreach (var p in props)
                {
                    var isIEnumerable = p.IsPropertyACollection();
                    var val = p.GetValue(authenticationProperties);
                    var ln = new TreeNode(p.Name);
                    TreeNode tn = null;

                    if (isIEnumerable && val != null)
                    {
                        var tnd = new TreeNode("Dictionary");
                        propertiesNode.Nodes.Add(tnd);
                        //propertiesNode.Nodes.Add(ln);
                        var dict = (IDictionary<string, string>) val;
                        foreach (var kv in dict)
                        {
                            var ln1 = new TreeNode(kv.Key);
                            var tnv = new TreeNode(kv.Value);
                            ln1.Nodes.Add(tnv);
                            tnd.Nodes.Add(ln1);
                        }
                    }
                    else
                    {
                        tn = new TreeNode(val == null ? "null" : val.ToString());
                        ln.Nodes.Add(tn);
                        propertiesNode.Nodes.Add(ln);
                        tn.Expand();
                    }
                }
            }
            rn.Expand();
        }
    }

    static class Extensions
    {
        public static bool IsPropertyACollection(this PropertyInfo property)
        {
            return (!typeof(String).Equals(property.PropertyType) &&
                typeof(IEnumerable).IsAssignableFrom(property.PropertyType));
        }

    }
}