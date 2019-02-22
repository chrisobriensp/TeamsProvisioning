using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace COB.Teams.Provisioning
{
    public class TeamDetails
    {
        private JObject _teamTemplate = null;
        JArray _teamOwnerArray = new JArray();

        public TeamDetails(string TemplatePath)
        {
            string teamDetails = string.Empty;
            _teamTemplate = null;

            using (StreamReader sr = new StreamReader(TemplatePath))
            {
                _teamTemplate = (JObject)JToken.ReadFrom(new JsonTextReader(sr));
            }
        }

        public void AddSimpleProperty(string PropertyName, string Value, bool? AddAsObject)
        {
            if (_teamTemplate != null)
            {
                if (string.IsNullOrEmpty(PropertyName) || (string.IsNullOrEmpty(Value)))
                {
                    throw new ArgumentException("Error: Either the property name or value are not valid. Ensure you are passing valid values!");
                }
                _teamTemplate.Add(PropertyName, (AddAsObject.HasValue && AddAsObject.Value ? JToken.Parse(Value) : Value));
            }
            else
            {
                throw new InvalidOperationException("Error: The template has not been loaded. Ensure you have specified a valid template before calling this method!");
            }
        }

        public void AddOwner(string PropertyName, string OwnerID)
        {
            if (_teamTemplate != null)
            {
                if (!string.IsNullOrEmpty(OwnerID))
                {
                    _teamOwnerArray.Add(OwnerID);
                    AddSimpleProperty(PropertyName, _teamOwnerArray.ToString(Formatting.None), true);
                }
                else
                {
                    throw new ArgumentException("Error: The owner value is not valid. Ensure you are passing valid values!");
                }
            }
            else
            {
                throw new InvalidOperationException("Error: The template has not been loaded. Ensure you have specified a valid template before calling this method!");
            }
        }

        public override string ToString()
        {
            return _teamTemplate.ToString();
        }
    }
}
