using System;
using System.Net.Http;
using System.Threading.Tasks;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGet_Click(object sender, EventArgs e)
    {
        var t = CallApi();
        litResult.Text = t.Result ?? "No response";
    }

    private async Task<string?> CallApi()
    {
        using (HttpClient c = new HttpClient())
        {
            // Replace with correct API URL when running locally (IIS Express or configured host)
            var resp = await c.GetAsync("http://localhost:5000/api/values/1");
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadAsStringAsync();
        }
    }
}
