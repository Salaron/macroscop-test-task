using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Client.Model;

namespace Client.Network
{
    public class PalindromeCheck
    {
        [JsonProperty]
        public string Input { get; private set; }
        [JsonProperty]
        public bool IgnoreSymbols { get; private set; }

        private FileModel _file;

        public PalindromeCheck(FileModel file, bool ignoreSymbols)
        {
            _file = file;
            Input = _file.GetContent();
            IgnoreSymbols = ignoreSymbols;
        }

        public async Task<bool> Check()
        {
            try
            {
                _file.UpdateStatus(FileStatus.Loading);

                HttpClient client = new()
                {
                    BaseAddress = new Uri(Settings.ServerUrl)
                };
                var requestBody = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("isPalindrome", requestBody);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<PalindromeResponse>(responseBody);
                    if (responseJson != null)
                    {
                        if (responseJson.Result == true)
                        {
                            _file.UpdateStatus(FileStatus.Palindrome);
                        }
                        else
                        {
                            _file.UpdateStatus(FileStatus.NotPalindrome);
                        }
                    }
                    else
                    {
                        _file.UpdateStatus(FileStatus.Error);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    _file.UpdateStatus(FileStatus.Overload);
                }
                else
                {
                    _file.UpdateStatus(FileStatus.Error);
                }

                return true;
            }
            catch
            {
                _file.UpdateStatus(FileStatus.Error);
                return false;
            }
        }
    }
}
