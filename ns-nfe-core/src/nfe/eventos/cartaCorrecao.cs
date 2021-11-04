using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using ns_nfe_core.src.eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.eventos
{
    public class CartaCorrecao
    {
        public class Body
        {
            public string chNFe { get; set; }
            public string tpAmb { get; set; }
            public string dhEvento { get; set; }
            public string nSeqEvento { get; set; }
            public string xCorrecao { get; set; }
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
                string url = "https://nfe.ns.eti.br/nfe/cce";

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
                            tpEvento = "CCE",
                            nSeqEvento = requestBody.nSeqEvento
                        };

                        try
                        {
                            var retornoDownloadEvento = await DownloadEvento.sendPostRequest(downloadEventoBody, caminhoSalvar, exibeNaTela);
                        }

                        catch (Exception ex)
                        {
                            util.gravarLinhaLog("[ERRO_DOWNLOAD_CCE]: " + ex);
                        }

                    }
                }

                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_CORRIGIR_NFE]: " + ex.Message);
                return null;
            }
        }
    }
}
