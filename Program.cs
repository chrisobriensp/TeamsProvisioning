using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using COB.Teams.Provisioning.Entities;

namespace COB.Teams.Provisioning
{
    class Program
    {
        private static readonly string DISPLAY_NAME_ATTRIBUTE = "displayName";
        private static readonly string DESCRIPTION_ATTRIBUTE = "description";
        private static readonly string OWNER_ATTRIBUTE = "owners@odata.bind";
        private static readonly string GRAPH_TEAMS_ENDPOINT = "https://graph.microsoft.com/beta/teams";
        private static readonly string GRAPH_USERS_ENDPOINT = "https://graph.microsoft.com/v1.0/users";

        private static string _accessToken = null;
        private static HttpClient _httpClient = new HttpClient();

        static void Main(string[] args)
        {
            // NOTE: in the real world you'd want to fetch these from Azure Key Vault..
            string clientId = "[AAD CLIENT ID HERE]";
            string clientSecret = "[AAD CLIENT SECRET HERE]";
            string tenantDomain = "[TENANT PREFIX HERE].onmicrosoft.com";

            fetchAndStoreAccessToken(clientId, clientSecret, tenantDomain);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            // NOTE: update these details as required, or pull in from another source (e.g. a file).. 
            string ownerUPN = "cob@chrisobrien.com";
            string displayName = "[TEAM NAME HERE]";
            string description = "[TEAM DESCRIPTION HERE]";
            string templatePath = "Templates\\COBCoreTeamTemplate.json";

            CreateTeam(templatePath, displayName, description, ownerUPN);
        }

        private static void CreateTeam(string templatePath, string teamDisplayName, string teamDescription, string teamOwnerID)
        {
            TeamDetails teamConfig = new TeamDetails(templatePath);
            teamConfig.AddSimpleProperty(DISPLAY_NAME_ATTRIBUTE, teamDisplayName, false);
            teamConfig.AddSimpleProperty(DESCRIPTION_ATTRIBUTE, teamDescription, false);
            teamConfig.AddOwner(OWNER_ATTRIBUTE, teamOwnerID);

            string teamConfigString = teamConfig.ToString().Trim();

            StringContent postContent = new StringContent(teamConfigString, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, GRAPH_TEAMS_ENDPOINT);
            request.Content = postContent;
            var response = _httpClient.SendAsync(request).Result;
            var createdTeamDetails = response.Headers.Location;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Successfully created team - details: '{0}'", createdTeamDetails));
            }
            else
            {
                Console.WriteLine("ERROR: Failed to create Team.");
            }

            Console.ReadLine();
        }

        private async static Task<string> GetGraphUserIdByUPN(string UserUPN)
        {
            string userUPN = null;
            string graphUserDetailsEndPoint = string.Format("{0}/{1}", GRAPH_USERS_ENDPOINT, UserUPN);

            var request = new HttpRequestMessage(HttpMethod.Get, graphUserDetailsEndPoint);
            var response = _httpClient.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string userResponse = await response.Content.ReadAsStringAsync();
                SimpleGraphUser userResponseObject = JsonConvert.DeserializeObject<SimpleGraphUser>(userResponse);
                userUPN = userResponseObject.id;
            }
            else
            {
                Console.WriteLine(string.Format("ERROR: Failed to fetch user with UPN '{0}'.", UserUPN));
            }

            return userUPN;
        }

        public static void fetchAndStoreAccessToken(string clientId, string clientSecret, string tenantDomain)
        {
            string authority = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/", "https://login.windows.net", tenantDomain);
            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(clientId, clientSecret);

            var result = authContext.AcquireTokenAsync("https://graph.microsoft.com", clientCredential).Result;
            _accessToken = result.AccessToken;
        }
    }
}
