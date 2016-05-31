using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.IdentityModel.Claims;

namespace BearerOAuthTokenDeserializer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var exampleBearerTokenValue=
            "nYQZ24q4huBJvAMBi18ufVvx6DKvbHgOkGG9CgHLFTEJ-4FqN64XG3p4srpRmCMlUdFpbUV32Q-UG6_ltdmeGXDRiTOl53O73Rw6-9tF50CoeDArndlcs1vz-efsQQ6LdISA5hNfZFdtN72QzrVt9lKl2FWe8e5lWNsw7zVucH10To-joWzl5NI5b9dKg83Q5MAnj8m2TolzGLIH8W4mrDCoax1K4MaLVX-JjSyItG14hhaUeoHpKcsKqMrKdCKAk2BsYQ";

            tbBearerToken.Text = exampleBearerTokenValue;
        }

        private void btnDeserialize_Click(object sender, EventArgs e)
        {

            lblError.Text = string.Empty;
            AuthenticationTicket token = null;
            try
            {
                token = DataProtector.Create().Unprotect(tbBearerToken.Text.Trim());
            }
            catch (Exception ex)
            {
                lblError.Text = "DataProtectorException:  " + Environment.NewLine + ex.Message;
            }

            try
            {
                UI.RenderTree(token, tvBearer);
            }
            catch (Exception ex)
            {
                lblError.Text = "UI.Render.Exception:  " + Environment.NewLine + ex.Message;
            }
        }

    }

}
