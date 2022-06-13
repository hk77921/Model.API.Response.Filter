using API.Response.Filter.Models;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace API.Response.Filter.Service
{
    public class UserProfile
    {
        public async Task<ClientProfile> GetAllUserProfile(string baseUrl, Params userParam)
        {
            string _userList = string.Empty;
            string JsonResponse = string.Empty;

            List<ClientProfile> _clientList = new List<ClientProfile>();
            ClientProfile ?_client = new ClientProfile();

            string _baseUrl = baseUrl;
            string _endPoint = "/users";

            try
            {
                _userList = await ExternalAPICall(_baseUrl, _endPoint, userParam);
                if (!string.IsNullOrEmpty(_userList))
                {
                    _clientList = GetUsersWithParams(_userList, userParam);
                    if (_clientList.Count > 0)
                    {
                        foreach (var client in _clientList)
                        {
                            _client = await GetUserProfileByID(baseUrl, client, userParam);

                        }
                    }
                    else
                        _client = null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return _client;
        }
        private async Task<ClientProfile> GetUserProfileByID(string baseUrl, ClientProfile? client, Params userParam)
        {
            string _client = string.Empty;
            string _baseUrl = baseUrl;
            string _endPoint = "/users/id:{0}";
            _endPoint = string.Format(_endPoint, client.ID);

            try
            {
                _client = await ExternalAPICall(_baseUrl, _endPoint, userParam);
                JsonNode _userNode = JsonNode.Parse(_client)!;

                JsonArray _certificationsNode = _userNode!["certifications"]!.AsArray();
                client.IsAttained = false;

                foreach (var _certificate in _certificationsNode)
                {

                    if (_certificate["unique_id"].ToString() == userParam.CertificateNumber && _certificate["course_id"].ToString() ==userParam.CourseCode)
                    {
                        client.CertificateNumber = _certificate["unique_id"].ToString();
                        client.CourseCode = _certificate["course_id"].ToString();
                        client.DateOfIssue = _certificate["issued_date"].ToString();
                        client.DateOfExpiry = _certificate["expiration_date"].ToString();
                        client.IsAttained = true;
                    }

                }
                if (client.IsAttained == false)
                {
                    client = null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return client;
        }
        public async Task<string> ExternalAPICall(string baseUrl, string endPoint, Params userParam)
        {
            string _apiResponse = string.Empty;

            string _baseUrl = baseUrl;
            string _apiUrl = _baseUrl + endPoint;

            try
            {
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();

                request.RequestUri = new Uri(_apiUrl);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Authorization", "Basic SG03U0tjRE5FR1J3OWtZR2NTSFlydHh5azdhTko1Og==");
                HttpResponseMessage response = httpClient.Send(request);

                _apiResponse = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return _apiResponse;
        }
        private List<ClientProfile> GetUsersWithParams(string userList, Params requestParam)
        {
            List<ClientProfile> _clientProfileList = new List<ClientProfile>();

            JsonNode document = JsonNode.Parse(userList)!;
            JsonNode _root = document.Root;
            JsonArray uesrArray = _root.AsArray();

            foreach (var item in uesrArray)
            {
                if ((item["last_name"] is not null) && (item["custom_field_1"] is not null))
                {
                    ClientProfile _client = new ClientProfile();

                    if (item["last_name"].ToString().ToLower().Trim() == requestParam.Surname.ToLower().Trim()
                        && item["custom_field_1"].ToString().Trim() == requestParam.DateOfBirth.Trim())
                    {

                        _client.Surname = item["last_name"].ToString();
                        _client.FirstName = item["first_name"].ToString();
                        _client.DOB = item["custom_field_1"].ToString();
                        _client.ID = item["id"].ToString();
                        _client.CustomerPhotoType = 1;
                        _client.CustomerPhoto = item["avatar"].ToString();

                        _clientProfileList.Add(_client);
                    }
                }

            }

            return _clientProfileList;


        }
    }
}
