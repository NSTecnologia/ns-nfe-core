using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.utilitarios
{
    public class ConsultarCadastro
    {
        public class Body
        {
            public string CNPJCont { get; set; }
            public string UF { get; set; }
            public string IE { get; set; }
            public string CNPJ { get; set; }
            public string CPF { get; set; }
        }

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public dynamic retConsCad { get; set; }
            public string nsNRec { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody)
        {
            string url = "https://nfe.ns.eti.br/util/conscad";
            try
            {
                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));
                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_CONSULTA_CADASTRO_CONTRIBINTE]: " + ex.Message);
                return null;
            }
        }
    }
}
