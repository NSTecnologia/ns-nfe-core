using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.utilitarios
{
    public class ConsultarWebService
    {
        public class Body
        {
            public string CNPJCont { get; set; }
            public int UF { get; set; }
            public string tpAmb { get; set; }
            public string versao { get; set; }
        }

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public dynamic retStatusServico { get; set; }
            public string erros { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody)
        {
            string url = "https://nfe.ns.eti.br/util/wssefazstatus";
            try
            {
                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));
                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_CONSULTA_STATUS_WS]: " + ex.Message);
                return null;
            }
        }
    }
}
