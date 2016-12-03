using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JoomlaContentEditor.Model
{
    public static class Network
    {
        public static async Task<bool> IsInternetAvailable()
        {
            WebClient client = new WebClient();

            try
            {
                using (await client.OpenReadTaskAsync("http://zs-1.edu.pl"))
                {
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
