using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.utilitarios
{
    public class ListarNSNRec
    {
        public class Body
        {
            public string chNFe { get; set; }
        }

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public dynamic nsNRecs { get; set; }
            public string erros { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody)
        {
            string url = url = "https://nfe.ns.eti.br/util/list/nsnrecs";
            try
            {
                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));
                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_LISTAR_NSNREC]: " + ex.Message);
                return null;
            }
        }
    }
}
