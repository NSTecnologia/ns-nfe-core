using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ns_nfe_core.src.eventos;

namespace ns_nfe_core.src.nfe.eventos
{
    public class Cancelamento
    {
        public class Body
        {
            public string chNFe { get; set; }
            public string tpAmb { get; set; }
            public string dhEvento { get; set; }
            public string nProt { get; set; }
            public string xJust { get; set; }
        };

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public dynamic retEvento { get; set; }
            public string erro { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody, string tpDown = "X", string caminhoSalvar = @"NFe/Eventos/", bool exibeNaTela = false)
        {
            try
            {
                string url = "https://nfe.ns.eti.br/nfe/cancel";

                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));

                if (responseAPI.status == "200")
                {
                    if (responseAPI.retEvento.cStat == "135")
                    {
                        DownloadEvento.Body downloadEventoBody = new DownloadEvento.Body
                        {
                            chNFe = responseAPI.retEvento.chNFe,
                            tpAmb = requestBody.tpAmb,
                            tpDown = tpDown,
                            tpEvento = "CANC",
                            nSeqEvento = "1"
                        };

                        try
                        {
                            var retornoDownloadEvento = await DownloadEvento.sendPostRequest(downloadEventoBody, caminhoSalvar, exibeNaTela);
                        }

                        catch (Exception ex)
                        {
                            util.gravarLinhaLog("[ERRO_DOWNLOAD_CANCELAMENTO]: " + ex);
                        }

                    }
                }

                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_CANCELAR_NFE]: " + ex.Message);
                return null;
            }
        }
    }
}
