using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.eventos
{
    public class DownloadInutilizacao
    {
        public class Body
        {
            public string chave { get; set; }
            public string tpAmb { get; set; }
            public string tpDown { get; set; }
        };

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public dynamic retInut { get; set; }
            public string erros { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody, string caminhoSalvar = @"NFe/Eventos/", bool exibeNaTela = false)
        {
            try
            {
                string url = "https://nfe.ns.eti.br/nfe/get/inut";

                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));

                if (responseAPI.retInut.json != null)
                {
                    var json = (string)responseAPI.retInut.json;
                    var chave = (string)responseAPI.retInut.chave;
                    util.salvarArquivo(caminhoSalvar, chave, "-procInut.json", json);
                }

                if (responseAPI.retInut.xml != null)
                {
                    var xml = (string)responseAPI.retInut.xml;
                    var chave = (string)responseAPI.retInut.chave;
                    util.salvarArquivo(caminhoSalvar, chave, "-procInut.xml", xml);
                }

                if (responseAPI.retInut.pdf != null)
                {
                    var pdf = (string)responseAPI.retInut.pdf;
                    var chave = (string)responseAPI.retInut.chave;
                    util.salvarArquivo(caminhoSalvar, chave, "-procInut.pdf", pdf);

                    if (exibeNaTela)
                    {
                        util.exibirPDF(caminhoSalvar, chave, "-procInut.pdf");
                    };
                }

                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_DOWNLOAD_INUTILIZACAO]: " + ex.Message);
                return null;
            }
        }
    }
}
