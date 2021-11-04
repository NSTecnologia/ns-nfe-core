using Newtonsoft.Json;
using ns_nfe_core.src.commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_nfe_core.src.nfe.eventos
{
    public class Inutilizacao
    {
        public class Body
        {
            public int cUF { get; set; }
            public string tpAmb { get; set; }
            public string ano { get; set; }
            public string CNPJ { get; set; }
            public string serie { get; set; }
            public string nNFIni { get; set; }
            public string nNFFin { get; set; }
            public string xJust { get; set; }
        };

        public class Response
        {
            public string status { get; set; }
            public string motivo { get; set; }
            public dynamic retornoInutNFe { get; set; }
            public string erro { get; set; }

        }

        public static async Task<Response> sendPostRequest(Body requestBody, string tpDown = "X", string caminhoSalvar = @"NFe/Eventos/", bool exibeNaTela = false)
        {
            try
            {
                string url = "https://nfe.ns.eti.br/nfe/inut";

                var responseAPI = JsonConvert.DeserializeObject<Response>(await nsAPI.postRequest(url, JsonConvert.SerializeObject(requestBody)));

                if (responseAPI.status == "200")
                {
                    if (responseAPI.retornoInutNFe.cStat == "102")
                    {
                        DownloadInutilizacao.Body downloadEventoBody = new DownloadInutilizacao.Body
                        {
                            chave = responseAPI.retornoInutNFe.chave,
                            tpDown = tpDown,
                            tpAmb = requestBody.tpAmb,
                        };

                        try
                        {
                            var retornoDownloadEvento = await DownloadInutilizacao.sendPostRequest(downloadEventoBody, caminhoSalvar, exibeNaTela);
                        }

                        catch (Exception ex)
                        {
                            util.gravarLinhaLog("[ERRO_DOWNLOAD_INUTILIZACAO]: " + ex);
                        }

                    }
                }

                return responseAPI;
            }

            catch (Exception ex)
            {
                util.gravarLinhaLog("[ERRO_INUTILIZAR_NFE]: " + ex.Message);
                return null;
            }
        }
    }
}
