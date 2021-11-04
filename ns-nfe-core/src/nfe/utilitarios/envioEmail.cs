using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.utilitarios
{
    public class EnvioEmail
    {
        public class Body
        {
            public string chNFe { get; set; }
            public string tpAmb { get; set; }
            public bool anexarPDF { get; set; }
            public bool anexarEvento { get; set; }
            public string[] email { get; set; }
        }

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public string erros { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody)
        {
            string url = "https://nfe.ns.eti.br/util/resendemail";
            try
            {
                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));
                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_UTIL_ENVIO_EMAIL]: " + ex.Message);
                return null;
            }
        }
    }
}
